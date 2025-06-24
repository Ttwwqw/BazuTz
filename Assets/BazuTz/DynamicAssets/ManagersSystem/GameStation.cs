
public enum Station {
    Play, Pause, Stop, Loading, Initializing
}

public class GameStation : Manager {

    public GameStation() { }

    public GameStation(Station station) {
        _currentStation = station;
    }

    public event System.Action<Station> OnStationChanged;

    private Station _currentStation = Station.Initializing;

    public Station Station {
        get => _currentStation;
        set {
            if (value != _currentStation) {
                _currentStation = value;
                OnStationChanged?.Invoke(_currentStation);
            }
        }
    }
   
}
