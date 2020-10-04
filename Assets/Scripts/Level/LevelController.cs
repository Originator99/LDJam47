using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour {
    public static LevelController instance;
    public List<LevelSO> levelData;

    public LevelSO currentLevelSO;



    private void Awake() {
        if(instance != null) {
            Destroy(gameObject);
        } else {
            instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += HandleSceneLoad;
            GameEventSystem.GameEventHandler += HandleGameEvents;
            currentLevelSO = null;

            StartLevel();
        }
    }

    private void OnDestroy() {
        SceneManager.sceneLoaded -= HandleSceneLoad;
        GameEventSystem.GameEventHandler -= HandleGameEvents;
    }

    private void HandleSceneLoad(Scene scene, LoadSceneMode mode) {
        if(scene.name == SceneName.MainScene.ToString()) {
            Destroy(gameObject);
            instance = null;
        } else if(scene.name != SceneName.Loader.ToString()) {
            if(currentLevelSO != null) {
                GameEventSystem.RaiseGameEvent(GAME_EVENT.LEVEL_START, currentLevelSO.levelTimer);
            }
        }
    }

    private void HandleGameEvents(GAME_EVENT type, System.Object data = null) {
        
    }

    private void StartLevel() {
        if(levelData != null && levelData.Count > 0) {
            currentLevelSO = levelData[0];
            GameEventSystem.RaiseGameEvent(GAME_EVENT.LEVEL_START, currentLevelSO.levelTimer);

        } else {
            Debug.LogError("Level data is null");
        }
    }

    public void RestartLevel() {
        if(levelData != null && levelData.Count > 0) {
            currentLevelSO = levelData[0];
            if(SceneManager.GetActiveScene().name == currentLevelSO.ID.ToString()) {
                GameEventSystem.RaiseGameEvent(GAME_EVENT.REST_LEVEL);
            } else {
                Loader.Load(currentLevelSO.ID);
            }
        } else {
            Debug.LogError("Level data is null");
        }
    }

    public void SwitchLevel(SceneName ID) {
        if(currentLevelSO.ID != ID) {
            currentLevelSO = levelData.Find(x => x.ID == ID);
            if(currentLevelSO != null) {
                Loader.Load(currentLevelSO.ID);
            } else {
                Debug.LogError("Invalid level ID " + ID.ToString() + " to switch the level");
            }
        } else {
            Debug.LogWarning("Trying to switch level where player is already on!");
        }
    }
}
