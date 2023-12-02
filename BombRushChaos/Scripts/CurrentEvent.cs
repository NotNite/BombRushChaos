using TMPro;
using UnityEngine;

namespace BombRushChaos.Scripts;

public class CurrentEvent : MonoBehaviour {
    private TextMeshProUGUI tmp;

    private void Awake() {
        this.tmp = this.GetComponent<TextMeshProUGUI>();
    }

    private void Update() {
        // Always push ourselves to the bottom left of the screen
        var size = this.tmp.GetPreferredValues();
        this.gameObject.transform.localPosition = new Vector3(
            -(Screen.width / 2) + (size.x / 2),
            -(Screen.height / 2) + (size.y / 2),
            0f
        );

        this.tmp.SetText(Plugin.RunningEvent?.Name ?? string.Empty);
    }
}
