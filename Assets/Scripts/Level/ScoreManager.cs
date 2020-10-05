using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScoreManager {
    private static int platformsDestroyedCount, obstaclesDestroyedCount, enemiesKilledCount;
    private static int freezeTimePowerupCount, bombPowerupCount;
    public static void Init(){
        platformsDestroyedCount = 0;
        obstaclesDestroyedCount = 0;
        enemiesKilledCount = 0;

        freezeTimePowerupCount = 0;
        bombPowerupCount = 0;
    }

    public static void OnPlatformDestroyed() {
        platformsDestroyedCount++;
    }
    public static void OnObstaclesDestroyed() {
        obstaclesDestroyedCount++;
    }
    public static void OnEnemiesKilled() {
        enemiesKilledCount++;
    }
    public static void OnFreezeTimeCollected() {
        freezeTimePowerupCount++;
    }
    public static void OnBombCollected() {
        bombPowerupCount++;
    }

    public static int GetPlatformsDestroyed() {
        return platformsDestroyedCount;
    }
    public static int GetObstaclesDestroyed() {
        return obstaclesDestroyedCount;
    }
    public static int GetEnemiesKilled() {
        return enemiesKilledCount;
    }

    public static int GetFreeTimePowerupCount() {
        return freezeTimePowerupCount;
    }

    public static int GetBombPowerupCount() {
        return bombPowerupCount;
    }

    public static bool CanUseFreezeTimePowerup() {
        return freezeTimePowerupCount > 0;
    }
    public static bool CanUseBombPowerup() {
        return bombPowerupCount > 0;
    }

    public static void OnFreezeTimePowerupUsed() {
        freezeTimePowerupCount--;
        if(freezeTimePowerupCount < 0) {
            freezeTimePowerupCount = 0;
        }
    }

    public static void OnBombTimePowerupUsed() {
        bombPowerupCount--;
        if(bombPowerupCount < 0) {
            bombPowerupCount = 0;
        }
    }
}
