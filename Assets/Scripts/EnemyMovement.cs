using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyMovement : MonoBehaviour
{
    public GameObject player;
    public GameObject pointA;
    public GameObject pointB;
    private Rigidbody2D rb;
    private Transform endPoint;
    public float speed = 5f;
    public int damage= 1;
    public bool movesHorizontal =true;
    bool playerSmashing = false;

    public GameObject count;
    TextMeshProUGUI squashCounter;

    SpriteRenderer mySR;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        endPoint = pointB.transform;
        mySR = GetComponent<SpriteRenderer>();
        squashCounter = count.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerSmashing = player.gameObject.GetComponent<PlayerController>().smashing;

        Vector2 point = endPoint.position - transform.position;
        if (!movesHorizontal)
        {
            if (endPoint == pointB.transform)
            {
                rb.velocity = new Vector2(0, speed);
            }
            else
            {
                rb.velocity = new Vector2(0, -speed);
            }
        }
        else
        {
            if (endPoint == pointB.transform)
            {
                rb.velocity = new Vector2(speed, 0);
            }
            else
            {
                rb.velocity = new Vector2(-speed, 0);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
    }
   private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.gameObject == pointB)
        {
            mySR.flipX = true;
            endPoint = pointA.transform;
        }
        if (collision.gameObject == pointA)
        {
            mySR.flipX = false;
            endPoint = pointB.transform;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == player)
        {
            if (playerSmashing)
            {
                player.GetComponent<PlayerController>().squashCount++;
                squashCounter.text = (player.GetComponent<PlayerController>().squashCount).ToString();
                Destroy(gameObject);
            }
            else
            {
                Vector2 direction = (collision.transform.position - transform.position).normalized;
                Debug.Log(direction);
                player.GetComponent<PlayerController>().Damage(damage, direction);
            }
            
        }
    }
}
