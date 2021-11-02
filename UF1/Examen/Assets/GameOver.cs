using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public PlayerController playerController;

    private void Start()
    {
        playerController.onPlayerDeath += GameOver2;
    }

    void GameOver2()
    {
        SceneManager.LoadScene(2);
    }
}
