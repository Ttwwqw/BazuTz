
public interface IDamageable {
    public void DoDamage(float daname);
}

public interface IHealth {
    public float CurrentHp { get; }
    public float MaxHp { get; }
    public event HealthValueChanged OnHealthValueChanged;
    public event HealthValueChanged OnHealthMaxValueChanged;
}

public delegate void HealthValueChanged(float oldValue, float newValue);