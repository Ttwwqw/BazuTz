
using UnityEngine;
using UnityEngine.EventSystems;

public class StartNavigationPoint : MonoBehaviour {

	[SerializeField] private bool _selectOnAwake;
	[SerializeField] private GameObject _startNavigationPoint;
	
	private void OnEnable() {
		if (_selectOnAwake) {
			Select();
		}
	}

	public void Select() {
		EventSystem.current.SetSelectedGameObject(_startNavigationPoint);
	}

}
