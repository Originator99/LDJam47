using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrinterBullet : MonoBehaviour
{
    [SerializeField]
    GameObject explosion;

    Rigidbody2D rigidbody;

    private void OnEnable()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        Invoke("explode", 2f);
        if (transform.childCount == 0)
        {
            //Do nothing
        }
        else if (transform.GetChild(0).gameObject.activeInHierarchy)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }

        StartCoroutine(Rotating());

    }

    float roateSpeed = 10f;

    IEnumerator Rotating()
    {
        while (true)
        {
            transform.RotateAround(transform.position, transform.forward, roateSpeed* Time.deltaTime * 90f);
            yield return null;
        }
    }

    void hideBullet()
    {
        gameObject.SetActive(false);
    }

    void explode()
    {
        if (rigidbody.velocity.magnitude>0)
        {
            rigidbody.velocity = Vector2.zero;
        }
        if (transform.childCount == 0)
        {
            Instantiate(explosion, transform.position, Quaternion.identity, transform);
        }
        if (!transform.GetChild(0).gameObject.activeInHierarchy)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
        GetComponent<SpriteRenderer>().enabled = false;
        Invoke("hideBullet", 0.25f);
    }

    private void OnDisable()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        CancelInvoke();
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.collider.CompareTag("Player"))
    //    {
    //        //collision.collider.GetComponent<PlayerController>().health--;

    //        explode();
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //collision.GetComponent<PlayerController>().health--;
            //collision.GetComponent<PlayerController>().PlayDamageAUdio();
            //Handheld.Vibrate();
            explode();
        }
        else if (collision.CompareTag("Wall"))
        {
            explode();
        }
    }
}
