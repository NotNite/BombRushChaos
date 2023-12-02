using Reptile;

namespace BombRushChaos.Events;

public class NoBoostEvent : IEvent {
    public string Name => "Dropped the boostpack";
    public float Duration => 30f;

    public void Update() {
        var player = WorldHandler.instance.GetCurrentPlayer();
        player.boostCharge = 0f;
    }

    public void Dispose() { }
}
