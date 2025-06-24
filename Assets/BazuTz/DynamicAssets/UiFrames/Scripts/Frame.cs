
using System;

public interface IFrame {
	public bool IsOpen { get; }
	public event Action<IFrame> OnStationChanged;
	public void Open();
	public void Close();
	public void Hide();
}

public interface IIdentifiedAsset {
	public string Id { get; }
}

[Serializable]
public struct Association {
	public string Identifier;
	public string AssetName;
	public Association(string id, string name) {
		Identifier = id; AssetName = name;
	}
}