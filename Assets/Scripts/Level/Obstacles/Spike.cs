using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour {
    public Rigidbody2D rb;
    public Transform attachPoint;
    public LayerMask groundLayerMask;

    private const string PLATFORM_TAG = "destructible";
    private bool eventSent;

    private void Start() {
        eventSent = false;
        rb.isKinematic = true;
    }

    private void Update() {
        RaycastHit2D platformRayHitCheck = Physics2D.Raycast(attachPoint.position, Vector2.up, 1f);
        Debug.DrawRay(attachPoint.position, Vector2.up);
        if(platformRayHitCheck.collider != null && platformRayHitCheck.collider.tag.Contains(PLATFORM_TAG)) {
            rb.isKinematic = true;
            eventSent = false;
        } else {
            if(!eventSent) {
                eventSent = true;
                GameEventSystem.RaiseGameEvent(GAME_EVENT.OBSTACLE_DESTROYED, transform);
                rb.isKinematic = false;
            }
            platformRayHitCheck = Physics2D.Raycast(attachPoint.position, Vector2.down, 1f, groundLayerMask);
            if(platformRayHitCheck.collider != null) {
                OnSpikeTouchedTheGround();
            }
        }
    }

    private void OnSpikeTouchedTheGround() {

    }

    private void OnCollisionEnter2D(Collision2D collision) {

    }

}
