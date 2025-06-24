
using TMPro;
using UnityEngine;

public class HealthDisplay : MonoBehaviour {

	[SerializeField] private HealthComponent _healthComponent;
	[SerializeField] private TMP_Text _healthLabel;

	private void Awake() {
		_healthComponent.OnHealthValueChanged += OnHpChanged;
		_healthComponent.OnHealthMaxValueChanged += OnHpChanged;
		RefreshLabel();
	}
	private void OnDestroy() {
		_healthComponent.OnHealthValueChanged -= OnHpChanged;
		_healthComponent.OnHealthMaxValueChanged -= OnHpChanged;
	}

	private void OnHpChanged(float a, float b) {
		RefreshLabel();
	}

	private void RefreshLabel() {

		_healthLabel.text = string.Format("{0}/{1}", (int)_healthComponent.CurrentHp, (int)_healthComponent.MaxHp);

	}

}
