
using System;

public interface INativeResourceLoader {
	T Load<T>(string folder, string assetName) where T : UnityEngine.Object;
}

public interface IResourceLoader {
	void Load<T>(string bundleName, string assetName, Action<T> callback) where T : UnityEngine.Object;
}

public interface ICollectionsLoader {
	T Load<T>() where T : ScriptableCollection;
}
