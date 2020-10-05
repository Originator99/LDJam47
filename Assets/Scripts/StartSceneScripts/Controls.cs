using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controls : MonoBehaviour {
    public Button close;
    public void Show() {
        close.onClick.RemoveAllListeners();
        close.onClick.AddListener(delegate() {
            Hide();
        });
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }
}
