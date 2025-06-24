
using UnityEngine;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour {

	[SerializeField] private Slider _sound;
	[SerializeField] private Slider _music;

	public const string SoundKey = "sound";
	public const string MusicKey = "music";

	private Audio _audioManager;

	private void Start() {

		_audioManager = Managers.GetManager<Audio>();

		if (_sound != null) {
			_sound.onValueChanged.RemoveAllListeners();
			_sound.onValueChanged.AddListener(OnSoundVolumeHasChanged);
			_sound.value = float.Parse(LocalSaveSystem.TryGetLocalValue(SoundKey, "1"));
		}
		if (_music != null) {
			_music.onValueChanged.RemoveAllListeners();
			_music.onValueChanged.AddListener(OnMusicVolumeHasChanged);
			_music.value = float.Parse(LocalSaveSystem.TryGetLocalValue(MusicKey, "1"));
		}

	}

	private void OnSoundVolumeHasChanged(float value) {
		LocalSaveSystem.SetLocalValue(SoundKey, value.ToString());
		_audioManager.SoundVolume = value;
	}

	private void OnMusicVolumeHasChanged(float value) {
		LocalSaveSystem.SetLocalValue(MusicKey, value.ToString());
		_audioManager.MusicVolume = value;
	}

}