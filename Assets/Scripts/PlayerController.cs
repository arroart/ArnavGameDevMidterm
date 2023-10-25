using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    public int MaxHealth = 10;
    public int health;
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

    public GameObject respawnPoint;

    bool damageForce = false;

    public Vector2 knockDirection;

    int resetCounter;
    public GameObject rCount;
    TextMeshProUGUI rCounter;

    public GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        rCounter= rCount.GetComponent<TextMeshProUGUI>();
        health = MaxHealth;
        myBody = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        mySR = GetComponent<SpriteRenderer>();
        tr = GetComponent<TrailRenderer>();
        HealthBar.gameObject.GetComponent<HealthBar>().SetMaxHealth(MaxHealth);
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

        if ((Input.GetKeyDown(KeyCode.DownArrow)|| Input.GetKeyDown(KeyCode.S))&& !grounded)
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

        if (damageForce)
        {
            Debug.Log("aa");
            damageForce = false;
            Vector2 force = knockDirection * knockbackForce;
            outerBody.AddForce(force, ForceMode2D.Impulse);
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
                Respawn();
            }

            invincible = true;

            mySR.color = Color.red;

            //damageForce = true;
            //Vector2 force = direction * knockbackForce;
            //myBody.AddForce(force, ForceMode2D.Impulse);

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
            Respawn();
        }

        if (collision.gameObject.tag == "Respawn")
        {
            Debug.Log("1");
            if (respawnPoint != collision.gameObject)
            {
                Debug.Log("2");
                respawnPoint.GetComponent<RespawnPoint>().unsetRespawn();
                collision.GetComponent<RespawnPoint>().setRespawn();
            }
            
        }
    }

    public void Respawn()
    {
        if (respawnPoint != null)
        {
            resetCounter++;
            rCounter.text=resetCounter.ToString();
            if (health == 0)
            {
                health = MaxHealth;
            }
            
            HealthBar.gameObject.GetComponent<HealthBar>().SetHealth(health);
            mySR.color = Color.white;
            transform.position = respawnPoint.transform.position;
        }
    }
}
