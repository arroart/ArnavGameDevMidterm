using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    float horizontalMove;
    public float speed = 2f;

    Rigidbody2D myBody;
    public Rigidbody2D outerBody;

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
    public float knockbackForce = 5f;
    bool invincible = false;

    public int squashCount;

    bool canDash=true;
    bool isDashing=false;
    public float dashingPower = 24f;
    public float dashingTime = 0.2f;
    public float dashingCoolDown = 1f;
    TrailRenderer tr;

    public TrailRenderer smashTr;



    public GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        mySR = GetComponent<SpriteRenderer>();
        tr = GetComponent<TrailRenderer>();
        HealthBar.gameObject.GetComponent<HealthBar>().SetMaxHealth(health);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing)
        {
            return;
        }
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
            Vector2 localScale = transform.localScale;
           localScale.x = 1f;
            transform.localScale = localScale;
        }
        if (horizontalMove < 0f)
        {
            Vector2 localScale = transform.localScale;
            localScale.x = -1f;
            transform.localScale = localScale;
        }


        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
     
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
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
            smashTr.emitting = true;
            mySR.color = Color.red;
            myBody.AddForce(Vector2.down * smashPower, ForceMode2D.Impulse);
        }
      
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, castDist);
        Debug.DrawRay(transform.position, Vector2.down * castDist, Color.red);

        if(hit.collider!=null && hit.transform.tag == "Ground")
        {
            myAnim.SetBool("jumping", false);
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
            outerBody.AddForce(force, ForceMode2D.Impulse);

            HealthBar.gameObject.GetComponent<HealthBar>().SetHealth(health);

            Invoke("afterHit",1);
        }
       
    }
    void afterHit() { 
    
        mySR.color = Color.white;
        invincible = false;
    }

    private IEnumerator Dash()
    {
        Debug.Log("dashing");
        canDash = false;
        isDashing = true;
        float originalGravity = myBody.gravityScale;
        myBody.gravityScale = 0f;
        myBody.velocity = new Vector2(transform.localScale.x* dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        myBody.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCoolDown);
        canDash = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(smashing&& collision.transform.tag == "Ground")
        {
            myAnim.SetBool("smashing", false);
            smashing = false;
            smashTr.emitting = false;
            mySR.color = Color.white;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("hi");
        if (collision.gameObject.tag == "FollowLight")
        {
            gm.ResetLevel();
        } 
    }
}
