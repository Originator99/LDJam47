using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikePoint : MonoBehaviour {
    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject != null && collision.gameObject.CompareTag(GlobalConstants.player_tag)) {
            Debug.Log("Player dead");
            GameEventSystem.RaiseGameEvent(GAME_EVENT.LEVEL_END, LEVEL_END_REASON.PLAYER_DEAD);
        }
    }
}
