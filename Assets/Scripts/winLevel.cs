using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class winLevel : MonoBehaviour
{
    public GameObject player;
    public string nextLevel;
    public GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            gm.TravelToNextLevel(nextLevel);
        }
    }


}
