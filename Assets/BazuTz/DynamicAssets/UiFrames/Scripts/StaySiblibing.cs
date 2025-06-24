using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO - настроить нормально интерфейсы
public class StaySiblibing : MonoBehaviour {

	private int _currentChildIndex = 0;

	private void Awake() {
		_currentChildIndex = transform.GetSiblingIndex();
	}
	private void LateUpdate() {
		transform.SetSiblingIndex(_currentChildIndex);
	}







}
