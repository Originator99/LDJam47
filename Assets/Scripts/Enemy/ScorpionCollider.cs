using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorpionCollider : MonoBehaviour {
    public Scorpion controller;

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.name.Contains("Spike")) {
            controller.OnDead();
        }
    }
}
