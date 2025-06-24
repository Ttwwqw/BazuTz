
using System;
using System.Collections;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.AI;
using System.Linq;

public class LazyEnemy : NetworkBehaviour {

    public event Action<LazyEnemy> OnDeath;

    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private HealthComponent _hp;

    [SerializeField] private Transform _shootingPoint;
    [SerializeField] private Bullet _bulletPrefab;

    [SerializeField] private LayerMask _raysLayermask;

    private float _reactionDistance = 8f;
    private float _shootingRange = 10f;

    [SerializeField] private State _state = State.Idle;
    private Target _currentTarget;
    private CoroutineWrapper _updatingRoutine = null;
    
    private Vector3 _originalPosition;

	public override void OnNetworkSpawn() {
        if (IsServer) {
            _originalPosition = transform.position;
            _hp.OnHealthValueChanged += OnHpChanged;
            _updatingRoutine?.Stop();
            _updatingRoutine = CoroutineBehavior.StartCoroutine(UpdatingCoroutine());
        }
	}

	public override void OnNetworkDespawn() {
        _hp.OnHealthValueChanged -= OnHpChanged;
        _updatingRoutine?.Stop();
    }

	private void OnHpChanged(float old, float curr) {
        if (IsServer && curr <= 0) {
            OnDeath?.Invoke(this);
        }
    }

	private IEnumerator UpdatingCoroutine() {

        float _shootingTimer = 1f;

        while (true) {

            if (_currentTarget.target == null) {
                _state = State.Idle;
            }

            switch (_state) {
            case State.Idle:

                if (TrySelectNewTarget()) {
                    _state = State.Ñhase;
                } else {
                    _agent.SetDestination(_originalPosition);
                }

                break;
            case State.Ñhase:

                if (_currentTarget.target == null) {
                    _state = State.Idle;
                    _currentTarget = default;
                    break;
                }
                
                if (InSightLine(_currentTarget.coll) && Vector3.Distance(_currentTarget.target.position, transform.position) <= _shootingRange) {
                    _state = State.Attack;
                } else if (Vector3.Distance(_originalPosition, transform.position) > 20f) {
                    _state = State.Idle;
                    _currentTarget = default;
                } else {
                    TryFindClosestTarget();
                    _agent.SetDestination(_currentTarget.target.position);
                }

                break;
            case State.Attack:

                if (!InSightLine(_currentTarget.coll) || Vector3.Distance(_currentTarget.target.position, transform.position) > _shootingRange) {
                    TryFindClosestTarget();
                     _state = State.Ñhase;
                } else {
                    _agent.SetDestination(transform.position);
                    Shoot();
                }

                break;
            }

            _shootingTimer += Time.deltaTime;
            yield return null;

        }

        void Shoot() {
            if (_shootingTimer >= 1f) {
                var bullet = NetworkManager.Singleton.SpawnManager.InstantiateAndSpawn(
                    _bulletPrefab.GetComponent<NetworkObject>(),
                    0, false, false, false,
                    _shootingPoint.position,
                    _shootingPoint.rotation);
                bullet.GetComponent<Bullet>().Setup("Player", 5f, ((_currentTarget.target.position + Vector3.up) - _shootingPoint.position).normalized);
                _shootingTimer = 0f;
            }
        }

    }

    // Without changing target on null
    private void TryFindClosestTarget() {
        var targets = GetTargetsInSightLine();
        if (targets.Length > 0) {
            _currentTarget = new Target() { damageble = targets[0].GetComponent<IDamageable>(), target = targets[0].transform, coll = targets[0] };
        }
    }

	private bool TrySelectNewTarget() {

        var targets = GetTargetsInSightLine();

        if (targets.Length > 0) {
            if (_currentTarget.coll != null && targets.ContaitsWhere(x => x == _currentTarget.coll)) {
                return false;
            } else {
                _currentTarget = new Target() { damageble = targets[0].GetComponent<IDamageable>(), target = targets[0].transform, coll = targets[0] };
                return true;
            }
        }

        bool lastIsNotNull = _currentTarget.target != null;
        if (lastIsNotNull && Vector3.Distance(_currentTarget.target.position, transform.position) < 20f) {
            return false;
        }

        _currentTarget = default;
        return lastIsNotNull;

    }

    private Collider[] GetTargetsInSightLine() {

        var hits = Physics.OverlapSphere(transform.position, _reactionDistance);
        var targets = (from h in hits where h.gameObject.CompareTag("Player") && h.gameObject.TryGetComponent<IDamageable>(out var d) && InSightLine(h) select h).ToArray();

        Array.Sort(targets, (x, y) => { return Vector3.Distance(x.transform.position, transform.position).CompareTo(Vector3.Distance(y.transform.position, transform.position)); });

        return targets;

    }

    bool InSightLine(Transform t) {
        return Physics.Raycast(_shootingPoint.position, (t.position - _shootingPoint.position).normalized, out var hit, 100, _raysLayermask) && hit.collider.transform == t;
    }
    bool InSightLine(Collider coll) {
        return Physics.Raycast(_shootingPoint.position, (coll.bounds.center - _shootingPoint.position).normalized, out var hit, _raysLayermask) && hit.collider == coll;
    }

    private enum State {
        Idle, Ñhase, Attack
    }

    private struct Target {
        public IDamageable damageble;
        public Collider coll;
        public Transform target;
    }

}

