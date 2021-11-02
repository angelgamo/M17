using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreGameOver : MonoBehaviour
{
    private int score;
    void Start()
    {
        score = PlayerPrefs.GetInt("Score");
        this.GetComponent<TMPro.TextMeshProUGUI>().text = score.ToString() + " Score";
    }
}
