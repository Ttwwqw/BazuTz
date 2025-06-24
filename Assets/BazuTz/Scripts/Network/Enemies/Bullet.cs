
using UnityEngine;
using Unity.Netcode;

public class Bullet : NetworkBehaviour {

    [SerializeField] private Rigidbody _rigi;

    private string _reactionTag;
    private float _damage;

    public void Setup(string reactionTag, float damage, Vector3 startDiraction) {
        
        _damage = damage;
        _reactionTag = reactionTag;

        _rigi.AddForce(startDiraction * 25f, ForceMode.Impulse);

    }

	private void OnTriggerEnter(Collider other) {

        if (!IsServer) {
            return;
        }

        if (other.CompareTag(_reactionTag) && other.TryGetComponent<IDamageable>(out var d)) {
            d.DoDamage(_damage);
            NetworkObject.Despawn(true);
        } else {
            NetworkObject.Despawn(true);
        }

	}

}
