using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderCallback :MonoBehaviour {
    private bool isFirstFrame = true;
    private void Update() {
        if(isFirstFrame) {
            StartCoroutine(DelayedLoadingCallback());
            isFirstFrame = false;
        }
    }

    private IEnumerator DelayedLoadingCallback() {
        yield return new WaitForSeconds(1f);
        Loader.LoaderCallback();
    }
}