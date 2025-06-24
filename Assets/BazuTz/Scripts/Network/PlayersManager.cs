

using UnityEngine;
using Unity.Netcode;
using System.Collections;

public class PlayersManager : NetworkBehaviour, IManager {

	public IEnumerator OnInitialize() {
		yield return null;
	}
	public IEnumerator OnStart() {
		yield return null;
	}
	public IEnumerator Final() {
		yield return null;
	}


}
