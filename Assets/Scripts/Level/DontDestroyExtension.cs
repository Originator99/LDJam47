using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyExtension : MonoBehaviour {
    #region Internal class
    public static class DontDestroyExtensionHelper {
        private static List<string> initializedObjects;

        public static bool CheckIfInitialized(string ID) {
            if(initializedObjects != null) {
                int index = initializedObjects.FindIndex(x => x == ID);
                return index != -1;
            }
            return false;
        }

        public static void Add(string ID) {
            if(initializedObjects == null)
                initializedObjects = new List<string>();
            int index = initializedObjects.FindIndex(x => x == ID);
            if(index == -1) {
                initializedObjects.Add(ID);
            } else {
                Debug.LogError("Dont destroy extension already has this same ID " + ID);
            }
        }

        public static void Remove(string ID) {
            if(initializedObjects != null)
                initializedObjects.Remove(ID);
        }
    }
    #endregion

    public SceneName sceneToResetOn;
    public SceneName sceneToActivateOn;

    private string _ID;
    private void Awake() {
        _ID = gameObject.name + ">" + SceneManager.GetActiveScene().name;
        gameObject.name = _ID;
        if(!DontDestroyExtensionHelper.CheckIfInitialized(_ID)) {
            DontDestroyExtensionHelper.Add(_ID);
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += HandleSceneSwitch;
        } else {
            Destroy(gameObject);
        }
    }

    private void OnDestroy() {
        SceneManager.sceneLoaded -= HandleSceneSwitch;
    }

    private void HandleSceneSwitch(Scene scene, LoadSceneMode mode) {
        if(scene.name == sceneToResetOn.ToString()) {
            Destroy(gameObject);
            DontDestroyExtensionHelper.Remove(_ID);
        } else if(scene.name == sceneToActivateOn.ToString()) {
            gameObject.SetActive(true);
        } else {
            gameObject.SetActive(false);
        }
    }
}
