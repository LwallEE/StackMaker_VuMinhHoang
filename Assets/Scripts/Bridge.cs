using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    [SerializeField] private GameObject emptyTileObj;

    [SerializeField] private GameObject fillTileObj;

    private bool hasFill;

    public bool CanFill()
    {
        return !hasFill;
    }

    public void PlayerFillBridge()
    {
        hasFill = true;
        emptyTileObj.SetActive(false);
        fillTileObj.SetActive(true);
    }
}
