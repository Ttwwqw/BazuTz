
using UnityEngine;
using System.Collections;

// [TODO] - change to scene 3side management
[DefaultExecutionOrder(298)]
public class GameInPoint : MonoBehaviour {

	[field: SerializeField] public bool AutoLoadNextScene { get; set; } = false;
	[field: SerializeField] public int SceneToAutoLoading { get; set; } = 1;

	private static bool _gameInitialised = false;

	private void Awake() {
		CoroutineBehavior.StartCoroutine(InitializeRoutine());
	}

	private IEnumerator InitializeRoutine() {

		if (_gameInitialised) {
			yield break;
		}

		_gameInitialised = true;

		var resourceLoader = new ResourcesLoader();

		Managers.AddManager(new GameStation());
		Managers.AddManager(resourceLoader);
		Managers.AddManager(new InputManager(resourceLoader.NativeLoader));
		Managers.AddManager(new CrossSceneBuffer());

		//SaveBridge.LoadAllData();

		yield return CoroutineBehavior.StartCoroutine(Managers.Initialize());

		//Localization.Initialize((SystemLanguage)System.Enum.Parse(typeof(SystemLanguage), PlayerPrefs.GetString("lang", SystemLanguage.English.ToString())), Game.GetManager<ResourcesLoader>().GetNativeLoader);

		if (AutoLoadNextScene) {
			Scenes.LoadScene(SceneToAutoLoading);
		} else {
			var _activeScene = FindFirstObjectByType<SceneInPoint>();
			yield return CoroutineBehavior.StartCoroutine(_activeScene.Initialize());
		}

	}

}
