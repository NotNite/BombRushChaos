using System;

namespace BombRushChaos;

public interface IEvent : IDisposable {
    public string Name { get; }
    public float Duration { get; }
    
    public void Update();
}
