using Reptile;
using UnityEngine;

namespace BombRushChaos.Events;

public class HoldComboEvent : IEvent {
    public string Name => "Don't drop your combo!";
    public float Duration => 30f;

    private float lastBaseScore;
    private float lastMultiplier;
    private bool didTheThing;

    public HoldComboEvent() {
        var player = WorldHandler.instance.GetCurrentPlayer();
        this.lastBaseScore = player.baseScore;
        this.lastMultiplier = player.scoreMultiplier;
    }

    public void Update() {
        if (this.didTheThing) return;

        var player = WorldHandler.instance.GetCurrentPlayer();
        var old = this.lastBaseScore * this.lastMultiplier;
        var @new = player.baseScore * player.scoreMultiplier;
        if (@new < old) {
            player.ChangeHP((int) Mathf.Ceil(player.HP));
            this.didTheThing = true;
        }
    }

    public void Dispose() { }
}
