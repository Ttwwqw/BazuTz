
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorEventValue : MonoBehaviour {
	
	public string Parameter;
	[SerializeField] private Animator _animator;

	public void SetFloat(float value) {
		_animator.SetFloat(Parameter, value);
	}

	public void SetInt(int value) {
		_animator.SetInteger(Parameter, value);
	}

	public void SetBool(bool value) {
		_animator.SetBool(Parameter, value);
		Debug.Log("Set");
	}

}
