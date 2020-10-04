using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader {
    private class LoadingMonoBehaviour :MonoBehaviour {
    }

    private static Action OnLoaderCallback;
    private static AsyncOperation asyncOperation;
    public static void Load(SceneName name) {
        OnLoaderCallback = () => {
            GameObject loadingGameObject = new GameObject("Loading Game Object");
            loadingGameObject.AddComponent<LoadingMonoBehaviour>().StartCoroutine(LoadSceneAsync(name));
        };

        SceneManager.LoadScene(SceneName.Loader.ToString());
    }

    private static IEnumerator LoadSceneAsync(SceneName name) {
        yield return null;
        asyncOperation = SceneManager.LoadSceneAsync(name.ToString());
        while(!asyncOperation.isDone) {
            yield return null;
        }
    }

    public static float GetLoadingProgress() {
        if(asyncOperation != null) {
            return asyncOperation.progress;
        }
        return 1f;
    }

    public static void LoaderCallback() {
        OnLoaderCallback?.Invoke();
        OnLoaderCallback = null;
    }
}

public enum SceneName {
    Loader,
    MainScene,
    LEVEL_1_1,
    LEVEL_1_2,
    LEVEL_1_3,
    LEVEL_1_4,
}