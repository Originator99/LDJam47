using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour {
    public static LevelController instance;
    public List<LevelSO> levelData;

    public LevelSO currentLevelSO;

    [Header("Power ups")]
    public GameObject freezeTimePrefab;
    public GameObject bombPrefab;

    private void Awake() {
        if(instance != null) {
            Destroy(gameObject);
        } else {
            instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += HandleSceneLoad;
            GameEventSystem.GameEventHandler += HandleGameEvents;
            HandleDefaults();
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
                float timer = currentLevelSO.levelTimer;
                if(currentLevelSO.currentTimer > 0) {
                    timer = currentLevelSO.currentTimer;
                }
                GameEventSystem.RaiseGameEvent(GAME_EVENT.LEVEL_START, timer);
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

    private void HandleDefaults() {
        ResetCurrentTimers();
        ScoreManager.Init();
        currentLevelSO = null;
        GenerateDiamond();
        StartLevel();
    }

    private void ResetCurrentTimers() {
        if(levelData != null && levelData.Count > 0) {
            foreach(var data in levelData) {
                data.currentTimer = 0;
            }

        } else {
            Debug.LogError("Level data is null");
        }
    }

    private void StartLevel() {
        if(levelData != null && levelData.Count > 0) {
            currentLevelSO = levelData[0];
            if(SceneManager.GetActiveScene().name == currentLevelSO.ID.ToString()) {
                GameEventSystem.RaiseGameEvent(GAME_EVENT.LEVEL_START, currentLevelSO.levelTimer);
            } else {
                Loader.Load(currentLevelSO.ID);
            }

        } else {
            Debug.LogError("Level data is null");
        }
    }

    public void RestartLevel() {
        GameEventSystem.RaiseGameEvent(GAME_EVENT.REST_LEVEL);
        HandleDefaults();
    }

    public void SwitchLevel(SceneName ID) {
        if(currentLevelSO.ID != ID) {
            currentLevelSO.currentTimer = (int)GameRunTimeHelper.CurrentTimer;
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


    private string sceneToPutDiamond;
    private bool diamondInstantiated;

    private void GenerateDiamond() {
        diamondInstantiated = false;
        platforms = Random.Range(minPlatformsToDestroy, maxPlatformsToDestroy);
        obstacles = Random.Range(minObstaclesToDestroy, maxObstaclesToDestroy);
        enemies = Random.Range(minEnemiesToKill, maxEnemiesToKill);

        int n = Random.Range(0, levelData.Count);
        sceneToPutDiamond = levelData[n].ID.ToString();

        Debug.Log("platforms : " + platforms + " obstacles : " + obstacles + " enemies : " + enemies + " scene : " + sceneToPutDiamond);
    }

    private void OnPlatformDestroyed(Transform obj) {
        ScoreManager.OnPlatformDestroyed();
        UpdateForDiamond(obj);
        CheckCollectableDrop(obj, GAME_EVENT.PLATFORM_DESTROYED);
    }
    private void OnObstacleDestroyed(Transform obj) {
        ScoreManager.OnObstaclesDestroyed();
        UpdateForDiamond(obj);
        CheckCollectableDrop(obj, GAME_EVENT.OBSTACLE_DESTROYED);

    }
    private void OnEnemyKilled(Transform obj) {
        ScoreManager.OnEnemiesKilled();
        UpdateForDiamond(obj);
        CheckCollectableDrop(obj, GAME_EVENT.ENEMY_KILLED);
    }

    private void UpdateForDiamond(Transform obj) {
        if(ScoreManager.GetEnemiesKilled() >= enemies && ScoreManager.GetObstaclesDestroyed() >= obstacles && ScoreManager.GetPlatformsDestroyed() >= platforms && SceneManager.GetActiveScene().name == sceneToPutDiamond && !diamondInstantiated) {
            diamondInstantiated = true;
            Instantiate(diamondPrefab, obj.position, Quaternion.identity);
        }
        
        Debug.Log("platforms : " + ScoreManager.GetPlatformsDestroyed() + " obstacles : " + ScoreManager.GetObstaclesDestroyed() + " enemies : " + ScoreManager.GetEnemiesKilled() + " scene : " + sceneToPutDiamond);
    }

    #endregion

    #region Collectables Logic
    private void CheckCollectableDrop(Transform objectDestroyed, GAME_EVENT type) {
        int chance = 0;
        List<CollectableType> types = new List<CollectableType>();
        if(type == GAME_EVENT.PLATFORM_DESTROYED) {
            chance = 7;
            types.Add(CollectableType.FREEZE_TIME);
        } else if(type == GAME_EVENT.OBSTACLE_DESTROYED) {
            chance = 10;
            types.Add(CollectableType.FREEZE_TIME);
        } else if(type == GAME_EVENT.ENEMY_KILLED) {
            chance  = 100;
            types.Add(CollectableType.BOMB);
        }

        int temp = Random.Range(1, 101);
        if(temp <= chance) {
            if(types != null && types.Count > 0) {
                int index = Random.Range(0, types.Count);
                if(types.Count > index) {
                    SpawnPowerup(objectDestroyed, types[index]);
                } else {
                    Debug.LogError("Types enum array has invalid count and index range");
                }
            } else {
                Debug.LogError("Types enum array is null");
            }
        }
    }

    private void SpawnPowerup(Transform parent, CollectableType type) {
        if(parent != null) {
            GameObject go = null;
            if(type == CollectableType.FREEZE_TIME && freezeTimePrefab != null) {
                go = Instantiate(freezeTimePrefab, parent.position, Quaternion.identity);
            } else if(type == CollectableType.BOMB && bombPrefab != null) {  
                go = Instantiate(bombPrefab, parent.position, Quaternion.identity);
            }
            go.SetActive(true);
        }
    }
    #endregion
}
