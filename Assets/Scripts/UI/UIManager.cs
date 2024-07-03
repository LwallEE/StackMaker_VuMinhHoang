using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    [SerializeField] private GameObject startGamePanel;

    [SerializeField] private InGamePanel inGamePanel;

    [SerializeField] private FinishGamePanel finishGamePanel;

    private void Awake()
    {
        Instance = this;
    }

    public void CloseAll()
    {
        startGamePanel.SetActive(false);
        inGamePanel.gameObject.SetActive(false);
        finishGamePanel.gameObject.SetActive(false);
    }

    public void OpenStartPanel()
    {
        CloseAll();
        startGamePanel.gameObject.SetActive(true);
    }

    public void OpenInGamePanel(int level)
    {
        CloseAll();
        inGamePanel.gameObject.SetActive(true);
        inGamePanel.UpdateVisual(level);
    }

    public void OpenFinishPanel()
    {
        CloseAll();
        finishGamePanel.gameObject.SetActive(true);
        finishGamePanel.UpdateVisual(GameController.Instance.currentGameScore);
    }
}
