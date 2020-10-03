using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public float speed;
    public float lifeTime;
    public GameObject destroyEffect;

    private void Update() {

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.up, 0.25f);
        if(hitInfo.collider != null) {
            Debug.Log("Bullet hit : " + hitInfo.collider.name);
            if(hitInfo.collider.tag != "Player") {
                DestroyProjectile();
            }
        }

        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void DestroyProjectile() {
        if(destroyEffect != null) {
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
        } else {
            Debug.LogError("Destroy effect is null for " + gameObject.name);    
        }
        Destroy(gameObject);
    }
}
