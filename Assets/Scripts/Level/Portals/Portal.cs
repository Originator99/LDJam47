using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Portal : MonoBehaviour {
    private SceneName portalID;
    public void RenderPortal(LevelSO data) {
        if(data != null) {
            portalID = data.ID;
            GetComponent<SpriteRenderer>().sprite = data.portalSprite;
            gameObject.name = data.ID.ToString();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag(GlobalConstants.player_tag)) {
            LevelController.instance.SwitchLevel(portalID);
        }
    }

}
