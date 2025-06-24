
using System;
using System.Collections.Generic;

public class CrossSceneBuffer : Manager, IDisposable {

    private List<(string tag,object value)> _buffer = new List<(string,object)>();

    public void AddValue<T>(string tag, T value) {
        if (!_buffer.ContaitsWhere(x => x.tag == tag)) {
            _buffer.Add((tag,value));
        }
    }

    public void RemoveValue(string tag) {
        _buffer.RemoveWhere(x => x.tag == tag);
    }

    public bool TryGetBufferedValue<T>(string tag, out T result, bool removeValueFromBuffer = false) {

        if (_buffer.ContaitsWhere(x => x.tag == tag, out var obj) && obj is T typedT) {
            
            result = typedT;

            if (removeValueFromBuffer) {
                _buffer.Remove(obj);
            }

            return true;

        }

        result = default;
        return false;

    }

    public bool TryGetBufferedValue<T>(Predicate<T> filter, out T result, bool removeValueFromBuffer = false) {

        if (_buffer.ContaitsWhere((x) => x.value is T xt && filter.Invoke(xt), out var value)) {

            result = (T)value.value;

            if (removeValueFromBuffer) {
                _buffer.Remove(value);
            }

            return true;

        }

        result = default;
        return false;

    }

    public void Dispose() {
        _buffer.Clear();
    }

}
