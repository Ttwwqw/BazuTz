
using TMPro;
using System;
using UnityEngine;

public class NotAwaible : FrameBase {

	[SerializeField] private TextMeshProUGUI _label;
	[SerializeField] private bool _canCloseSelf = true;
	private Action _onClose;

	public override void Close() {
		if (!_canCloseSelf) {
			return;
		}
		base.Close();
		_onClose?.Invoke();
	}

	public override void Open() {
		Setup(_canCloseSelf, Localization.TryTranslate(72, "Oops! \n This function has be available in next updates."));
		base.Open();
	}

	public void DirectClose() {
		base.Close();
		_onClose?.Invoke();
	}

	public void Setup(bool canCloseSelf = true, params string[] labelLines) {
		_canCloseSelf = canCloseSelf;
		_label.text = String.Join("\n", labelLines);
		_onClose = null;
	}
	public void Setup(Action onClose, params string[] labelLines) {
		_canCloseSelf = true;
		_label.text = String.Join("\n", labelLines);
		_onClose = onClose;
	}

}
