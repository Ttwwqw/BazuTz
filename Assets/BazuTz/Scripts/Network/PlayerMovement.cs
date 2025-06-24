
using System;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : NetworkBehaviour {

    [SerializeField] private CharacterController _chController;
	[SerializeField, Range(0.1f, 10f)] private float _movementSpeed = 5f;

	private CameraFollow _camera = null;
	private InputAction _inputMoveAction = null;

	public override void OnNetworkSpawn() {

		if (IsOwner) {

			_chController ??= GetComponent<CharacterController>();
			_inputMoveAction = Managers.GetManager<InputManager>().PlayerMap.FindAction("Move");

			if (CameraLink.GetCamera("main").TryGetComponent<CameraFollow>(out var c)) {
				_camera = c;
				c.SetCameraFollowTarget(_chController.transform);
			} else {
				Debug.LogWarning("Cant find [CameraFollow] component on camera - [CameraLink (main) ]");
			}

		}
		
		base.OnNetworkSpawn();

	}

	public override void OnNetworkDespawn() {

		_camera?.SetCameraFollowTarget(null);

		base.OnNetworkDespawn();

	}

	private void Update() {

		if (IsOwner) {

			if (_inputMoveAction != null) {
				var input = _inputMoveAction.ReadValue<Vector2>();
				float y = _chController.isGrounded ? 0 : -1;
				_chController.Move(_movementSpeed * Time.deltaTime * new Vector3(input.x, y, input.y));
			}
			
		}

	}

}
