using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    public float offset;
    public GameObject projectile;
    public Transform shootPoint;

    private float timeBtwShots;
    public float fireRate;

    private void Update() {
        Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float rotz = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotz + offset);

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
