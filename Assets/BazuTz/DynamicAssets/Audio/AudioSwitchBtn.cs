
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(500)]
public class AudioSwitchBtn : MonoBehaviour {

	[SerializeField] private GameObject _disabledToggle;

	private void Awake() {
		SetupButton();
	}

	public void SwitchMusicEnabled() {

		var manager = Managers.GetManager<Audio>();
		if (manager.MusicVolume > 0f) {
			manager.MusicVolume = 0f;
			manager.SoundVolume = 0f;
		} else {
			manager.MusicVolume = 1f;
			manager.SoundVolume = 1f;
		}

		SetupButton();
	}

	private void SetupButton() {

		Managers.SubscribeOnManagerCreated<Audio>((a) => {
			bool isSoundEnabled = (a as Audio).MusicVolume > 0f;
			_disabledToggle.SetActive(!isSoundEnabled);
		});

		
	}

}
