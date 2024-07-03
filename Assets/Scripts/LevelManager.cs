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
        PlayerData.CurrentLevel = index;
    }

    public int GetMapIndex()
    {
        return currentMapIndex;
    }
    public void LoadNextMap()
    {
        LoadMap((currentMapIndex+1)%mapPrefab.Count);
    }

    public void LoadCurrentMap()
    {
        LoadMap(currentMapIndex);
    }

    public Vector3 GetSpawnPositionOfPlayer()
    {
        if(currentMap != null)
            return currentMap.GetSpawnPosition();
        return Vector3.zero;
    }
}
