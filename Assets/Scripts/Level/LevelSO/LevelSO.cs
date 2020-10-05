using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Level/New Level Data", order = 1)]
public class LevelSO : ScriptableObject {
    public SceneName ID;
    public int levelTimer;
    public int currentTimer;
    public Sprite portalSprite;
}
