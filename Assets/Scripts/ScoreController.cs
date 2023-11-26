using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreController : MonoBehaviour
{
    [SerializeField]
    private int score = 0;
    [SerializeField]
    private TMP_Text scoreText;
    private static ScoreController instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public void SetScore(int value)
    {
        score = value;
        scoreText.text = score.ToString();
    }
    public int GetScore()
    {
        return score;
    }

    public void IncreaseScore(int value)
    {
        score += value;
        scoreText.text = score.ToString();
    }

    public void DecreaseScore(int value)
    {
        score -= value;
        scoreText.text = score.ToString();
    }

    public static ScoreController GetController()
    {
        return instance;
    }
}
