
using UnityEngine;

public class UiFrame : MonoBehaviour {

	public string FrameId;

	public virtual void Open() {
		gameObject.SetActive(true);
		transform.SetAsLastSibling();
	}
	public virtual void Close() {
		if (gameObject.activeSelf) {
			gameObject.SetActive(false);
		}
	}

}