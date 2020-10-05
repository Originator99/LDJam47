using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeTimerWorldObject : MonoBehaviour {
    public CollectableType ID;
    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject != null && collision.gameObject.CompareTag("Player")) {
            GameEventSystem.RaiseGameEvent(GAME_EVENT.POWER_UP_COLLECTED, ID);
            Destroy(gameObject);
        }
    }
}
