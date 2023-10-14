using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthbar;
    // Start is called before the first frame update
    void Start()
    {
        healthbar = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetMaxHealth(int health)
    {
        healthbar.maxValue = health;
        healthbar.value = health;
    }
    public void SetHealth(int health)
    {
        healthbar.value = health;
    }
}
