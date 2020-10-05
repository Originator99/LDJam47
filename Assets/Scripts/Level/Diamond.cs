using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject != null && collision.gameObject.CompareTag(GlobalConstants.player_tag) && !GameRunTimeHelper.GameOver) {
            GameEventSystem.RaiseGameEvent(GAME_EVENT.LEVEL_END, LEVEL_END_REASON.OBJECTIVE_COLLECTED);
        }
    }
}
