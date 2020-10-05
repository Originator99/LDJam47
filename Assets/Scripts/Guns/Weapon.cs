using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class Weapon : MonoBehaviour {
    public float offset;
    public GameObject projectile;
    public Transform shootPoint;
    public AudioSource shootSound;

    private float timeBtwShots;
    public float fireRate;

    private int facingDirection;
    private float minRotClamp, maxRotClamp;

    float yAngle = 0;

    CameraShake cameraShake;

    private void Start() {
        cameraShake = FindObjectOfType<CameraShake>();

        facingDirection = 1;
        minRotClamp = -90;
        maxRotClamp = 90;
    }

    private void Update() {
        Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float rotz = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, yAngle, rotz + offset);
        //if (Input.GetKeyDown(KeyCode.LeftArrow))
        //{
        //    minRotClamp = 90f;
        //    maxRotClamp = 180f;
        //    facingDirection = -1;
        //    //yAngle = -180f;
        //}
        //else if (Input.GetKeyDown(KeyCode.RightArrow))
        //{
        //    minRotClamp = -90f;
        //    maxRotClamp = 90f;
        //    facingDirection = 1;
        //    //yAngle = 0f;
        //}

        //float temp = rotz + offset;

        //if (facingDirection == -1)
        //{
        //    if (temp < -90)
        //    {
        //        minRotClamp = -180f;
        //        maxRotClamp = -90f;
        //        transform.rotation = Quaternion.Euler(0f, yAngle, Mathf.Clamp(rotz + offset, minRotClamp, maxRotClamp));
        //    }
        //    else
        //    {
        //        minRotClamp = 90f;
        //        maxRotClamp = 180f;
        //        transform.rotation = Quaternion.Euler(0f, yAngle, Mathf.Clamp(rotz + offset, minRotClamp, maxRotClamp));
        //    }
        //}
        //else
        //    transform.rotation = Quaternion.Euler(0f, yAngle, Mathf.Clamp(rotz + offset, minRotClamp, maxRotClamp));

        if(timeBtwShots <= 0) {
            if(Input.GetMouseButtonDown(0) && !GameRunTimeHelper.GameOver) {
                timeBtwShots = fireRate;
                Instantiate(projectile, shootPoint.position, transform.rotation);
                shootSound.Play();
            }
        } else {
            timeBtwShots -= Time.unscaledDeltaTime;
        }

       
    }
}
