using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    public GameObject respawnPoint;
    public GameObject player;
    Animator anim;
    // Start is called before the first frame update
  

    void Start()
    {
        anim = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setRespawn()
    {
        Debug.Log("3");
        respawnPoint.GetComponent<SpriteRenderer>().color = Color.white;
        respawnPoint.GetComponent<TrailRenderer>().emitting = true;
        respawnPoint.GetComponent<ParticleSystem>().enableEmission = true;
        anim.SetBool("isActive", true);
        player.GetComponent<PlayerController>().respawnPoint = gameObject;

    }

    public void unsetRespawn()
    {
        respawnPoint.GetComponent<SpriteRenderer>().color = Color.gray;
        respawnPoint.GetComponent<TrailRenderer>().emitting = false;
        respawnPoint.GetComponent<ParticleSystem>().enableEmission = false;
        anim.SetBool("isActive", false);
    }
}
