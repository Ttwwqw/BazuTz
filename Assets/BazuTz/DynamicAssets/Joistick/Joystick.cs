
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;


public class Joystick : SceneManager, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler {

	[Header("NotRequred-CanvasLink 'main'")]
	[SerializeField] private Canvas _canvas;
	[SerializeField] private RectTransform _joistickGraphic;
    [SerializeField] private RectTransform _center;
    [SerializeField] private RectTransform _finger;
    [SerializeField] private float _maxFingerDistance;

	public event Action HitEvent;
	public event Action<Vector2> MoveEvent;

	public Vector3 Diraction { get; set; } = Vector3.zero;

	private Vector2 _startTouchPoint;
	private bool _isActiveNow = false;

	private void Awake() {
		_canvas = _canvas == null ? CanvasLink.GetCanvas("main") : _canvas;
		_joistickGraphic.gameObject.SetActive(false);
	}

	public void OnPointerDown(PointerEventData eventData) {

		if (_isActiveNow) {
			OnPointerUp(eventData);
		} else {
			_isActiveNow = true;
			_joistickGraphic.gameObject.SetActive(true);
			_startTouchPoint = eventData.position / _canvas.scaleFactor;
			_joistickGraphic.anchoredPosition = eventData.position / _canvas.scaleFactor;
		}

		Diraction = Vector3.zero;

	}

	public void OnPointerMove(PointerEventData eventData) {

		if (_isActiveNow) {
			var currPoss = eventData.position / _canvas.scaleFactor;

			var fingerDist = Mathf.Clamp(Vector2.Distance(currPoss, _startTouchPoint), 0f, _maxFingerDistance);
			var inputDir = (currPoss - _startTouchPoint).normalized;
			_finger.anchoredPosition = -(_joistickGraphic.anchoredPosition - (_startTouchPoint + inputDir * fingerDist));

			var dir = inputDir * (fingerDist / _maxFingerDistance);
			Diraction = new Vector3(dir.x, 0f, dir.y);
			MoveEvent?.Invoke(Diraction);
		}

	}

	public void OnPointerUp(PointerEventData eventData) {
		if (_isActiveNow) {
			_isActiveNow = false;
			_joistickGraphic.gameObject.SetActive(false);
			HitEvent?.Invoke();
		}
		Diraction = Vector3.zero;
	}

	private Vector2 MousePositionToScreenAnchor() {
		return (Vector2)Input.mousePosition / _canvas.scaleFactor;
	}


}
