
using UnityEngine;

public class AudioChangeSettings : MonoBehaviour {

    public void ChangeSoundVolume(float value) {
        Managers.GetManager<Audio>().SoundVolume = value;
    }

    public void ChangeMusicVolume(float value) {
        Managers.GetManager<Audio>().MusicVolume = value;
    }

    public void EnableSound() {
        Managers.GetManager<Audio>().SoundVolume = 1;
    }

    public void EnableMusic() {
        Managers.GetManager<Audio>().MusicVolume = 1;
    }

    public void DisableSound() {
        Managers.GetManager<Audio>().SoundVolume = 0;
    }

    public void DisableMusic() {
        Managers.GetManager<Audio>().MusicVolume = 0;
    }

}
