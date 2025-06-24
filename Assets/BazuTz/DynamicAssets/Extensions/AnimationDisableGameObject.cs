
using UnityEngine;
using System.Collections.Generic;

public class AnimationDisableGameObject : MonoBehaviour {

	[SerializeField] private List<GameObject> _targetsToDisable;
	[SerializeField] private List<GameObject> _targetsToEnable;

	public void AnimationCallback() {
		_targetsToDisable.ForEach(x => x.SetActive(false));
		_targetsToEnable.ForEach(x => x.SetActive(true));
	}

}
