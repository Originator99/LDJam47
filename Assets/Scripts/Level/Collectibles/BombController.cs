using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour {
    public GameObject destroyEffect;
    private float waitTimer = 2f;
    private float range = 3f;

    private void Update() {
        if(waitTimer > 0) {
            waitTimer -= Time.deltaTime;
            if(waitTimer < 0) {
                DoBlast();
            }
        }
    }

    private void DoBlast() {
        if(destroyEffect != null) {
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, range);
        if(colliders != null && colliders.Length > 0) {
            for(int i = 0; i < colliders.Length; i++) {
                if(colliders[i].CompareTag(GlobalConstants.platform_big_tag) || colliders[i].CompareTag(GlobalConstants.platform_one_way_tag)) {
                    GameEventSystem.RaiseGameEvent(GAME_EVENT.PLATFORM_DESTROYED, colliders[i].transform);
                } else if(colliders[i].CompareTag(GlobalConstants.player_tag)) {
                    GameEventSystem.RaiseGameEvent(GAME_EVENT.LEVEL_END, LEVEL_END_REASON.PLAYER_DEAD);
                }
            }
        }
        gameObject.SetActive(false);
        Destroy(gameObject, 1f);
    }
}
