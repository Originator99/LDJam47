﻿using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDestructionController : MonoBehaviour {
    public GameObject platformMiniDestroyEffect, platformBigDestroyEffect;

    private List<Platform> destroyedPlatforms;
    private List<Platform> destroyedObstacles;

    private void Awake() {
        destroyedPlatforms = new List<Platform>();
        destroyedObstacles = new List<Platform>();
        GameEventSystem.GameEventHandler += HandleGameEvents;
    }
    private void OnDestroy() {
        GameEventSystem.GameEventHandler -= HandleGameEvents;
    }

    private void HandleGameEvents(GAME_EVENT type, System.Object data = null) {
        if(type == GAME_EVENT.PLATFORM_DESTROYED && data.GetType() == typeof(Transform)) {
            OnPlatformDestroyed(data as Transform);
        }
        if(type == GAME_EVENT.OBSTACLE_DESTROYED && data.GetType() == typeof(Transform)) {
            OnObstacleDestroyed(data as Transform);
        }
        if(type == GAME_EVENT.REST_LEVEL) {
            ResetLevelDestruction();
        }
    }

    private void ResetLevelDestruction() {
        if(destroyedPlatforms != null) {
            foreach(var platform in destroyedPlatforms) {
                platform.Reset();
            }
            destroyedPlatforms.Clear();
        }
        if(destroyedObstacles != null) {
            foreach(var obstacle in destroyedObstacles) {
                obstacle.Reset();
            }
            destroyedObstacles.Clear();
        }
    }

    private void OnPlatformDestroyed(Transform destroyedPlatform) {
        if(destroyedPlatforms == null)
            destroyedPlatforms = new List<Platform>();

        DoPlatformDestroyEffect(destroyedPlatform);
        destroyedPlatform.gameObject.SetActive(false);

        Platform platform = new Platform(destroyedPlatform);
        destroyedPlatforms.Add(platform);
    }

    private void OnObstacleDestroyed(Transform obstacle) {
        if(destroyedObstacles == null)
            destroyedObstacles = new List<Platform>();

        Platform obs = new Platform(obstacle);
        destroyedObstacles.Add(obs);
    }

    private void DoPlatformDestroyEffect(Transform platform) {
        if(platform.CompareTag(GlobalConstants.platform_one_way_tag) && platformMiniDestroyEffect != null) {
            Instantiate(platformMiniDestroyEffect, platform.position, platform.rotation);
        } else if(platform.CompareTag(GlobalConstants.platform_big_tag) && platformBigDestroyEffect != null) {
            Instantiate(platformBigDestroyEffect, platform.position, platform.rotation);
        }
    }
}
