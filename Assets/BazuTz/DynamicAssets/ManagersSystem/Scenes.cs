
using System;
using UnityEngine;
using System.Collections;
using Object = UnityEngine.Object;

public static class Scenes {

	private static SceneInPoint _activeScene = null;
	private static CoroutineWrapper _sceneLoadRoutine = null;

	public static UnityEngine.SceneManagement.Scene Current => UnityEngine.SceneManagement.SceneManager.GetActiveScene();

	public static void LoadScene(int index, Action onSceneLoaded = null) {
		if (_sceneLoadRoutine == null) {
			_sceneLoadRoutine = CoroutineBehavior.StartCoroutine(SceneLoaderCoroutine(index),
				() => { Debug.Log(string.Format("[Scene] Scene: {0} - Loaded", index)); onSceneLoaded?.Invoke(); _sceneLoadRoutine = null; });
		}
	}

	public static void LoadScene(string name, Action onSceneLoaded = null) {
		Debug.Log(string.Format("[Scene] Try start load scene"));
		if (_sceneLoadRoutine == null) {
			Debug.Log(string.Format("[Scene] Start loading routine"));

			_sceneLoadRoutine = CoroutineBehavior.StartCoroutine(SceneLoaderCoroutine(name),
				() => { Debug.Log(string.Format("[Scene] Scene: {0} - Loaded", name)); onSceneLoaded?.Invoke(); _sceneLoadRoutine = null; });
		}
	}

	/// TODO - loading splash, cross scene info
	private static IEnumerator SceneLoaderCoroutine(int sceneIndex) {

		LoadingSliderFill loadingCanvas = Object.Instantiate(Resources.Load("UiFrames/LoadingCanvas", typeof(LoadingSliderFill)) as LoadingSliderFill);
		Object.DontDestroyOnLoad(loadingCanvas.gameObject);
		loadingCanvas.SetValue(0f);

		GameStation gameStation = Managers.GetManager<GameStation>();
		gameStation.Station = Station.Stop;

		if (_activeScene != null) {
			yield return CoroutineBehavior.StartCoroutine(_activeScene.Dispose());
		}

		yield return new WaitForSeconds(1f);
		
		if (sceneIndex == UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex) {
			yield return CoroutineBehavior.StartCoroutine(LoadEmptyScene());
		}
		
		gameStation.Station = Station.Loading;
		var asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneIndex);
		while (!asyncLoad.isDone) {
			loadingCanvas.SetValue(asyncLoad.progress);
			yield return null;
		}
		
		gameStation.Station = Station.Initializing;
		_activeScene = Object.FindFirstObjectByType<SceneInPoint>();
		yield return CoroutineBehavior.StartCoroutine(_activeScene.Initialize());

		loadingCanvas.Hide(() => { Object.Destroy(loadingCanvas.gameObject); });

		gameStation.Station = Station.Play;

	}

	/// TODO - loading splash, cross scene info
	private static IEnumerator SceneLoaderCoroutine(string sceneIndex) {

		LoadingSliderFill loadingCanvas = Object.Instantiate(Resources.Load("UiFrames/LoadingCanvas", typeof(LoadingSliderFill)) as LoadingSliderFill);
		Object.DontDestroyOnLoad(loadingCanvas.gameObject);
		loadingCanvas.SetValue(0f);

		GameStation gameStation = Managers.GetManager<GameStation>();
		gameStation.Station = Station.Stop;

		if (_activeScene != null) {
			yield return CoroutineBehavior.StartCoroutine(_activeScene.Dispose());
		}

		yield return new WaitForSecondsRealtime(1f);

		if (sceneIndex == UnityEngine.SceneManagement.SceneManager.GetActiveScene().name) {
			yield return CoroutineBehavior.StartCoroutine(LoadEmptyScene());
		}

		gameStation.Station = Station.Loading;
		var asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneIndex);
		while (!asyncLoad.isDone) {
			loadingCanvas.SetValue(asyncLoad.progress);
			yield return null;
		}

		gameStation.Station = Station.Initializing;
		_activeScene = Object.FindFirstObjectByType<SceneInPoint>();
		yield return CoroutineBehavior.StartCoroutine(_activeScene.Initialize());

		loadingCanvas.Hide(() => { Object.Destroy(loadingCanvas.gameObject); });

		gameStation.Station = Station.Play;

	}

	private static IEnumerator LoadEmptyScene() {
		var asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("LoadingScreen");
		while (!asyncLoad.isDone) {
			yield return null;
		}
	}

}
