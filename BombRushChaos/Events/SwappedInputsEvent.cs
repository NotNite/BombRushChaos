namespace BombRushChaos.Events;

public class SwappedInputsEvent : IEvent {
    public string Name => "Dazed and confused";
    public float Duration => 15f;
    public void Update() { }
    public void Dispose() { }
}
