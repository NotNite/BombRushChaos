using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using BombRushChaos.Events;
using HarmonyLib;
using Reptile;
using UnityEngine;
using Random = System.Random;

namespace BombRushChaos;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInProcess("Bomb Rush Cyberfunk.exe")]
public class Plugin : BaseUnityPlugin {
    public static ManualLogSource Log = null!;
    public static List<Type> Events = new();
    public static Random Random = new();
    public static Harmony Harmony = new("BombRushChaos.Harmony");

    public static IEvent? RunningEvent = null;
    public static float EventTime = 0f;

    public static Dictionary<Type, int> Weights = new() {
        {typeof(GoFastEvent), WeightConstants.PlayerModification},
        {typeof(HoldComboEvent), WeightConstants.PlayerModification},
        {typeof(InstantHeatEvent), WeightConstants.UnforgivingAndRare},
        {typeof(NoBoostEvent), WeightConstants.PlayerModification},
        {typeof(PeaceAndQuietEvent), WeightConstants.Harmless},
        {typeof(PoloEvent), WeightConstants.Risky}
    };

    private void Awake() {
        Log = this.Logger;
        Events = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(x => typeof(IEvent).IsAssignableFrom(x) && !x.IsAbstract)
            .ToList();
        Harmony.PatchAll();
        Core.OnUpdate += this.CoreUpdate;
    }

    private void OnDestroy() {
        Core.OnUpdate -= this.CoreUpdate;
        RunningEvent?.Dispose();
        Harmony.UnpatchAll();
    }

    private void CoreUpdate() {
        var core = Core.instance;
        if (core == null || core.IsCorePaused) return;

        var baseModule = BaseModule.instance;
        if (baseModule == null
            || !baseModule.IsPlayingInStage
            || baseModule.IsInGamePaused) return;

        if (RunningEvent == null) this.PickNewEvent();
        EventTime += Time.deltaTime;
        RunningEvent?.Update();
        if (EventTime >= RunningEvent!.Duration) this.PickNewEvent();
    }

    private void PickNewEvent() {
        RunningEvent?.Dispose();

        var eventsWithoutCurrent = Events.Where(x => x != RunningEvent?.GetType()).ToList();
        var weightedEvents = new List<Type>();
        foreach (var @event in eventsWithoutCurrent) {
            var weight = Weights.TryGetValue(@event, out var w) ? w : 1;
            for (var i = 0; i < weight; i++) weightedEvents.Add(@event);
        }

        var type = weightedEvents[Random.Next(weightedEvents.Count)];
        RunningEvent = (IEvent) Activator.CreateInstance(type)!;
        EventTime = 0f;

        var chance = weightedEvents.Count(x => x == type) / (float) weightedEvents.Count;
        Log.LogDebug($"Picking new event: {RunningEvent.Name} for {RunningEvent.Duration}s ({type.Name}, {chance:P})");
    }

    private record WeightConstants {
        public const int Harmless = 5;
        public const int PlayerModification = 5;
        public const int Risky = 3;
        public const int UnforgivingAndRare = 1;
    }
}
