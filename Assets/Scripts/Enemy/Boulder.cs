using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boulder : MonoBehaviour
{
    [SerializeField]
    float enemySpeed = 5f;

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        transform.Translate(Vector3.up * Time.deltaTime * enemySpeed);
    }
}
