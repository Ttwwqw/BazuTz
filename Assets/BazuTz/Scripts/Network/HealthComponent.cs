
using UnityEngine;
using Unity.Netcode;

public class HealthComponent : NetworkBehaviour, IDamageable, IHealth {

	private NetworkVariable<float> _currHp = new NetworkVariable<float>(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
	private NetworkVariable<float> _maxHp = new NetworkVariable<float>(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

	public event HealthValueChanged OnHealthValueChanged;
	public event HealthValueChanged OnHealthMaxValueChanged;

	public float CurrentHp {
		get => _currHp.Value;
		private set {
			var old = _currHp.Value;
			_currHp.Value = Mathf.Clamp(value, 0, MaxHp);
			OnHealthValueChanged?.Invoke(old, _currHp.Value);
		}
	}

	public float MaxHp {
		get => _maxHp.Value;
		private set {
			var old = _maxHp.Value;
			_maxHp.Value = Mathf.Clamp(value, 0, float.MaxValue);
			if (value < CurrentHp) {
				CurrentHp = value;
			}
			OnHealthMaxValueChanged?.Invoke(old, _maxHp.Value);
		}
	}

	public void DoDamage(float damage) {
		if (IsServer && damage > 0) {
			CurrentHp -= damage;
		}
	}

	public void SetHpValues(float current, float max) {
		if (IsServer) {
			CurrentHp = current; MaxHp = max;
		}
	}

	public override void OnNetworkSpawn() {
		if (!IsServer) {
			_currHp.OnValueChanged += OnServerChangedCurrHp;
			_maxHp.OnValueChanged += OnServerChangeMaxHp;
			OnHealthValueChanged?.Invoke(CurrentHp, CurrentHp);
			OnHealthMaxValueChanged?.Invoke(MaxHp, MaxHp);
		}
	}

	public override void OnNetworkDespawn() {
		if (!IsServer) {
			_currHp.OnValueChanged -= OnServerChangedCurrHp;
			_maxHp.OnValueChanged -= OnServerChangeMaxHp;
		}
	}

	private void OnServerChangedCurrHp(float previousValue, float newValue) {
		OnHealthValueChanged?.Invoke(previousValue, newValue);
	}

	private void OnServerChangeMaxHp(float previousValue, float newValue) {
		OnHealthMaxValueChanged?.Invoke(previousValue, newValue);
	}

}
