using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private void Awake() {
        GameEventSystem.GameEventHandler += HandleGameEvents;
    }

    private void Start() {
        GameEventSystem.RaiseGameEvent(GAME_EVENT.LEVEL_START, 120);
    }

    private void OnDestroy() {
        GameEventSystem.GameEventHandler -= HandleGameEvents;
    }

    private void HandleGameEvents(GAME_EVENT type, System.Object data = null) {
        if(type == GAME_EVENT.LEVEL_START) {
            OnLevelStart();
        }
        if(type == GAME_EVENT.LEVEL_END && data!=null && data.GetType() == typeof(LEVEL_END_REASON)) {
            OnLevelEnd((LEVEL_END_REASON)data);
        }
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.PageUp)) {
            GameEventSystem.RaiseGameEvent(GAME_EVENT.REST_LEVEL);
        }
    }

    private void OnLevelStart() {
        GameRunTimeHelper.GameOver = false;
    }

    private void OnLevelEnd(LEVEL_END_REASON reason) {
        if(!GameRunTimeHelper.GameOver) {
            GameRunTimeHelper.GameOver = true;
            Debug.Log("Game over" + reason.ToString());
        } else {
            Debug.LogWarning("Level end event called multiple times. Ignoring the second call");
        }
    }
}
