using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickHealth : MonoBehaviour
{
    public GameObject lightCircle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = lightCircle.transform.position;
    }
}
