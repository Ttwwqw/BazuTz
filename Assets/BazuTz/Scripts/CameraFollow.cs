
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    [SerializeField, Range(2f, 15f)] private float _followDistance = 12f;
    [SerializeField, Range(1f, 50f)] private float _clampPower = 5f;

    private Transform _trackTarget = null;
    private Vector3 _targetPos;

    public void SetCameraFollowTarget(Transform transform) {

        _trackTarget = transform;

        if (_trackTarget != null) {
            transform.position = _targetPos = _trackTarget.transform.position - (transform.rotation * Vector3.forward) * _followDistance;
        }

    }

	private void Update() {

        if (_trackTarget != null) {

            _targetPos = _trackTarget.transform.position - (transform.rotation * Vector3.forward) * _followDistance;
            // TODO - chande pow with dist
            transform.position = Vector3.LerpUnclamped(transform.position, _targetPos, _clampPower * Time.deltaTime);

        }

	}

}
