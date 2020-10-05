using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bomb : MonoBehaviour, ICollectable {
    public GameObject bombPrefab;
    public Button button;
    public Text count;

    private const KeyCode keycode = KeyCode.B;
    private GameObject player;

    private void Awake() {
        GameEventSystem.GameEventHandler += HandleGameEvents;
        player = GameObject.FindGameObjectWithTag(GlobalConstants.player_tag);
        if(player == null) {
            Debug.LogError("Player Object is null");
        }
    }
    private void Start() {
        CheckAndSetCollectable();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(delegate () {
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
            if((CollectableType)data == CollectableType.BOMB) {
                ScoreManager.OnBombCollected();
                CheckAndSetCollectable();
            }
        }
    }


    public void CheckAndSetCollectable() {
        count.text = "x" + ScoreManager.GetBombPowerupCount();
        if(ScoreManager.CanUseBombPowerup()) {
            button.interactable = true;
        } else {
            button.interactable = false;
        }
    }

    public void UseCollectable() {
        if(ScoreManager.CanUseBombPowerup()) {
            ScoreManager.OnBombTimePowerupUsed();

            if(bombPrefab != null) {
                Instantiate(bombPrefab, player.transform.position, Quaternion.identity);
            }

            CheckAndSetCollectable();
        }
    }
}
