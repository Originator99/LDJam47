using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelEndUI : MonoBehaviour {
    public Text header;
    public Button retryButton, mainscreenButton;
    public Text retryButtonText;
    public Text platformsDestroyed, obstaclesDestroyed, enemiesKilled, timeTaken;

    public AudioSource endSoundClip;

    public void ShowEndScreen(LEVEL_END_REASON reason) {
        RenderData(reason);
        gameObject.SetActive(true);
        endSoundClip.Play();
    }

    public void HideEndScreen() {
        gameObject.SetActive(false);
    }

    private void RenderData(LEVEL_END_REASON reason) {
        header.text = GetHeaderText(reason);
        platformsDestroyed.text = "x" + ScoreManager.GetPlatformsDestroyed();
        obstaclesDestroyed.text = "x" + ScoreManager.GetObstaclesDestroyed();
        enemiesKilled.text = "x" + ScoreManager.GetEnemiesKilled();
        timeTaken.text = GetTimeTaken();

        SetButtons(reason);
    }

    private string GetTimeTaken() {
        int time = 0;
        var levelData = LevelController.instance.levelData;
        foreach(var data in levelData) {
            time += (data.levelTimer - (data.levelTimer + data.currentTimer));
        }
        float minutes = Mathf.Floor(time / 60);
        float seconds = Mathf.RoundToInt(time % 60);
        return minutes + " : " + seconds;
    }

    private void SetButtons(LEVEL_END_REASON reason) {
        retryButtonText.text = reason == LEVEL_END_REASON.OBJECTIVE_COLLECTED ? "Replay" : "Retry";
        retryButton.onClick.RemoveAllListeners();
        retryButton.onClick.AddListener(delegate {
            LevelController.instance.RestartLevel();
            HideEndScreen();
        });
        mainscreenButton.onClick.RemoveAllListeners();
        mainscreenButton.onClick.AddListener(delegate() {
            Loader.Load(SceneName.MainScene);
        });
    }

    private string GetHeaderText(LEVEL_END_REASON reason) {
        if(reason == LEVEL_END_REASON.OBJECTIVE_COLLECTED)
            return "LEVEL COMPLETE!";
        return "YOU DIED..";
    }
}
