
using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class GameFrames : SceneManager {

	public event Action<GameFrames, IFrame> OnFrameOpen;
	public event Action<GameFrames, IFrame> OnFrameClose;
	public event Action<GameFrames, IFrame> OnFrameStateChanged;

	public const float MainAnimationTiming = 0.2f;

	[Header("If we need initial frame who must open after managers initialyze")]
	[SerializeField] private FrameBase _startFrame;
	[Header("Frames who's already be added on Ui before game is builded")]
	[SerializeField] private FrameBase[] _inSceneFrames;
	[Header("Not required (Catched from Canvas link)")]
	[SerializeField] private Transform _canvas;

	private INativeResourceLoader _resourcesLoader;
	private List<IFrame> _loadedFrames = new List<IFrame>();
	private Association[] _associations;

	public override IEnumerator OnInitialize() {
		for (int i = 0; i < _inSceneFrames.Length; i++) {
			if (_loadedFrames.AddWithoutDoubles(_inSceneFrames[i])) {
				_inSceneFrames[i].OnStationChanged += OnFrameStationChanged;
			}
		}
		CheckAllNullables();
		yield return null;
	}

	public override IEnumerator OnStart() {
		_resourcesLoader = Managers.GetManager<ResourcesLoader>().NativeLoader;
		_associations = JsonHelper.FromJson<Association>(_resourcesLoader.Load<TextAsset>("UiFrames", "Associtations").text);
		yield return null;
	}

	public override IEnumerator Final() {
		if (_startFrame != null) {
			_startFrame.Open();
		}
		return base.Final();
	}

	public bool HasOpenedFrames() {
		return _loadedFrames.ContaitsWhere(x => x.IsOpen);
	}

	public bool IsOpen<T>() where T : class, IFrame {
		return FrameIsLoaded(out T frame) && frame.IsOpen;
	}

	public T Open<T>(bool disableOther = false) where T : class, IFrame {
		if (disableOther) {
			CloseAll();
		}
		var frame = Get<T>();
		frame?.Open();
		return frame;
	}

	public void Close<T>() where T : class, IFrame {
		if (FrameIsLoaded<T>(out var frame)) {
			frame.Close();
		}
	}

	public void CloseAll() {
		_loadedFrames?.ForEach(x => x?.Hide());
	}

	public T Get<T>() where T : class, IFrame {
		if (FrameIsLoaded<T>(out var loadedFrame)) {
			return loadedFrame;
		}
		if (LoadFrame<T>(out var newFrame)) {
			return newFrame;
		}
		return null;
	}

	private void OnFrameStationChanged(IFrame frame) {
		if (frame.IsOpen) {
			OnFrameOpen?.Invoke(this, frame);
		} else {
			OnFrameClose?.Invoke(this, frame);
		}
		OnFrameStateChanged?.Invoke(this, frame);
	}

	private bool FrameIsLoaded<T>(out T loadedFrame) where T : class, IFrame {
		var frame = _loadedFrames.Find(x => x is T);
		loadedFrame = frame as T;
		return frame != null;
	}

	private bool TryFindAssociation(Type type, out string assetName) {
		string fullName = type.FullName;
		string asset = _associations.First(x => x.Identifier == fullName).AssetName;
		assetName = asset;
		return !string.IsNullOrEmpty(asset);
	}

	private bool LoadFrame<T>(out T frame) where T : class, IFrame {
		if (TryFindAssociation(typeof(T), out var assetName)) {
			var loadedFrame = _resourcesLoader.Load<UnityEngine.Object>("UiFrames", assetName);
			if (loadedFrame != null) {
				var newFrame = Instantiate(loadedFrame as GameObject, _canvas, false).GetComponent<IFrame>();
				_loadedFrames.Add(newFrame);
				frame = newFrame as T;
				newFrame.OnStationChanged += OnFrameStationChanged;
				return true;
			}
		}
		frame = null;
		return false;
	}

	private void CheckAllNullables() {
		if (_canvas == null) {
			_canvas = CanvasLink.GetCanvas("main").transform;
		}
		_loadedFrames = (from f in _loadedFrames where f != null select f).ToList();
	}
	
}