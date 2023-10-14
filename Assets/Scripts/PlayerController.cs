using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    float horizontalMove;
    public float speed = 2f;

    Rigidbody2D myBody;

    bool grounded = false;
    public bool smashing = false;

    public float castDist = 1f;

    public float jumpPower = 2f;
    public float gravityScale = 5f;
    public float gravityFall = 40f;

    public float smashPower = 2f;

    bool jump = false;

    Animator myAnim;
    SpriteRenderer mySR;

    public GameObject HealthBar;

    public int health = 10;
    public float knockbackForce = 100f;
    bool invincible = false;

    public int squashCount;

    public GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        mySR = GetComponent<SpriteRenderer>();
        HealthBar.gameObject.GetComponent<HealthBar>().SetMaxHealth(health);
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && grounded)
        {
            myAnim.SetBool("jumping", true);
            jump = true;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && !grounded)
        {
            smashing = true;
            myAnim.SetBool("smashing", true);
        }

        if (horizontalMove > 0.2f || horizontalMove < -0.2f)
        {
            myAnim.SetBool("running", true);
        }
        else
        {
            myAnim.SetBool("running", false);
        }
        if (horizontalMove > 0f)
        {

        }
    }

    private void FixedUpdate()
    {
        float moveSpeed = speed * horizontalMove;

        if (jump)
        {
            myBody.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            jump = false;
        }

        if (myBody.velocity.y > 0)
        {
            myBody.gravityScale = gravityScale;

        }else if (myBody.velocity.y < 0 && !smashing)
        {
            myBody.gravityScale = gravityFall;
        }

        if (smashing)
        {
            myBody.AddForce(Vector2.down * smashPower, ForceMode2D.Impulse);
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, castDist);
        Debug.DrawRay(transform.position, Vector2.down * castDist, Color.red);

        if(hit.collider!=null && hit.transform.tag == "Ground")
        {
            myAnim.SetBool("jumping", false);
            myAnim.SetBool("smashing", false);
            smashing = false;
            grounded = true;
        }
        else
        {
            grounded = false;

        }

        myBody.velocity = new Vector3(moveSpeed, myBody.velocity.y, 0f);
    }

    public void Damage(int damage, Vector2 direction)
    {
        if (!invincible)
        {
            health -= damage;
            Debug.Log(health);
            if (health<=0)
            {
                gm.ResetLevel();
            }

            invincible = true;

            mySR.color = Color.red;

            Vector2 force = direction * knockbackForce;
            myBody.AddForce(force, ForceMode2D.Impulse);

            HealthBar.gameObject.GetComponent<HealthBar>().SetHealth(health);

            Invoke("afterHit",1);
        }
       
    }
    void afterHit()
    {
        mySR.color = Color.white;
        invincible = false;
    }
}
