using System;
using BombRushChaos.Events;
using Reptile;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BombRushChaos.Scripts;

public class ChasingPolo : MonoBehaviour {
    private void Awake() {
        var player = this.GetPlayer();
        if (player == null) return;

        // Choose a random direction to place Polo in
        const float distance = 20f;
        var direction = Random.Range(0f, 360f);
        var position = player.transform.position;

        var dir = Vector3.forward * distance;
        dir = Quaternion.Euler(0f, direction, 0f) * dir;

        var pos = position + dir;
        this.transform.position = pos;

        Core.Instance.AudioManager.PlaySfxUI(
            SfxCollectionID.EnvironmentSfx,
            AudioClipID.MascotUnlock
        );

        Core.OnUpdate += this.CoreUpdate;
    }

    private void OnDestroy() {
        Core.OnUpdate -= this.CoreUpdate;
    }

    private Player? GetPlayer() {
        var worldHandler = WorldHandler.instance;
        if (worldHandler == null) return null;
        var player = worldHandler.GetCurrentPlayer();
        if (player == null) return null;
        return player;
    }

    private void CoreUpdate() {
        const float killRange = 0.5f;
        const float minSpeed = 5f;
        const float maxSpeed = 10f;

        if (Plugin.RunningEvent is not PoloEvent poloEvent) return;
        var progress = Plugin.EventTime / poloEvent.Duration;
        var speed = Mathf.Lerp(minSpeed, maxSpeed, progress);
        
        var player = this.GetPlayer();
        if (player == null) return;

        var tf = this.transform;
        var position = tf.position;

        var playerPosition = player.transform.position;
        var diff = playerPosition - position;
        var len = diff.magnitude;

        if (len < killRange) {
            player.ChangeHP((int) Mathf.Ceil(player.HP));
            Destroy(this.gameObject);
            return;
        }

        var direction = diff.normalized;
        position += direction * (speed * Time.deltaTime);
        tf.position = position;

        var rot = Quaternion.identity;
        rot.eulerAngles = new(
            -90,
            (Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg) + 180,
            0
        );
        tf.rotation = rot;
    }
}
