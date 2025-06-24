
using TMPro;
using UnityEngine;

public class VersionLabel : MonoBehaviour {

	[SerializeField] private TextMeshProUGUI _label;

	private void Awake() {
		_label.text = string.Format("Game version: {0}", Application.version);
	}

}
