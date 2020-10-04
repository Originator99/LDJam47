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
            GenerateDiamond();
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
        if(type == GAME_EVENT.PLATFORM_DESTROYED) {
            OnPlatformDestroyed(data as Transform);
        }
        if(type == GAME_EVENT.OBSTACLE_DESTROYED) {
            OnObstacleDestroyed(data as Transform);
        }
        if(type == GAME_EVENT.ENEMY_KILLED) {
            OnEnemyKilled(data as Transform);
        }
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

    #region Diamond / Objective Controls
    [Header("Diamond Variables")]
    public GameObject diamondPrefab;
    public int minPlatformsToDestroy, maxPlatformsToDestroy;
    public int minObstaclesToDestroy, maxObstaclesToDestroy;
    public int minEnemiesToKill, maxEnemiesToKill;

    private float platforms, obstacles, enemies;
    private float currentPlatforms, currentObstacles, currentEnemies;

    private string sceneToPutDiamond;
    private bool diamondInstantiated;

    private void GenerateDiamond() {
        diamondInstantiated = false;
        platforms = Random.Range(minPlatformsToDestroy, maxPlatformsToDestroy);
        obstacles = Random.Range(minObstaclesToDestroy, maxObstaclesToDestroy);
        enemies = Random.Range(minEnemiesToKill, maxEnemiesToKill);

        currentPlatforms = 0;
        currentObstacles = 0;
        currentEnemies = 0;

        int n = Random.Range(0, levelData.Count);
        sceneToPutDiamond = levelData[n].ID.ToString();

        Debug.Log("platforms : " + platforms + " obstacles : " + obstacles + " enemies : " + enemies + " scene : " + sceneToPutDiamond);
    }

    private void OnPlatformDestroyed(Transform obj) {
        if(currentPlatforms < platforms) {
            currentPlatforms++;
        }
        UpdateForDiamond(obj);
    }
    private void OnObstacleDestroyed(Transform obj) {
        if(currentObstacles < obstacles) {
            currentObstacles++;
        }
        UpdateForDiamond(obj);
    }
    private void OnEnemyKilled(Transform obj) {
        if(currentEnemies < enemies) {
            currentEnemies++;
        }
        UpdateForDiamond(obj);
    }

    private void UpdateForDiamond(Transform obj) {
        if(currentEnemies == enemies && currentObstacles == obstacles && currentPlatforms == platforms && SceneManager.GetActiveScene().name == sceneToPutDiamond && !diamondInstantiated) {
            diamondInstantiated = true;
            Instantiate(diamondPrefab, obj.position, Quaternion.identity);
        }
    }

    #endregion
}
