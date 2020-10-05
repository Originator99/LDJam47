using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombWorldObject : MonoBehaviour {
    public CollectableType ID;

    private float showTimout = 1f;

    private void Awake() {
        GameEventSystem.GameEventHandler += HandleGameEvents;
    }
    private void Start() {
        GameEventSystem.RaiseGameEvent(GAME_EVENT.POWER_UP_COLLECTED, ID);
    }
    private void OnDestroy() {
        GameEventSystem.GameEventHandler -= HandleGameEvents;
    }
    private void HandleGameEvents(GAME_EVENT type, System.Object data = null) {
        if(type == GAME_EVENT.LEVEL_END) {
            Destroy(gameObject);
        }
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
