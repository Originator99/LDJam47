using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour {
    public Text timerText;

    private float currentTimer;

    private void Awake() {
        GameEventSystem.GameEventHandler += HandleGameEvents;
    }
    private void OnDestroy() {
        GameEventSystem.GameEventHandler -= HandleGameEvents;
    }
    private void HandleGameEvents(GAME_EVENT type, System.Object data = null) {
        if(type == GAME_EVENT.LEVEL_START && data!=null && data.GetType() == typeof(int)) {
            OnLevelStart((int)data);
        }
        if(type == GAME_EVENT.LEVEL_END) {
            OnLevelEnd();
        }
    }

    private void Update() {
        if(currentTimer > 0) {
            currentTimer -= Time.deltaTime;
            float minutes = Mathf.Floor(currentTimer / 60);
            float seconds = Mathf.RoundToInt(currentTimer % 60);

            timerText.text = minutes + " : " + seconds;
            if(currentTimer <= 0) {
                GameEventSystem.RaiseGameEvent(GAME_EVENT.LEVEL_END, LEVEL_END_REASON.TIMER_EXPIRED);
            }
        }
    }

    private void OnLevelStart(int timer) {
        currentTimer = timer;
    }

    private void OnLevelEnd() {
        currentTimer = -1;
        timerText.text = "";
    }
}
