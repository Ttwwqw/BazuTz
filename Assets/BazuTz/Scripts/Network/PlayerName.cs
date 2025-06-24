
using TMPro;
using UnityEngine;
using Unity.Netcode;

public class PlayerName : NetworkBehaviour {

	[SerializeField] private TMP_Text _label;
	[SerializeField] private GameObject _selfHint;

	private NetworkVariable<NetworkString> _playerName = new NetworkVariable<NetworkString>();

	public override void OnNetworkSpawn() {

		_playerName.OnValueChanged += OnNameValueChanged;

		if (IsServer) {
			_playerName.Value = OwnerClientId.ToString();
		}

		if (IsClient) {
			_label.text = _playerName.Value;
		}

		_selfHint.gameObject.SetActive(IsOwner);

	}

	public override void OnNetworkDespawn() {
		_playerName.OnValueChanged -= OnNameValueChanged;
	}

	private void OnNameValueChanged(NetworkString previousValue, NetworkString newValue) {

		_label.text = newValue.Value;

	}

}
