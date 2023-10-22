using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int maxLH = 10;
    public static int lightHealth=10;
    public GameObject lightBar;
    public GameObject lightCircle;

    public GameObject count;
    TextMeshProUGUI LHCounter;

    public void LoadGame()
    {
        SceneManager.LoadScene("Game");
    }
    public void TravelToNextLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void changeLightHealth()
    {
        lightHealth--;
        lightBar.gameObject.GetComponent<HealthBar>().SetHealth(lightHealth);
        LHCounter.text = lightHealth.ToString();

        if (lightHealth <= 0)
        {
            Destroy(lightCircle);
        }
    }

    private void Start()
    {
        lightBar.gameObject.GetComponent<HealthBar>().SetMaxHealth(maxLH);
        lightBar.gameObject.GetComponent<HealthBar>().SetHealth(lightHealth);
        LHCounter = count.GetComponent<TextMeshProUGUI>();
        LHCounter.text = lightHealth.ToString();
    }
}
