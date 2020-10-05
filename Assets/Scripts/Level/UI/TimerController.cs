using System;
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
        if(type == GAME_EVENT.LEVEL_START) {
            float timer = 0;
            if(data.GetType() == typeof(int)) {
                int temp = (int)data;
                timer = temp;
            } else if(data.GetType() == typeof(float)) {
                timer = (float)data;
            }
            OnLevelStart(timer);
        }
        if(type == GAME_EVENT.LEVEL_END) {
            OnLevelEnd();
        }
    }

    private void Update() {
        if(GameRunTimeHelper.CurrentTimer > 0) {
            GameRunTimeHelper.CurrentTimer -= Time.deltaTime;
            float minutes = Mathf.Floor(GameRunTimeHelper.CurrentTimer / 60);
            float seconds = Mathf.RoundToInt(GameRunTimeHelper.CurrentTimer % 60);

            timerText.text = minutes + " : " + seconds;
            if(GameRunTimeHelper.CurrentTimer <= 0) {
                GameEventSystem.RaiseGameEvent(GAME_EVENT.LEVEL_END, LEVEL_END_REASON.TIMER_EXPIRED);
            }
        }
    }

    private void OnLevelStart(float timer) {
        GameRunTimeHelper.CurrentTimer = timer;
    }

    private void OnLevelEnd() {
        GameRunTimeHelper.CurrentTimer = -1;
        timerText.text = "";
    }
}
