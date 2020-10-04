using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boulder : MonoBehaviour
{
    [SerializeField]
    float enemySpeed = 5f;

    public Object boulderRef { get; private set; }

    Rigidbody2D rigidbody2D;

    Vector2 startPos;

    [SerializeField]
    float respawnTime = 3f;

    private void Start()
    {
        boulderRef = Resources.Load("Boulder");

        rigidbody2D = GetComponent<Rigidbody2D>();
        startPos = transform.position;
        //transform.Translate(Vector3.left * Time.deltaTime * enemySpeed);
        rigidbody2D.AddForce(transform.right *enemySpeed, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        if (rigidbody2D.velocity.magnitude==0f && gameObject.activeInHierarchy )
        {
            gameObject.SetActive(false);
            Invoke("Respawn", respawnTime);
        }
    }

    void Respawn()
    {
        GameObject boulderClone = (GameObject)Instantiate(boulderRef);
        boulderClone.transform.position = startPos;
        Destroy(this);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        gameObject.SetActive(false);
        Invoke("Respawn", respawnTime);
    }
}
