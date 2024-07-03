using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazyPool : MonoBehaviour
{
    public static LazyPool Instance { get; private set; }
    private Dictionary<GameObject, List<GameObject>> _poolObjt = new Dictionary<GameObject, List<GameObject>>();

    private void Awake()
    {
        Instance = this;
    }

    public GameObject GetObj(GameObject objKey)
    {
        //if pool doesn't have type of object yet
        if (!_poolObjt.ContainsKey(objKey))
        {
            _poolObjt.Add(objKey, new List<GameObject>());
        }
        
        //if pool has type of object and object available
        //return object
        foreach (GameObject g in _poolObjt[objKey])
        {
            if (g.activeInHierarchy)
                continue;
            g.SetActive(true);
            return g;
        }
        //if doesn't have available object, create new one
        GameObject g2 = Instantiate(objKey, transform);
        _poolObjt[objKey].Add(g2);
        return g2;
        
    }

    public T GetObj<T>(GameObject objKey) where T : Component
    {
        return this.GetObj(objKey).GetComponent<T>();
    }

    public void AddObjectToPool(GameObject obj)
    {
        obj.transform.SetParent(transform); 
        obj.SetActive(false);
    }
}
