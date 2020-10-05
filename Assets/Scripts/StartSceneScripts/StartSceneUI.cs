using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartSceneUI : MonoBehaviour {
    public Button startButton;

    private void Start() {
        startButton.onClick.RemoveAllListeners();
        startButton.onClick.AddListener(delegate() {
            Loader.Load(SceneName.LEVEL_Test_1_1);
        });
    }
}
