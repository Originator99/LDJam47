using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    public float offset;
    public GameObject projectile;
    public Transform shootPoint;

    private float timeBtwShots;
    public float fireRate;

    private int facingDirection;
    private float minRotClamp, maxRotClamp;

    private void Start() {
        facingDirection = 0;
        minRotClamp = -90;
        maxRotClamp = 90;
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.LeftArrow)) {
            minRotClamp = 0;
            maxRotClamp = 180;
            facingDirection = -1;
        } else if(Input.GetKeyDown(KeyCode.RightArrow)) {
            minRotClamp = -90f;
            maxRotClamp = 90;
            facingDirection = 1;
        }
        Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float rotz = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Clamp(rotz + offset, minRotClamp, maxRotClamp));

        if(timeBtwShots <= 0) {
            if(Input.GetMouseButtonDown(0)) {
                timeBtwShots = fireRate;
                Instantiate(projectile, shootPoint.position, transform.rotation);
            }
        } else {
            timeBtwShots -= Time.unscaledDeltaTime;
        }

       
    }
}
