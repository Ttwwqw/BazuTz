
using System;

public class SampleCloseAnimation : AnimatorWithCallback {

	public override void SetTrigger(string trigger, Action callback = null) {

		_callabck = callback == null ? DisableGameObject : callback + DisableGameObject;
		Animator.SetTrigger(trigger);

		void DisableGameObject() => gameObject.SetActive(false);

	}

}
