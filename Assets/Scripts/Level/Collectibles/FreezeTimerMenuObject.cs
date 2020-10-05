using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FreezeTimerMenuObject : MonoBehaviour , ICollectable {
    public Button button;
    public Text count;

    private const KeyCode keycode = KeyCode.F;

    private TimeManager manager;

    private void Awake() {
        GameEventSystem.GameEventHandler += HandleGameEvents;
        manager = FindObjectOfType<TimeManager>();
        if(manager == null) {
            Debug.LogError("Time Manager is null in " + gameObject.name);
        }
    }

    private void Start() {
        CheckAndSetCollectable();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(delegate() {
            UseCollectable();
        });
    }

    private void Update() {
        if(Input.GetKeyDown(keycode)) {
            UseCollectable();
        }
    }

    private void OnDestroy() {
        GameEventSystem.GameEventHandler -= HandleGameEvents;
    }

    private void HandleGameEvents(GAME_EVENT type, System.Object data = null) {
        if(type == GAME_EVENT.LEVEL_START) {
            CheckAndSetCollectable();
        }
        if(type == GAME_EVENT.POWER_UP_COLLECTED && data != null && data.GetType() == typeof(CollectableType)) {
            if((CollectableType)data == CollectableType.FREEZE_TIME) {
                ScoreManager.OnFreezeTimeCollected();
                CheckAndSetCollectable();
            }
        }
    }

    public void CheckAndSetCollectable() {
        count.text = "x" + ScoreManager.GetFreeTimePowerupCount();
        if(ScoreManager.CanUseFreezeTimePowerup()) {
            button.interactable = true;
        } else {
            button.interactable = false;
        }
    }

    public void UseCollectable() {
        if(ScoreManager.CanUseFreezeTimePowerup()) {
            ScoreManager.OnFreezeTimePowerupUsed();
            if(manager != null) {
                manager.DoSlowmotion(5f);
            }
            CheckAndSetCollectable();
        }   
    }
}
