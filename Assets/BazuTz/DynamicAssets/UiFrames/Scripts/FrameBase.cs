
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class FrameBase : MonoBehaviour, IFrame {

	[SerializeField] protected AnimatorWithCallback _animator;
	[SerializeField] protected GameObject _startNavigationTarget;

	public event Action<IFrame> OnStationChanged;
	[field: SerializeField] public bool IsOpen { get; protected set; } = false;

	public virtual void Close() {
		if (gameObject.activeSelf) {
			IsOpen = false;
			if (_animator != null) {
				_animator.SetTrigger("Hide", () => gameObject.SetActive(false));
			} else {
				gameObject.SetActive(false);
			}
			OnStationChanged?.Invoke(this);
		}
	}

	public virtual void Hide() {
		if (gameObject.activeSelf) {
			IsOpen = false;
			if (_animator != null) {
				_animator.SetTrigger("Hide", () => gameObject.SetActive(false));
			} else {
				gameObject.SetActive(false);
			}
			OnStationChanged?.Invoke(this);
		}
	}

	public virtual void Open() {

		_animator?.ClearCallbacks();

		if (gameObject.activeSelf) {
			gameObject.SetActive(false);
		}

		if (!gameObject.activeSelf) {

			IsOpen = true;
			transform.SetAsLastSibling();
			gameObject.SetActive(true);

			if (_startNavigationTarget != null) {
				EventSystem.current.SetSelectedGameObject(_startNavigationTarget);
			}

			OnStationChanged?.Invoke(this);
			
		}
	}
	
}
