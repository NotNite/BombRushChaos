using Reptile;

namespace BombRushChaos.Events;

public class InstantHeatEvent : IEvent {
    public string Name => "Is it just me or is it hot in here?";
    public float Duration => 30f;

    public InstantHeatEvent() {
        WantedManager.instance.SetDebugStarAmount(6);
    }

    public void Update() { }

    public void Dispose() {
        WantedManager.instance.SetDebugStarAmount(0);
    }
}
