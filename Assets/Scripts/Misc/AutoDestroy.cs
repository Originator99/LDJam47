using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Destroys a gameobject after a couple of seconds
/// </summary>
public class AutoDestroy : MonoBehaviour {

    public float destroyAfter = 7f;

    private void Start() {
        Invoke("DestroyAfter", destroyAfter);
    }

    private void DestroyAfter() {
        if(gameObject != null) {
            Destroy(gameObject);
        }
    }
}
