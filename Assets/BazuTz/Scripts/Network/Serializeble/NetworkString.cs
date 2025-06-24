
using Unity.Netcode;
using Unity.Collections;

public struct NetworkString : INetworkSerializable {
	
	public NetworkString(string value) {
		_value = new FixedString32Bytes(value);
	}

	private FixedString32Bytes _value;

	public string Value {
		get => ToString();
		set => _value = new FixedString32Bytes(value);
	}

	public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
		serializer.SerializeValue(ref _value);
	}

	public override string ToString() {
		return _value.ToString();
	}

	public static implicit operator string(NetworkString s) => s.ToString();
	public static implicit operator NetworkString(string s) => new NetworkString(s);

}
