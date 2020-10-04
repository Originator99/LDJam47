using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController2D;

public class PrinterAI : MonoBehaviour
{
    int health = 1;

    [SerializeField]
    float enemySpeed = 5f;

    [SerializeField]
    bool moveRight = true;

    [SerializeField]
    LayerMask layerMask;

    [SerializeField]
    LayerMask playerLayerMask;

    bool sittingIdle = false;

    [SerializeField]
    GameObject bulletPrefab;

    List<GameObject> bulletsInScene;

    [SerializeField]
    float bulletSpeed = 10.0f;

    GameObject player;

    bool startFiring = false;

    public Vector3 Velocity;
    public Rigidbody2D rb2d;
    public bool IsAlive = true;
    private double _decelerationTolerance = 12.0;


    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();

        Physics2D.queriesStartInColliders = false;
        bulletsInScene = new List<GameObject>();

        for (int i = 0; i < 10; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.SetActive(false);
            bulletsInScene.Add(bullet);
        }

        StartCoroutine(MoveRight());
        StartCoroutine(DetectPlayer());
        StartCoroutine(DetectGround());

        StartCoroutine(StartFiring());
    }

    void FixedUpdate()
    {
        if (IsAlive)
        {
            IsAlive = Vector3.Distance(rb2d.velocity, Velocity) < _decelerationTolerance;
            Velocity = rb2d.velocity;
        }
    }

    int count = 3;
    int bulletCount = 1;
    IEnumerator StartFiring()
    {
        while (true)
        {
            if (startFiring && count%3==0)
            {
                if (bulletCount<3)
                {
                    GetComponent<Animator>().SetBool("Reload", false);
                    Fire();
                    bulletCount++;
                    yield return new WaitForSeconds(1.5f);
                }
                else
                {
                    GetComponent<Animator>().SetBool("Reload", false);
                    yield return new WaitForSeconds(1.5f);
                    Fire();
                    count++;
                    bulletCount = 1;
                }
            }
            else if (startFiring && count%3!=0)
            {
                yield return new WaitForSeconds(1.0f);
                GetComponent<Animator>().SetBool("Reload", true);
                count++;
            }
            else
            {
                yield return null;
            }
        }
    }

    void Fire()
    {
        for (int i = 0; i < bulletsInScene.Count; i++)
        {
            if (!bulletsInScene[i].activeInHierarchy)
            {
                GetComponent<AudioSource>().Play();
                bulletsInScene[i].transform.position = transform.position;
                bulletsInScene[i].transform.rotation = transform.rotation;
                bulletsInScene[i].SetActive(true);
                Rigidbody2D tempRigidbody2D = bulletsInScene[i].GetComponent<Rigidbody2D>();
                if (moveRight)
                {
                    tempRigidbody2D.AddForce(/*(transform.position - player.transform.position).normalized*/transform.right * -bulletSpeed, ForceMode2D.Impulse);
                }
                else
                {
                    tempRigidbody2D.AddForce(/*(player.transform.position - transform.position).normalized*/transform.right * -bulletSpeed, ForceMode2D.Impulse);
                }
                break;
            }
        }
    }

    IEnumerator MoveRight()
    {
        while (true)
        {
            if (!sittingIdle)
            {
                transform.Translate(Vector3.left * Time.deltaTime * enemySpeed);

                RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.right * -1, 1, layerMask);
                if (hitInfo.collider != null)
                {
                    Debug.DrawLine(transform.position, hitInfo.point, Color.red);
                    if (moveRight == true)
                    {
                        transform.eulerAngles = new Vector3(0, 0, 0);
                        moveRight = false;
                    }
                    else
                    {
                        transform.eulerAngles = new Vector3(0, -180, 0);
                        moveRight = true;
                    }
                }
                else
                {
                    Debug.DrawLine(transform.position, transform.position + transform.right * -1, Color.green);

                }
            }

            yield return null;
        }
    }

    float rotateRay = 30;

    IEnumerator DetectPlayer()
    {
        while (true)
        {
            RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.right * -1, 10, playerLayerMask);
            if (hitInfo.collider != null)  //error
            {
                if (hitInfo.collider.CompareTag("Wall"))
                {
                    PlayerNotDetected();
                }
                else if (hitInfo.collider.CompareTag("Player"))
                {
                    Debug.DrawLine(transform.position, hitInfo.point, Color.yellow);
                    if (!sittingIdle)
                    {
                        GetComponent<Animator>().SetBool("Idle", true);
                        sittingIdle = true;
                    }
                    player = hitInfo.collider.gameObject;
                    startFiring = true;
                }
                else if (hitInfo.collider.CompareTag("DiscoBall"))
                {
                    Debug.DrawLine(transform.position, hitInfo.point, Color.yellow);
                    if (!sittingIdle)
                    {
                        GetComponent<Animator>().SetBool("Idle", true);
                        sittingIdle = true;
                    }
                    player = hitInfo.collider.gameObject;
                    startFiring = true;
                }
            }
            else
            {
                PlayerNotDetected();
            }

            yield return null;
        }
    }

    IEnumerator DetectGround()
    {
        while (true)
        {
            RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.right * -1, 10, playerLayerMask);
            if (hitInfo.collider != null)  //error
            {
                if (hitInfo.collider.CompareTag("Wall"))
                {
                    PlayerNotDetected();
                }
                else if (hitInfo.collider.CompareTag("Player"))
                {
                    Debug.DrawLine(transform.position, hitInfo.point, Color.yellow);
                    if (!sittingIdle)
                    {
                        GetComponent<Animator>().SetBool("Idle", true);
                        sittingIdle = true;
                    }
                    player = hitInfo.collider.gameObject;
                    startFiring = true;
                }
                else if (hitInfo.collider.CompareTag("DiscoBall"))
                {
                    Debug.DrawLine(transform.position, hitInfo.point, Color.yellow);
                    if (!sittingIdle)
                    {
                        GetComponent<Animator>().SetBool("Idle", true);
                        sittingIdle = true;
                    }
                    player = hitInfo.collider.gameObject;
                    startFiring = true;
                }
            }
            else
            {
                PlayerNotDetected();
            }

            yield return null;
        }
    }

    private void PlayerNotDetected()
    {
        float x = Mathf.Cos(Mathf.Deg2Rad * 30);
        float y = Mathf.Sin(Mathf.Deg2Rad * 30);
        if (!moveRight)
        {
            Debug.DrawLine(transform.position, transform.position + transform.right + new Vector3(x, y, 0) * -10, Color.green);
            Debug.DrawLine(transform.position, transform.position + transform.right + new Vector3(x, -y, 0) * -10, Color.green);
        }
        else
        {
            Debug.DrawLine(transform.position, transform.position + transform.right + new Vector3(-x, -y, 0) * -10, Color.green);
            Debug.DrawLine(transform.position, transform.position + transform.right + new Vector3(-x, y, 0) * -10, Color.green);
        }
        enemySpeed = 3f;
        GetComponent<Animator>().SetBool("Idle", false);
        sittingIdle = false;
        startFiring = false;
    }
}
