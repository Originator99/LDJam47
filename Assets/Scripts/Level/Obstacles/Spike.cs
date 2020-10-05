using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour {
    public Rigidbody2D rb;
    public BoxCollider2D collider;
    public Transform attachPoint;
    public LayerMask groundLayerMask;

    private const string PLATFORM_TAG = "destructible";
    private bool eventSent;
    private bool onGround;

    private void Start() {
        onGround = false;
        eventSent = false;
        rb.isKinematic = true;
        collider.isTrigger = true;
    }

    private void Update() {
        RaycastHit2D platformRayHitCheck = Physics2D.Raycast(attachPoint.position, Vector2.up, 1f);
        Debug.DrawRay(attachPoint.position, Vector2.up);
        if(platformRayHitCheck.collider != null && platformRayHitCheck.collider.tag.Contains(PLATFORM_TAG)) {
            rb.isKinematic = true;
            eventSent = false;
            onGround = false;
        } else {
            if(!eventSent) {
                eventSent = true;
                GameEventSystem.RaiseGameEvent(GAME_EVENT.OBSTACLE_DESTROYED, transform);
                rb.isKinematic = false;
                collider.isTrigger = false;
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision) {
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
