
using System;
using UnityEngine;
using System.Collections.Generic;

public interface IManualUpdating {
	bool UpdateIsEnabled { get; set; }
}

public interface IUpdate : IManualUpdating {
	void Update(float timeDelta);
}

public interface ILateUpdate : IManualUpdating {
	void LateUpdate(float timeDelta);
}

public interface IFixedUpdate : IManualUpdating {
	void FixedUpdate(float timeDelta);
}

public class UpdatePoint : SceneManager, IDisposable {

	private static List<IUpdate> _updatable = new List<IUpdate>();
	private static List<ILateUpdate> _lateUpdatable = new List<ILateUpdate>();
	private static List<IFixedUpdate> _fixedUpdatable = new List<IFixedUpdate>();

	public void Subscrube<T>(T manager) where T : class, IManager {

		if (manager is IUpdate upd) {
			_updatable.AddWithoutDoubles(upd);
		}
		if (manager is ILateUpdate lateUpd) {
			_lateUpdatable.AddWithoutDoubles(lateUpd);
		}
		if (manager is IFixedUpdate fixUpd) {
			_fixedUpdatable.AddWithoutDoubles(fixUpd);
		}

	}

	public void Unsubscribe<T>(T manager) where T : class, IManager {

		if (manager is IUpdate upd) {
			_updatable.Remove(upd);
		}
		if (manager is ILateUpdate lateUpd) {
			_lateUpdatable.Remove(lateUpd);
		}
		if (manager is IFixedUpdate fixUpd) {
			_fixedUpdatable.Remove(fixUpd);
		}

	}

	public void Dispose() {
		_updatable.Clear();
		_lateUpdatable.Clear();
		_fixedUpdatable.Clear();
	}

	#region Updating

	private void Update() {
		if (_updatable.Count > 0) {
			_updatable.ForEach(x => { if (x != null && x.UpdateIsEnabled) x.Update(Time.unscaledDeltaTime); });
		}
	}
	private void LateUpdate() {
		if (_lateUpdatable.Count > 0) {
			_lateUpdatable.ForEach(x => { if (x != null && x.UpdateIsEnabled) x.LateUpdate(Time.unscaledDeltaTime); });
		}
	}
	private void FixedUpdate() {
		if (_fixedUpdatable.Count > 0) {
			_fixedUpdatable.ForEach(x => { if (x != null && x.UpdateIsEnabled) x.FixedUpdate(Time.fixedUnscaledDeltaTime); });
		}
	}

	#endregion

}
