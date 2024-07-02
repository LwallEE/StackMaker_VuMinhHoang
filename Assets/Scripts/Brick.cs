using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField] private GameObject brick;
    private bool hasAddToPlayer;

    public bool CanGetBrick()
    {
        return !hasAddToPlayer;
    }
    public void PlayerGetBrick()
    {
        hasAddToPlayer = true;
        brick.gameObject.SetActive(false);
    }
}
