using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUIController : MonoBehaviour {
    public LevelEndUI endLevelController;

    private void Awake() {
        GameEventSystem.GameEventHandler += HandleGameEvents;
        endLevelController?.HideEndScreen();
    }
    private void OnDestroy() {
        GameEventSystem.GameEventHandler -= HandleGameEvents;
    }

    private void HandleGameEvents(GAME_EVENT type, System.Object data = null) {
        if(type == GAME_EVENT.LEVEL_END) {
            endLevelController?.ShowEndScreen((LEVEL_END_REASON)data);
        }
    }
}
