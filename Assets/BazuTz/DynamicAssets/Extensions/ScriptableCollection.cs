
using UnityEngine;
using System.Collections.Generic;

public abstract class ScriptableCollection : ScriptableObject { }

public abstract class ScriptableItem : ScriptableObject {
    [field: SerializeField] public string Id { get; protected set; }
}

public abstract class ScriptableCollection<T> : ScriptableCollection where T : ScriptableItem {

    [SerializeField] protected List<T> _items = new List<T>();

    public T GetItem(string id) {
        return _items.Find(x => x.Id == id);
    }

    public List<T> GetAllitems() {
        return new List<T>(_items);
    }

#if UNITY_EDITOR
    protected void CatchAllItemsAndSaveAsset() {
        _items = Extensions.CatchAllScriptables<T>();
        UnityEditor.EditorUtility.SetDirty(this);
        UnityEditor.AssetDatabase.SaveAssetIfDirty(this);
    }
#endif

}
