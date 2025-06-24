
using UnityEngine;
using System.Collections.Generic;

[DefaultExecutionOrder(0), RequireComponent(typeof(Camera))]
public class CameraLink : MonoBehaviour {

    [field: SerializeField] public string Tag { get; private set; } = "untaged";

    private static Dictionary<string, List<Camera>> _cams = new Dictionary<string, List<Camera>>();

    private void Awake() {
        if (_cams.ContainsKey(Tag)) {
            _cams[Tag].Add(GetComponent<Camera>());
        }
        _cams.Add(Tag, new List<Camera>() { GetComponent<Camera>() });
    }

    private void OnDestroy() {
        _cams[Tag].Remove(GetComponent<Camera>());
        if (_cams[Tag].Count <= 0) {
            _cams.Remove(Tag);
        }
    }

    public static Camera GetCamera(string tag) {
        if (_cams.ContainsKey(tag)) {
            return _cams[tag][0];
        }
        return null;
    }

    public static List<Camera> GetCams(string tag) {
        if (_cams.ContainsKey(tag)) {
            return _cams[tag];
        }
        return null;
    }

}

