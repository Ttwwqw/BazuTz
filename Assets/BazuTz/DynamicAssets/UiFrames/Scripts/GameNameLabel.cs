
using TMPro;
using UnityEngine;

public class GameNameLabel : MonoBehaviour {

	[SerializeField] private TextMeshProUGUI _label;

	private void Awake() {
		_label.text = Application.productName;
	}

}
