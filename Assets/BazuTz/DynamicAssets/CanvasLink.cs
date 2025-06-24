
using UnityEngine;
using System.Collections.Generic;

[DefaultExecutionOrder(0), RequireComponent(typeof(Canvas))]
public class CanvasLink : MonoBehaviour {

    [field: SerializeField] public string Tag { get; private set; } = "untaged";

    private static Dictionary<string, List<Canvas>> _canvases = new Dictionary<string, List<Canvas>>();

    private void Awake() {
        if (_canvases.ContainsKey(Tag)) {
            _canvases[Tag].Add(GetComponent<Canvas>());
        }
        _canvases.Add(Tag, new List<Canvas>() { GetComponent<Canvas>() });
    }

	private void OnDestroy() {
        _canvases[Tag].Remove(GetComponent<Canvas>());
        if (_canvases[Tag].Count <= 0) {
            _canvases.Remove(Tag);
        }
    }

	public static Canvas GetCanvas(string tag) {
        if (_canvases.ContainsKey(tag)) {
            return _canvases[tag][0];
        }
        return null;
    }

    public static List<Canvas> GetCanvases(string tag) {
        if (_canvases.ContainsKey(tag)) {
            return _canvases[tag];
        }
        return null;
    }

}
