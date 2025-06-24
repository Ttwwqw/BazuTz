
using UnityEngine;

[DefaultExecutionOrder(0), RequireComponent(typeof(SceneManager))]
public class SceneManagerActivator : MonoBehaviour {
    [SerializeField] private bool _activateWhenAdded = true;
    private void Awake() {
        var manager = GetComponent<SceneManager>();
        Managers.AddManager(manager, _activateWhenAdded);
        Debug.Log(string.Format("[InPoint] Automaticly added manager: {0}", manager.GetType().FullName));
    }

}
