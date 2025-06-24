
using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorWithCallback : MonoBehaviour {

	[field: SerializeField] public Animator Animator { get; protected set; }

	protected Action _callabck;

	public virtual void ResetTrigger(string trigger) {
		if (Animator.isActiveAndEnabled) {
			Animator.ResetTrigger(trigger);
		}
	}

	public virtual void Play(string state, Action callback = null) {
		_callabck = callback;
		Animator.Play(state);
	}

	public virtual void SetTrigger(string trigger, Action callback = null) {
		_callabck = callback;
		Animator.SetTrigger(trigger);
	}

	public virtual void SetTrigger(string trigger, out float animationTime, Action callback = null) {
		_callabck = callback;
		Animator.SetTrigger(trigger);
		animationTime = Animator.GetCurrentAnimatorStateInfo(0).length;
	}

	public virtual void OnCallCallback() {
		_callabck?.Invoke();
		_callabck = null;
	}

	public virtual void ClearCallbacks() {
		_callabck = null;
	}

}
