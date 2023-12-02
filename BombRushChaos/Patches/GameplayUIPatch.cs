using BombRushChaos.Scripts;
using HarmonyLib;
using Reptile;
using TMPro;
using UnityEngine;

namespace BombRushChaos.Patches;

[HarmonyPatch(typeof(GameplayUI))]
public class GameplayUIPatch {
    [HarmonyPostfix]
    [HarmonyPatch("Init")]
    public static void Init(GameplayUI __instance) {
        var tricksInComboLabel = __instance.tricksInComboLabel;

        var obj = new GameObject("BombRushChaos_CurrentEvent");
        var tmp = obj.AddComponent<TextMeshProUGUI>();
        tmp.font = tricksInComboLabel.font;
        tmp.material = tricksInComboLabel.material;
        tmp.fontMaterial = tricksInComboLabel.fontMaterial;
        tmp.enableWordWrapping = false;
        tmp.alignment = TextAlignmentOptions.Center;

        obj.transform.SetParent(__instance.gameObject.transform);
        obj.AddComponent<CurrentEvent>();
    }
}
