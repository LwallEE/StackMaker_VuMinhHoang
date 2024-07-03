using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGamePanel : MonoBehaviour
{
    public void StartGame()
    {
        GameController.Instance.LoadCurrentLevelOfPlayer();
    }
}
