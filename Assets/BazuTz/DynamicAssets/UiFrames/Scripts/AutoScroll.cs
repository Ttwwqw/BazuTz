
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[DefaultExecutionOrder(1000), RequireComponent(typeof(ScrollRect))]
public class AutoScroll : MonoBehaviour {

    [field:SerializeField] public bool IsEnabled { get; set; } = true;
    [SerializeField] private int _startScrollWithMinElementsCount = 5;
    [SerializeField] private float _scrolingSpeed = 10f;
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private bool _isVertical = true;

    private Vector2 _targetPosition = Vector2.zero;
    private Selectable _lastSelected = null;
    private List<Selectable> _selectables = new List<Selectable>();

    private bool _updateIsLoacked = false;

    private void OnEnable() {
        _scrollRect.normalizedPosition = _targetPosition =  Vector2.one;
        RefreshElenemts();
    }

	private void OnDisable() {
        _updateIsLoacked = true;
    }

    public void RefreshElenemts() {
        _selectables.Clear();
        _lastSelected = null;
        _targetPosition = _scrollRect.normalizedPosition;
        CoroutineBehavior.Delay(0.1f, () => { _scrollRect.content.GetComponentsInChildren(_selectables); MoveScrollToPosition(true, true); _updateIsLoacked = false; }, true);
    }

	public void UpdateImmediately() {
        if (!ScrollIsEnabled) {
            return;
        }
        _updateIsLoacked = true;
        _selectables.Clear();
        _lastSelected = null;
        _targetPosition = _scrollRect.normalizedPosition;
        CoroutineBehavior.Delay(0.1f, ()=> { _scrollRect.content.GetComponentsInChildren(_selectables); MoveScrollToPosition(true); _updateIsLoacked = false; }, true);
        
    }

    public void UpdatePosition(bool lerp) {
        if (!MoveScrollToPosition(lerp)) {
            _scrollRect.normalizedPosition = _targetPosition;
        }
    }

	private void LateUpdate() {
        if (ScrollIsEnabled && !_updateIsLoacked) {
            MoveScrollToPosition(true);
            _scrollRect.normalizedPosition = Vector2.Lerp(_scrollRect.normalizedPosition, _targetPosition, Time.unscaledDeltaTime * _scrolingSpeed);
        }
    }

	private bool MoveScrollToPosition(bool lerp, bool force = false) {

        if (!ScrollIsEnabled && !force) {
            return false;
        }

        int selectedIndex = -1;
        Selectable selectedElement = EventSystem.current.currentSelectedGameObject ? EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>() : null;

        if (selectedElement) {
            selectedIndex = _selectables.IndexOf(selectedElement);
            _lastSelected = selectedElement;
        }

        if (selectedIndex > -1) {

            if (_isVertical) {
                _targetPosition = new Vector2(0, 1 - (selectedIndex / ((float)_selectables.Count - 1)));
            } else {
                _targetPosition = new Vector2(0 + (selectedIndex / ((float)_selectables.Count - 1)), 0);
            }
            
            if (!lerp) {
                _scrollRect.normalizedPosition = _targetPosition;
            }

            return true;

        }

        return false;

    }

    private bool ScrollIsEnabled { get =>
        IsEnabled && (_startScrollWithMinElementsCount == 0 || _startScrollWithMinElementsCount < _selectables.Count);
    }

}
