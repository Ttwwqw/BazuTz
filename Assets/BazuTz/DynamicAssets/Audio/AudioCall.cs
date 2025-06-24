
using UnityEngine;

[DefaultExecutionOrder(0)]
public class AudioCall : MonoBehaviour {

	[SerializeField] private float _awakeBlockDelay = 0f;

	[SerializeField] private bool _activateOnAwake;
	[SerializeField] private bool _isMusic;

	[SerializeField] private float _volume = 1f;
	[SerializeField] private float _repeatTiming = 0f;

	[SerializeField] private string _clipName;
	[SerializeField] private AudioClip _clip;

	private bool _isReady = true;

	private void Awake() {
		
		if (_activateOnAwake) {
			CoroutineBehavior.Delay(_awakeBlockDelay + 0.35f, Play);
		}
	}
	private void OnEnable() {
		if (_awakeBlockDelay != 0f) {
			_isReady = false;
			CoroutineBehavior.Delay(_awakeBlockDelay, () => _isReady = true);
		}
	}

	public void Play() {
		if (_isMusic) {
			PlayMusic();
		} else {
			PlaySound();
		}
	}

	public void ChangeVolume(float volume) {
		_volume = volume;
	}

	public void PlaySound(string clip) {
		if (_isReady) {
			Audio.Instance.PlaySound(clip, _volume);
		}
	}

	public void PlayMusic(string clip) {
		if (_isReady) {
			Audio.Instance.PlayMusic(clip, _volume);
		}
	}

	private void PlaySound() {

		if (!_isReady) {
			return;
		}

		if (_clip != null) {
			Audio.Instance.PlaySound(_clip, _volume);
		} else {
			Audio.Instance.PlaySound(_clipName, _volume);
		}

		if (_repeatTiming > 0f) {
			_isReady = false;
			CoroutineBehavior.Delay(_repeatTiming, () => _isReady = true);
		}

	}

	private void PlayMusic() {

		if (!_isReady) {
			return;
		}

		if (_clip != null) {
			Audio.Instance.PlayMusic(_clip, _volume);
		} else {
			Audio.Instance.PlayMusic(_clipName, _volume);
		}

		if (_repeatTiming > 0f) {
			_isReady = false;
			CoroutineBehavior.Delay(_repeatTiming, () => _isReady = true);
		}

	}

}
