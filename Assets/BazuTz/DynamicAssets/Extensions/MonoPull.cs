
using System.Collections.Generic;
using UnityEngine;
using System;

public class MonoPull<T> where T : MonoBehaviour {

	public MonoPull(T prefab, Transform spawnParent, bool useWorldSpace, int initialSize = 0) {
		_prefab = prefab; _spawnParent = spawnParent; _useWorldSase = useWorldSpace;
		_prefab.gameObject.SetActive(false);
		for (int i = 0; i < initialSize; i++) {
			_pull.Add(UnityEngine.Object.Instantiate(_prefab, _spawnParent, _useWorldSase));
		}
	}

	private T _prefab;
	private bool _useWorldSase;
	private Transform _spawnParent;

	private List<T> _pull = new List<T>();
	private List<T> _active = new List<T>();

	public List<T> ActiveElements => _active;

	public void Clear(bool destroyAll = false) {
		if (destroyAll) {
			_pull.ForEach(x => UnityEngine.Object.Destroy(x.gameObject));
			_active.ForEach(x => UnityEngine.Object.Destroy(x.gameObject));
			_pull.Clear(); _active.Clear();
		} else {
			_active.ForEach(x => x?.gameObject.SetActive(false));
			_active.MoveTo(_pull);
		}
	}
	public T GetItem(Transform newParent = null) {
		T obj = null;
		_pull.RemoveAll(x => x == null);
		if (_pull.Count > 0) {
			obj = _pull.RemoveLast();
		} else {
			obj = UnityEngine.Object.Instantiate(_prefab, _spawnParent, _useWorldSase);
		}
		_active.Add(obj);
		if (newParent != null) {
			obj.transform.SetParent(newParent);
		}
		obj.gameObject.SetActive(true);
		obj.transform.SetAsLastSibling();
		return obj;
	}
	public T[] GetItems(int count, Transform newParent = null) {
		T[] items = new T[count];
		for (int i = 0; i < count; i++) {
			items[i] = GetItem(newParent);
		}
		return items;
	}
	public void ReturnItem(T item) {
		_active.Remove(item);
		_pull.AddWithoutDoubles(item);
		item?.gameObject.SetActive(false);
	}
	public void ReturnItem(params T[] items) {
		foreach (var item in items) {
			_active.Remove(item);
			_pull.AddWithoutDoubles(item);
			item?.gameObject.SetActive(false);
		}
	}
	public void ReturnItem(List<T> items) {
		foreach (var item in items) {
			_active.Remove(item);
			_pull.AddWithoutDoubles(item);
			item?.gameObject.SetActive(false);
		}
	}
	public void RemoveItem(T item, bool destroy = false) {
		_active.Remove(item);
		_pull.Remove(item);
		if (destroy & item != null) {
			UnityEngine.Object.Destroy(item.gameObject);
		}
	}
	public void AddItem(T item, bool moveToPull = false) {
		if (item != null) {
			item.transform.SetParent(_spawnParent);
			if (moveToPull) {
				_active.Remove(item);
				_pull.AddWithoutDoubles(item);
				item?.gameObject.SetActive(false);
			} else {
				_active.AddWithoutDoubles(item);
				_pull.Remove(item);
				item.gameObject.SetActive(true);
			}
		}
	}
}

public class Pull<T> where T : class {

	public Pull(Func<T> creatingInstruction) {
		_createInstruction = creatingInstruction;
	}

	private Func<T> _createInstruction;
	private List<T> _pull = new List<T>();
	private List<T> _active = new List<T>();

	public void Clear(bool destroyAll = false) {
		if (destroyAll) {
			_pull.Clear();
			_active.Clear();
		} else {
			_active.MoveTo(_pull);
			_active.Clear();
		}
	}
	public T GetItem() {
		T obj = null;
		_pull.RemoveAll(x => x == null);
		if (_pull.Count > 0) {
			obj = _pull.RemoveLast();
		} else {
			obj = _createInstruction.Invoke();
		}
		_active.Add(obj);
		return obj;
	}
	public void ReturnItem(T item) {
		_active.Remove(item);
		_pull.AddWithoutDoubles(item);
	}
	public void ReturnItem(params T[] items) {
		foreach (var item in items) {
			_active.Remove(item);
			if (item != null) {
				_pull.AddWithoutDoubles(item);
			}
		}
	}
	public void ReturnItem(List<T> items) {
		foreach (var item in items) {
			_active.Remove(item);
			_pull.AddWithoutDoubles(item);
		}
	}
}

