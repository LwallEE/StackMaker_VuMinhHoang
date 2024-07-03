using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FinishGamePanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreTxt;
    public void NextLevel()
    {
        GameController.Instance.LoadNextLevel();
    }

    public void Restart()
    {
        GameController.Instance.RestartLevel();
    }

    public void UpdateVisual(int score)
    {
        scoreTxt.text = "STACK COLLECT : " + score;
    }
}
