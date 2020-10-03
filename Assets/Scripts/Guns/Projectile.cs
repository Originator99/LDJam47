using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public float speed;
    public float lifeTime;
    public GameObject destroyEffect;

    [SerializeField]
    LayerMask collisionMask;

    private void Start()
    {
        Physics2D.queriesStartInColliders = false;
    }

    private void Update()
    {

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.up, 0.25f);
        if (hitInfo.collider != null)
        {
            Debug.Log("Bullet hit : " + hitInfo.collider.name);
            if (hitInfo.collider.tag != "Player")
            {
                DestroyProjectile();
            }
        }

        transform.Translate(Vector2.right * speed * Time.deltaTime);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, Time.deltaTime*speed+.1f, collisionMask);
        if (hit.collider != null)
        {
            Vector2 reflectDir = Vector2.Reflect(transform.right, hit.normal);
            float rot = Mathf.Atan2(reflectDir.y, reflectDir.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0,0, rot);
        }
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
