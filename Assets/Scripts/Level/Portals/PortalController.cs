using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour {
    public List<Portal> portals;

    private void Awake() {
        GameEventSystem.GameEventHandler += HandleGameEvents;
    }
    private void OnDestroy() {
        GameEventSystem.GameEventHandler -= HandleGameEvents;
    }

    private void HandleGameEvents(GAME_EVENT type, System.Object data = null) {
        if(type == GAME_EVENT.LEVEL_START) {
            OnLevelStart();
        }
    }

    private void OnLevelStart() {
        RenderPortals();
    }

    private void RenderPortals() {
        if(portals != null) {
            List<LevelSO> data = LevelController.instance.levelData;
            LevelSO current = LevelController.instance.currentLevelSO;
            if(data != null && portals != null) {
                int index = 0;
                for(int i = 0; i < data.Count; i++) {
                    if(portals.Count > index) {
                        if(current.ID != data[i].ID) {
                            portals[index].gameObject.SetActive(true);
                            portals[index].RenderPortal(data[i]);
                            index++;
                        }
                    }
                }
                for(int i = index; i < portals.Count; i++) {
                    portals[i].gameObject.SetActive(false);
                }
            }
        }     
    }
}
