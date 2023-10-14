using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
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
}
