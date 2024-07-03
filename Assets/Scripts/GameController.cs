using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    WaitToStart,
    InGame,
    FinishGame
}
public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    private GameState currentGameState;
    public Action<GameState> OnGameStateChanged;

    public int currentGameScore;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ChangeState(GameState.WaitToStart);
    }

    public void ChangeState(GameState state)
    {
        currentGameState = state;
        if (state == GameState.WaitToStart)
        {
            ChangeToWaitToStartState();
        }
        else if (state == GameState.InGame)
        {
            ChangeToInGameState();
        }
        else if (state == GameState.FinishGame)
        {
            ChangeToFinishGameState();
        }
        OnGameStateChanged?.Invoke(currentGameState);
    }

    public bool IsInState(GameState state)
    {
        return currentGameState == state;
    }

    private void ChangeToWaitToStartState()
    {
        UIManager.Instance.OpenStartPanel();
    }

    private void ChangeToInGameState()
    {
        UIManager.Instance.OpenInGamePanel(LevelManager.Instance.GetMapIndex()+1);
    }

    private void ChangeToFinishGameState()
    {
        UIManager.Instance.OpenFinishPanel();
    }

    public void LoadNextLevel()
    {
        LevelManager.Instance.LoadNextMap();
        ChangeState(GameState.InGame);
    }

    public void RestartLevel()
    {
        LevelManager.Instance.LoadCurrentMap();
        ChangeState(GameState.InGame);
    }

    public void LoadCurrentLevelOfPlayer()
    {
        LevelManager.Instance.LoadMap(PlayerData.CurrentLevel);
        ChangeState(GameState.InGame);
    }
}
