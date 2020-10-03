using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    private void Update() {
        if(Input.GetKeyDown(KeyCode.PageUp)) {
            GameEventSystem.RaiseGameEvent(GAME_EVENT.REST_LEVEL);
        }
    }
}
