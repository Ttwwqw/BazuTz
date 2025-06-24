using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FingerLine : MonoBehaviour {

    [SerializeField] private RectTransform _line;
    [SerializeField] private RectTransform _traget;
	[SerializeField] private float _sizeOffset;

	private void LateUpdate() {
		AlingLine();
	}

	private void AlingLine() {

		var dir = (_traget.anchoredPosition - _line.anchoredPosition).normalized;
		var dist = Vector2.Distance(_traget.anchoredPosition, _line.anchoredPosition);
		//var angle = Vector2.Angle(Vector2.up, _traget.anchoredPosition);

		float radvalue = Mathf.Atan2(dir.y, dir.x);
		var angle = radvalue * (180 / Mathf.PI);


		_line.localRotation = Quaternion.Euler(0, 0, angle - 90f);
		_line.sizeDelta = new Vector2(3f, dist - _sizeOffset);



	}


}
