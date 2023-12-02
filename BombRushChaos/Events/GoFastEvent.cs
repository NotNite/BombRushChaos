namespace BombRushChaos.Events;

public class GoFastEvent : IEvent {
    public string Name => "Go Fast";
    public float Duration => 10f;

    public void Update() { }
    public void Dispose() { }
}
