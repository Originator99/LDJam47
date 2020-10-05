using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollectable {
    void UseCollectable();
    void CheckAndSetCollectable();
}

public enum CollectableType {
    FREEZE_TIME,
    BOMB
}