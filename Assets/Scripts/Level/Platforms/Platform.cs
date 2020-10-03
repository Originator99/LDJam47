using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform {
    public Transform transform;
    public Vector2 originalPosition;
    public Quaternion originalRotation;

    public Platform(Transform transform) {
        this.transform = transform;
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    public void Reset() {
        if(transform != null) {
            transform.gameObject.SetActive(true);
            transform.position = originalPosition;
            transform.rotation = originalRotation;
        }
    }
}
