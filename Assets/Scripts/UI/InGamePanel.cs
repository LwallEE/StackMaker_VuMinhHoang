using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGamePanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lvTxt;

    public void UpdateVisual(int level)
    {
        lvTxt.text = "LV " + level;
    }
}
