

using UnityEngine;
using Unity.Netcode;

public class NetHUD : MonoBehaviour {

#if UNITY_EDITOR || DEVELOPMENT_BUILD

	private void OnGUI() {

		if (!NetworkManager.Singleton) {
			return;
		}

		if (NetworkManager.Singleton.IsHost) {

			if (GUI.Button(new Rect(10, 10, 150, 25), "Shutdown")) {

				NetworkManager.Singleton.Shutdown(true);

			}

			GUI.Box(new Rect(10, 40, 150, 25), "");
			GUI.Label(new Rect(15, 42, 150, 25), string.Format("Players: {0}", NetworkManager.Singleton.ConnectedClientsList.Count));

			if (GUI.Button(new Rect(10, 75, 150, 25), "Start enemies spawn")) {
				Managers.GetManager<EnemySpawner>().StartSpawning();
			}

			if (GUI.Button(new Rect(10, 100, 150, 25), "Stop enemies spawn")) {
				Managers.GetManager<EnemySpawner>().StopSpawning();
			}

			return;

		}

		if (NetworkManager.Singleton.IsClient) {

			if (GUI.Button(new Rect(10, 10, 150, 25), "Disconnect")) {

				NetworkManager.Singleton.Shutdown(true);

			}

			return;

		}

		if (GUI.Button(new Rect(10, 10, 150, 25), "Start Client")) {

			if (NetworkManager.Singleton.StartClient()) {

				Debug.Log("Client is started");

			} else {

				Debug.LogWarning("Cannot start client");

			}

		}

		if (GUI.Button(new Rect(10, 40, 150, 25), "Start Host")) {

			if (NetworkManager.Singleton.StartHost()) {

				Debug.Log("Host is started");

			} else {

				Debug.LogWarning("Cannot start host");

			}

		}

	}

#endif

}
