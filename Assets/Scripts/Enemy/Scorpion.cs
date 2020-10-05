using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Scorpion : MonoBehaviour {
    [SerializeField]
    LayerMask lookLayerMask;
    [SerializeField]
    LayerMask shootLayerMask;
    [SerializeField]
    LayerMask deathLayerMask;

    public Transform actualScorpion;
    public Transform shootPoint, lookPoint, playerDetectionPoint;
    public Transform leftPoint, rightPoint;
    
    public GameObject projectile;
    public AudioSource shootSound;

    private float timeBtwShots;
    public float fireRate;
    public float fireRange;

    private int facingDirection;
    private bool isShooting;

    private Vector2 originalPosition;
    private Vector3 originalRotation;

    private void Awake() {
        originalPosition = actualScorpion.position;
        originalRotation = actualScorpion.eulerAngles;
    }

    private void Start() {
        isShooting = false;
        facingDirection = -1;
    }

    private void OnEnable() {
        actualScorpion.position = originalPosition;
        actualScorpion.eulerAngles = originalRotation;
    }

    private void Update() {
        if(GameRunTimeHelper.GameOver)
            return;

        RaycastHit2D lookPoints = Physics2D.Raycast(lookPoint.position, lookDirection(), 0.5f, lookLayerMask);
        if(lookPoints.collider != null) {
            FlipCharacter();
        }
        if(timeBtwShots > 0) {
            timeBtwShots -= Time.deltaTime;
        }
        RaycastHit2D shootPoint = Physics2D.Raycast(playerDetectionPoint.position, lookDirection(), fireRange, shootLayerMask);
        if(shootPoint.collider != null) {
            if(timeBtwShots <= 0) {
                Shoot(shootPoint.collider.transform);
            }
            isShooting = true;
        } else {
            isShooting = false;
        }

        if(!isShooting) {
            Vector2 target = GetMovePoint();
            if(Vector2.Distance(actualScorpion.position, target) <= 1.5f) {
                FlipCharacter();
            }

            actualScorpion.position = Vector2.MoveTowards(actualScorpion.position, GetMovePoint(), 2f * Time.deltaTime);
            if(actualScorpion.position.y > originalPosition.y || actualScorpion.position.y < originalPosition.y) {
                RaycastHit2D deathCheck = Physics2D.Raycast(actualScorpion.position, Vector2.down, 2f, deathLayerMask);
                if(deathCheck.collider != null) {
                    OnDead();
                }
            }
        }

    }

    private void Shoot(Transform shootAtTransform) {
        timeBtwShots = fireRate;
        Instantiate(projectile, shootPoint.position, actualScorpion.rotation);
    }

    private void FlipCharacter() {
        if(facingDirection == -1) {
            facingDirection = 1;
            actualScorpion.eulerAngles = new Vector3(0, -180, 0);
        } else {
            facingDirection = -1;
            actualScorpion.eulerAngles = new Vector3(0, 0, 0);
        }
    }

    private Vector2 GetMovePoint() {
        if(facingDirection == -1) {
            return new Vector2(leftPoint.position.x, actualScorpion.position.y);
        }
        return new Vector2(rightPoint.position.x, actualScorpion.position.y);
    }

    private Vector2 lookDirection() {
        if(facingDirection == 1) {
            return Vector2.right;
        }
        return Vector2.left;
    }

    public void OnDead() {
        GameEventSystem.RaiseGameEvent(GAME_EVENT.ENEMY_KILLED, transform);
        gameObject.SetActive(false);
    }
}
