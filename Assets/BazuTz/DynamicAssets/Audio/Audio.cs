
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// TODO - move to global with entry and mixing
public class Audio : SceneManager {

	public static Audio Instance { get; private set; }

	public AudioClip CurrentSound { get { if (_soundSource.clip != null) return _soundSource.clip; else return null; } }
	public AudioClip CurrentMusic { get { if (_musicSource.clip != null) return _musicSource.clip; else return null; } }

	public override IEnumerator OnInitialize() {
		Instance = this;
		yield return null;
	}
	public override IEnumerator OnStart() {
		_soundSource.volume = SoundVolume;
		_musicSource.volume = MusicVolume;
		_musicSource.Play();
		yield return null;
	}

	[SerializeField] private AudioSource _soundSource;
	[SerializeField] private AudioSource _musicSource;
	[SerializeField] private List<AudioClip> _soundCollection;
	[SerializeField] private List<AudioClip> _musicCollection;

	public float SoundVolume {
		get => float.Parse(LocalSaveSystem.TryGetLocalValue(SoundSettings.SoundKey, "1"));
		set {
			value = Mathf.Clamp(value, 0, 1);
			_soundSource.volume = value;
			LocalSaveSystem.SetLocalValue(SoundSettings.SoundKey, value.ToString());
		}
	}

	public float MusicVolume {
		get => float.Parse(LocalSaveSystem.TryGetLocalValue(SoundSettings.MusicKey, "1"));
		set {
			value = Mathf.Clamp(value, 0, 1);
			_musicSource.volume = value;
			LocalSaveSystem.SetLocalValue(SoundSettings.MusicKey, value.ToString());
		}
	}

	public void PlaySound(string soundName, float volumeScale = 1f) {
		if (_soundCollection.ContaitsWhere((x) => x.name == soundName, out var sound)) {
			_soundSource.PlayOneShot(sound, volumeScale);
		}
	}

	public void PlaySound(AudioClip clip, float volumeScale = 1f) {
		_soundSource.PlayOneShot(clip, volumeScale);
	}

	public void PlayMusic(string musicName, float volumeScale = 1f) {
		if (_musicCollection.ContaitsWhere((x) => x.name == musicName, out var music)) {
			_musicSource.Stop();
			_musicSource.volume = float.Parse(LocalSaveSystem.TryGetLocalValue(SoundSettings.SoundKey, "1")) * volumeScale;
			_musicSource.clip = music;
			_musicSource.loop = true;
			_musicSource.Play();

		}
	}

	public void PlayMusic(AudioClip clip, float volumeScale = 1f) {
		_musicSource.Stop();
		_musicSource.volume = float.Parse(LocalSaveSystem.TryGetLocalValue(SoundSettings.SoundKey, "1")) * volumeScale;
		_musicSource.clip = clip;
		_musicSource.loop = true;
		_musicSource.Play();
	}

	

}
