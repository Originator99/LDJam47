using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FreezeTimerMenuObject : MonoBehaviour , ICollectable {
    public Button button;
    public Text count;

    private const KeyCode keycode = KeyCode.F;

    private TimeManager manager;

    private Animator playerAnimator;
    private float freezeTimer;

    private void Awake() {
        GameEventSystem.GameEventHandler += HandleGameEvents;
        manager = FindObjectOfType<TimeManager>();
        if(manager == null) {
            Debug.LogError("Time Manager is null in " + gameObject.name);
        }

        GameObject player = GameObject.FindGameObjectWithTag(GlobalConstants.player_tag);
        if(player != null) {
            playerAnimator = player.GetComponent<Animator>();
        } else {
            Debug.LogError("Player Object is null");
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
        if(freezeTimer > 0) {
            freezeTimer -= Time.deltaTime;
            //if(freezeTimer <= 0) {
            //    playerAnimator.SetTrigger("Idle");
            //}
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
                playerAnimator.SetTrigger("Freeze");
                freezeTimer = 5f;
                manager.DoSlowmotion(5f);
            }
            CheckAndSetCollectable();
        }   
    }
}
