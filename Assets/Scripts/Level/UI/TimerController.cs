using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour {
    public Text timerText;

    private void Awake() {
        GameEventSystem.GameEventHandler += HandleGameEvents;
    }
    private void OnDestroy() {
        GameEventSystem.GameEventHandler -= HandleGameEvents;
    }

    private void HandleGameEvents(GAME_EVENT type, System.Object data = null) {
        if(type == GAME_EVENT.LEVEL_START && data!=null && data.GetType() == typeof(float)) {
        
        }
    }

    private void OnLevelStart(float timer) {
        
    }
}
