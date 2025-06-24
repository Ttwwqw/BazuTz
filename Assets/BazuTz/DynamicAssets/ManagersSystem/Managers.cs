
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public interface IManager {
	IEnumerator OnInitialize();
	IEnumerator OnStart();
	IEnumerator Final();
}

public abstract class Manager : IManager {
	public virtual IEnumerator OnInitialize() { yield return null; }
	public virtual IEnumerator OnStart() { yield return null; }
	public virtual IEnumerator Final() { yield return null; }
}

public abstract class SceneManager : MonoBehaviour, IManager {
	public virtual IEnumerator OnInitialize() { yield return null; }
	public virtual IEnumerator OnStart() { yield return null; }
	public virtual IEnumerator Final() { yield return null; }
}

[DefaultExecutionOrder(299)]
public static class Managers {

	private static bool _isInitialized = false;
	private static UpdatePoint _updatePoint = null;
	
	private static Dictionary<Type, IManager> _managers = new Dictionary<Type, IManager>();
	private static Dictionary<Type, List<Action<IManager>>> _createCallbacks = new Dictionary<Type, List<Action<IManager>>>();

	static Managers () {
		_updatePoint = new GameObject("UpdatePoint", typeof(UpdatePoint)).GetComponent<UpdatePoint>();
		UnityEngine.Object.DontDestroyOnLoad(_updatePoint.gameObject);
	}

	public static IEnumerator Initialize() {

		if (_isInitialized) {
			throw new NotImplementedException("First initialization pass allredy done! Use AddManager(m, true) to initialize single manager");
		}

		_isInitialized = true;

		foreach (var kvp in _managers) {
			yield return CoroutineBehavior.StartCoroutine(kvp.Value.OnInitialize());
		}
		foreach (var kvp in _managers) {
			yield return CoroutineBehavior.StartCoroutine(kvp.Value.OnStart());
		}
		foreach (var kvp in _managers) {
			yield return CoroutineBehavior.StartCoroutine(kvp.Value.Final());
		}

	}

	public static T GetManager<T>() where T : class, IManager {
		if (_managers.TryGetValue(typeof(T), out var value)) {
			return value as T;
		}
		return null;
	}

	public static void AddManager<T>(T manager, bool initialize = false, Action<T> whenAdded = null) where T : class, IManager {
		AddManager(manager.GetType(), manager, initialize, (m)=> whenAdded?.Invoke(m as T));
	}

	public static void AddManager(Type type, IManager manager, bool initialize = false, Action<IManager> whenAdded = null) {

		if (_managers.TryGetValue(type, out var m)) {
			DisposeManager(m);
		}

		_managers[type] = manager;

		if (initialize) {
			CoroutineBehavior.StartCoroutine(ManagerInitializer(manager), () => { InvokeAddCallbacks(); whenAdded?.Invoke(manager); });
			return;
		}

		_updatePoint.Subscrube(manager);

		InvokeAddCallbacks();

		whenAdded?.Invoke(manager);

		void InvokeAddCallbacks() {
			if (_createCallbacks.TryGetValue(type, out var value)) {
				value.ForEach(x => x?.Invoke(manager));
				_createCallbacks.Remove(type);
			}
		}

	}


	public static void RemoveManager<T>(T manager) where T : class, IManager {

		if (_managers.TryGetValue(typeof(T), out var m)) {
			DisposeManager(m);
			_managers.Remove(typeof(T));
		}

	}

	public static void RemoveManager(Type type) {

		if (_managers.TryGetValue(type, out var m)) {
			DisposeManager(m);
			_managers.Remove(type);
		}

	}

	public static void SubscribeOnManagerCreated<T>(Action<IManager> callback) where T : class, IManager {
		if (_managers.TryGetValue(typeof(T), out var manager)) {
			callback?.Invoke(manager as T);
			return;
		}
		if (_createCallbacks.TryGetValue(typeof(T), out var value)) {
			value.Add(callback);
		} else {
			_createCallbacks[typeof(T)] = new List<Action<IManager>>() { callback };
		}
	}

	private static void DisposeManager<T>(T manager) where T : class, IManager {

		_updatePoint.Unsubscribe(manager);

		if (manager is IDisposable disposable) {
			disposable.Dispose();
		}

	}

	private static IEnumerator ManagerInitializer<T>(T manager) where T : class, IManager {

		yield return CoroutineBehavior.StartCoroutine(manager.OnInitialize());
		yield return CoroutineBehavior.StartCoroutine(manager.OnStart());
		yield return CoroutineBehavior.StartCoroutine(manager.Final());

		_updatePoint.Subscrube(manager);

		if (_createCallbacks.TryGetValue(typeof(T), out var value)) {
			value.ForEach(x => x?.Invoke(manager));
			_createCallbacks.Remove(typeof(T));
		}

	}

}
