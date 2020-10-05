using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spike : MonoBehaviour {
    public Rigidbody2D rb;
    public BoxCollider2D collider;
    public Transform attachPoint;
    public LayerMask groundLayerMask;

    private const string PLATFORM_TAG = "destructible";
    private bool eventSent;
    private bool onGround;

    private Vector2 ogPos;
    private void Awake() {
        ogPos = transform.position;
    }

    private void OnEnable() {
        ogPos = transform.position;
    }

    private void Start() {
        onGround = false;
        eventSent = false;
        rb.gravityScale = 0;
    }

    private void Update() {
        RaycastHit2D platformRayHitCheck = Physics2D.Raycast(attachPoint.position, Vector2.up, 1f);
        Debug.DrawRay(attachPoint.position, Vector2.up);
        if(platformRayHitCheck.collider != null && platformRayHitCheck.collider.tag.Contains(PLATFORM_TAG)) {
            rb.gravityScale = 0;
            eventSent = false;
            onGround = false;
        } else {
            if(!eventSent) {
                eventSent = true;
                GameEventSystem.RaiseGameEvent(GAME_EVENT.OBSTACLE_DESTROYED, transform);
                rb.gravityScale = 1;
            }
        }
        Vector2 offset = (Vector2)transform.position - ogPos;
        Debug.Log(offset);
        if(offset.y > 1f) {
            onGround = true;
        } else if(offset.y < -1f) {
            onGround = true;
        } else {
            onGround = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(!onGround) {
            if(collision.gameObject != null && collision.gameObject.CompareTag(GlobalConstants.player_tag)) {
                Debug.Log("Player dead");
                GameEventSystem.RaiseGameEvent(GAME_EVENT.LEVEL_END, LEVEL_END_REASON.PLAYER_DEAD);
            }
        }

        if(collision.gameObject != null && collision.gameObject.layer == 8) {
            onGround = true;
        }
    }
}
