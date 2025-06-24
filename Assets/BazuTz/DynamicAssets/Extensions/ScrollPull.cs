
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ScrollPull<T> where T : MonoBehaviour {

    [SerializeField] private Transform _spawnParent;
    [SerializeField] private bool _useWorldSpace;
    [SerializeField] private T _prefab;

    [HideInInspector] public MonoPull<T> Pull;
    public List<T> ActiveItems => Pull.ActiveElements;

    public void Initialize() {
        if (Pull == null) {
            Pull = new MonoPull<T>(_prefab, _spawnParent, _useWorldSpace);
        } else {
            Pull.Clear();
        }
    }

    public T Create() => Pull.GetItem();
    public void Return(T item) => Pull.ReturnItem(item);
    public void Clear(bool destroyItems) => Pull.Clear(destroyItems);

}
