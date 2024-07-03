using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    [SerializeField] private List<Map> mapPrefab;
    private int currentMapIndex;
    private Map currentMap;
    private void Awake()
    {
        Instance = this;
    }

    public void LoadMap(int index)
    {
        if (currentMap != null)
        {
            Destroy(currentMap.gameObject);
        }

        currentMap = Instantiate(mapPrefab[index].gameObject).GetComponent<Map>();
        currentMapIndex = index;
    }

    public void LoadNextMap()
    {
        LoadMap((currentMapIndex+1)%mapPrefab.Count);
    }
}
