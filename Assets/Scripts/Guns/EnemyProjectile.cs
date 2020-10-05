using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour {
    public float speed;
    public float lifeTime;
    private void Update() {

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.left, 0.25f);
        if(hitInfo.collider != null) {
            if(hitInfo.collider != null && hitInfo.collider.tag == GlobalConstants.player_tag) {
                GameEventSystem.RaiseGameEvent(GAME_EVENT.LEVEL_END, LEVEL_END_REASON.PLAYER_DEAD);
            }
            //DestroyProjectile();
        }

        transform.Translate(Vector2.left * speed * Time.unscaledDeltaTime);
    }

    private void DestroyProjectile() {
        gameObject.SetActive(false);
    }
}
