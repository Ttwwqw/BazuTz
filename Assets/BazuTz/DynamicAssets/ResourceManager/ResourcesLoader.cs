
using System;
using System.IO;
using UnityEngine;

public class ResourcesLoader : Manager {

	public ResourcesLoader() {
		BundleLoader = new DefaultBundleDataLoader();
		NativeLoader = new DefaultResourcesLoader();
		CollectionsLoader = new DefaultCollectionsLoader(NativeLoader);
	}

	public IResourceLoader BundleLoader { get; private set; }
	public INativeResourceLoader NativeLoader { get; private set; }
	public ICollectionsLoader CollectionsLoader { get; private set; }

	private class DefaultResourcesLoader : INativeResourceLoader {
		public T Load<T>(string folder, string assetName) where T : UnityEngine.Object {
			return Resources.Load<T>(string.Format("{0}/{1}", folder, assetName));
		}
	}

	private class DefaultCollectionsLoader : ICollectionsLoader {
		public DefaultCollectionsLoader(INativeResourceLoader loader) {
			_collections = loader.Load<ScriptableCollections>("Collections", "ScriptableCollections");
		}

		private ScriptableCollections _collections;

		public T Load<T>() where T : ScriptableCollection {
			return _collections.GetCollection<T>();
		}
	}

	private class DefaultBundleDataLoader : IResourceLoader {
		public void Load<T>(string bundleName, string assetName, Action<T> callback) where T : UnityEngine.Object {
			var bundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, bundleName));
			if (bundle == null) {
				Debug.Log("Failed to load AssetBundle!");
				return;
			}
			var asset = bundle.LoadAsset<T>(assetName);
			callback?.Invoke(asset);
			bundle.Unload(false);
		}
	}
	
}
