
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IIachivenetListener {
    public void OnGainned(int id);
    public void OnProgressCahnged(int id, float progress);
}

public class Achievements : Manager {

    public Achievements() {
        _achievements = new List<AchievementSave>();
        _listeners = new Dictionary<int, List<IIachivenetListener>>();
    }

    private List<AchievementSave> _achievements;
    private Dictionary<int, List<IIachivenetListener>> _listeners;

	public override IEnumerator OnStart() {



		return base.OnStart();

	}

    public void AddListener(int achievementId, IIachivenetListener listener, out bool achievementIsCompleated) {
        achievementIsCompleated = IsCompeated(achievementId);
        AddListener(achievementId, listener);
    }

    public void AddListener(int achievementId, IIachivenetListener listener) {
        if (_listeners.ContainsKey(achievementId)) {
            _listeners[achievementId].Add(listener);
        } else {
            _listeners.Add(achievementId, new List<IIachivenetListener>() { listener });
        }
    }

    public void RemoveListener(int achievementId, IIachivenetListener listener) {
        if (_listeners.TryGetValue(achievementId, out var listeners) && listeners.Remove(listener) && listeners.Count <= 0) {
            _listeners.Remove(achievementId);
        }
    }

    public void AddProgress(int achievementId, float value) {

        AchievementSave save = null;
        float progress = 0f;
        if (_achievements.ContaitsWhere(x => x.id == achievementId, out save)) {
            save.progress += value;
            progress = save.progress;
        } else {
            save = new AchievementSave() { id = achievementId, isCompleated = false, progress = value };
            _achievements.Add(save);
            progress = value;
        }

        /// TODO - check compleated
        /// LOAD collection of all Achievement (create scriptable or set for it)

        if (_listeners.TryGetValue(achievementId, out var listeners)) {
            listeners.ForEach(x => x.OnProgressCahnged(achievementId, progress));
        }


    }

    public bool IsCompeated(int achievementId) {
        if (_achievements.ContaitsWhere(x => x.id == achievementId, out var a)) {
            return a.isCompleated;
        }
        return false;
    }

    public bool IsCompleated(string achievementName) {

        return false;
    }

    public float GetProgress(int achievementId) {
        if (_achievements.ContaitsWhere(x => x.id == achievementId, out var a)) {
            return a.progress;
        }
        return 0f;
    }

}

[System.Serializable]
public class AchievementSave {
    public int id;
    public bool isCompleated;
    public float progress;
}

[System.Serializable]
public class Achievement {

    [field: SerializeField] public int Id { get; private set; }
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public float MaxProgress { get; private set; }

    public bool IsCompleated(AchievementSave save) {
        return save.id == Id && save.progress >= MaxProgress;
    }

}
