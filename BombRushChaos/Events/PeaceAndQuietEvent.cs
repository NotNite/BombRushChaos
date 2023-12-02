namespace BombRushChaos.Events;

public class PeaceAndQuietEvent : IEvent {
    public string Name => "Peace and Quiet";
    public float Duration => 15f;

    public void Update() { }
    public void Dispose() { }
}
