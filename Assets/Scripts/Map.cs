using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] private Transform initialPlayerPos;

    public Vector3 GetSpawnPosition()
    {
        return initialPlayerPos.position;
    }
}
