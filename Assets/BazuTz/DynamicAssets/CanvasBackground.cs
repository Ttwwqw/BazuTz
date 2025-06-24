
using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasLink))]
public class CanvasBackground : MonoBehaviour {

    [SerializeField] private AnimatorWithCallback _animator;
    [SerializeField] private Image _background;

    public void Show(Action callback = null) {
        if (_animator.Animator.isActiveAndEnabled) {
            _animator.ResetTrigger("Hide");
            _animator.SetTrigger("Show", callback);
        } else {
            callback?.Invoke();
        }
    }

    public void Hide(Action callback = null) {
        if (_animator.Animator.isActiveAndEnabled) {
            _animator.ResetTrigger("Show");
            _animator.SetTrigger("Hide", callback);
        } else {
            callback?.Invoke();
        }
    }

    public void Change(Sprite sprite) {
        _background.sprite = sprite;
    }

}
