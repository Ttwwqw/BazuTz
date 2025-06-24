
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ScriptableCollections", menuName = "Scriptable Objects/ScriptableCollections")]
public class ScriptableCollections : ScriptableObject {

    [SerializeField] private List<ScriptableCollection> _collections = new List<ScriptableCollection>();

    public T GetCollection<T>() where T : ScriptableCollection {
        return _collections.Find(x => x is T) as T;
    }

    public bool TryGetCollection<T>(out T collection) where T : ScriptableCollection {
        collection = _collections.Find(x => x is T) as T;
        return collection != null;
    }

#if UNITY_EDITOR
    [ContextMenu("Catch all Collections in project")]
    private void CatchAllDataItems() {
        _collections = Extensions.CatchAllScriptables<ScriptableCollection>();
        UnityEditor.EditorUtility.SetDirty(this);
        UnityEditor.AssetDatabase.SaveAssetIfDirty(this);
    }
#endif

}
