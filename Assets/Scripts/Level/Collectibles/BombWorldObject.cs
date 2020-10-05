using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombWorldObject : MonoBehaviour {
    public CollectableType ID;

    private float showTimout = 1f;

    private void Start() {
        GameEventSystem.RaiseGameEvent(GAME_EVENT.POWER_UP_COLLECTED, ID);
    }

    private void Update() {
        if(showTimout > 0) {
            showTimout -= Time.deltaTime;
            if(showTimout <= 0) {
                Destroy(gameObject);
            }
        }
    }

}
