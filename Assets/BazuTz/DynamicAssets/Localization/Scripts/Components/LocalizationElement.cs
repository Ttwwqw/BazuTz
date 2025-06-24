
using TMPro;
using UnityEngine;

public class LocalizationElement : MonoBehaviour {

	[SerializeField] private string _key;
	[SerializeField] private TextMeshProUGUI _label;

	private void Awake() {
		Translate();
		Localization.OnLangugeIsChanged += Translate;
	}
	private void OnDestroy() {
		Localization.OnLangugeIsChanged -= Translate;
	}

	private void Translate() {
		_label.text = Localization.TryTranslate(_key, _label.text);
	}

}
