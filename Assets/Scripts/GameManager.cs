using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int maxLH = 10;
    public static int lightHealth=2;
    public GameObject lightBar;
    public GameObject lightCircle;

    public GameObject count;
    TextMeshProUGUI LHCounter;

    public GameObject portal;

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
            portal.SetActive(true);
            Destroy(lightCircle);
        }
    }

    private void Start()
    {
        lightHealth= maxLH;
        if (lightBar != null)
        {
            lightBar.gameObject.GetComponent<HealthBar>().SetMaxHealth(maxLH);
            lightBar.gameObject.GetComponent<HealthBar>().SetHealth(lightHealth);
        }

        LHCounter = count.GetComponent<TextMeshProUGUI>();
        LHCounter.text = lightHealth.ToString();
    }
}
