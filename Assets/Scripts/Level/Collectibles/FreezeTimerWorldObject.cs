using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeTimerWorldObject : MonoBehaviour {
    public CollectableType ID;
    private void Awake() {
        GameEventSystem.GameEventHandler += HandleGameEvents;
    }
    private void OnDestroy() {
        GameEventSystem.GameEventHandler -= HandleGameEvents;
    }
    private void HandleGameEvents(GAME_EVENT type, System.Object data = null) {
        if(type == GAME_EVENT.LEVEL_END) {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject != null && collision.gameObject.CompareTag(GlobalConstants.player_tag)) {
            GameEventSystem.RaiseGameEvent(GAME_EVENT.POWER_UP_COLLECTED, ID);
            Destroy(gameObject);
        }
    }
}
