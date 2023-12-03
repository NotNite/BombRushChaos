using BombRushChaos.Events;
using HarmonyLib;
using Reptile;
using UnityEngine;

namespace BombRushChaos.Patches;

[HarmonyPatch(typeof(Player))]
public class PlayerPatch {
    [HarmonyPostfix]
    [HarmonyPatch("GetDesiredVelocity")]
    public static void GetDesiredVelocity(Player __instance, ref Vector3 __result) {
        if (Plugin.RunningEvent is GoFastEvent) {
            __result *= 10f;
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch("SetInputs")]
    public static void SetInputs(ref UserInputHandler.InputBuffer inputBuffer) {
        if (Plugin.RunningEvent is SwappedInputsEvent) {
            inputBuffer.moveAxisX = -inputBuffer.moveAxisX;
            inputBuffer.moveAxisY = -inputBuffer.moveAxisY;
        }
    }
}
