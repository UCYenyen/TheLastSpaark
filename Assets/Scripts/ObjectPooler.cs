using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler instance;
    public List<GameObject> pooledObjects;
    public Transform poolParent;
    public GameObject objectToPool;
    public int amountToPool;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = Instantiate(objectToPool, poolParent);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }

    public GameObject SpawnPooledObject(Vector3 position, Quaternion rotation)
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                pooledObjects[i].transform.position = position;
                pooledObjects[i].transform.rotation = rotation;
                pooledObjects[i].SetActive(true);
                return pooledObjects[i];
            }
        }

        GameObject obj = Instantiate(objectToPool);
        obj.SetActive(false);
        pooledObjects.Add(obj);
        return obj;
    }

    public void ClearPool()
    {
        foreach (GameObject obj in pooledObjects)
        {
            Destroy(obj);
        }
        pooledObjects.Clear();
    }

    public void ResetPool()
    {
        foreach (GameObject obj in pooledObjects)
        {
            obj.SetActive(false);
        }
    }
}
