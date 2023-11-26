using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum GameState { GS_PAUSEMENU, GS_GAME, GS_LEVELCOMPLETED, GS_GAME_OVER }
public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public GameState currentGameState = GameState.GS_PAUSEMENU;

    [SerializeField] private Image[] keysTab;
    [SerializeField] private Image[] lives;
    [SerializeField] private Timer timer;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text enemiesText;


    private int minutes;
    private int seconds;
    private int keysFound = 0;
    private int enemiesKilled = 0;

    [SerializeField] private Canvas inGameCanvas;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        for(int i = 0; i < keysTab.Length; i++)
        {
            keysTab[i].color = Color.grey;
        }
        timer.OnValueChanged().AddListener(OnTimerChange);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SwitchState();
        }
    }

    private void OnTimerChange()
    {
        minutes = (int)timer.TimePast() / 60;
        seconds = (int)timer.TimePast() % 60;
        //timerText.text = timer.TimePast().ToString();
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void UpdateEnemies()
    {
        enemiesKilled++;
        enemiesText.text = enemiesKilled.ToString();
    }

    public void AddKeys()
    {
        keysTab[keysFound++].color = Color.white;
    }

    public void EnableLives(int pLives)
    {
        for(int i = 0; i < lives.Length; i++)
        {
            if(i < pLives)
            {
                lives[i].enabled = true;
            }
            else
            {
                lives[i].enabled = false;
            }
        }
    }

    public void SetGameState(GameState state)
    {
        currentGameState = state;
    }

    public void PauseMenu()
    {
        timer.Stop();
        currentGameState = GameState.GS_PAUSEMENU;
        inGameCanvas.enabled = false;
    }

    public void InGame()
    {
        currentGameState = GameState.GS_GAME;
        inGameCanvas.enabled = true;
        timer.Continue();
    }
    public void LevelCompleted()
    {
        currentGameState = GameState.GS_LEVELCOMPLETED;
        inGameCanvas.enabled = false;
    }
    public void GameOver()
    {
        currentGameState = GameState.GS_GAME_OVER;
        inGameCanvas.enabled = false;
    }

    private void SwitchState()
    {
        if (currentGameState == GameState.GS_PAUSEMENU)
        {
            InGame();
        }
        else
        {
            PauseMenu();
        }
    }
}
