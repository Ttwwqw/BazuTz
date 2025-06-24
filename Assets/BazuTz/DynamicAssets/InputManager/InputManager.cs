
using UnityEngine.InputSystem;

public class InputManager : Manager {

	public InputManager(INativeResourceLoader loader) {
		InputSheme = loader.Load<InputActionAsset>("Input", "InputSystem_Actions");
	}

	public InputActionAsset InputSheme { get; private set; }

	public InputActionMap UiMap => InputSheme.FindActionMap("UI");
	public InputActionMap PlayerMap => InputSheme.FindActionMap("Player");
	
	public void EnableActionMap(params string[] maps) {
		foreach (var m in maps) {
			InputSheme.FindActionMap(m).Enable();
		}
	}
	public void EnableAction(string map, string action) {
		InputSheme.FindActionMap(map).FindAction(action).Enable();
	}
	public void EnableActionMap(InputActionMap map) {
		map.Enable();
	}

	public void DisableActionMap(params string[] maps) {
		foreach (var m in maps) {
			InputSheme.FindActionMap(m).Disable();
		}
	}
	public void DisableAction(string map, string action) {
		InputSheme.FindActionMap(map).FindAction(action).Disable();
	}
	public void DisableActionMap(InputActionMap map) {
		map.Disable();
	}

}
