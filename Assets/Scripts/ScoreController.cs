using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    [SerializeField]
    private int _score = 0;
    private static ScoreController instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public void SetScore(int score)
    {
        _score = score;
    }
    public int GetScore()
    {
        return _score;
    }

    public void IncreaseScore(int value)
    {
        _score += value;
    }

    public void DecreaseScore(int value)
    {
        _score -= value;
    }

    public static ScoreController GetController()
    {
        return instance;
    }
}
