using UnityEngine;
using System.Collections.Generic;

public class EnemyPool : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string name;                // Just for debugging
        public GameObject prefab;
        public int size = 10;

        public Transform targetTransform;

        public List<GameObject> objects;
    }

    [Header("Enemy Pools")]
    [SerializeField] private Pool[] pools;

    private void Awake()
    {
        InitializePools();
    }

    private void InitializePools()
    {
        foreach (var pool in pools)
        {
            pool.objects = new List<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                //obj.GetComponent<EnemyController>().DisableAgent();
                pool.objects.Add(obj);
            }
        }
    }

    public GameObject GetEnemy(int poolIndex)
    {
        Pool pool = pools[poolIndex];

        foreach (var obj in pool.objects)
        {
            if (!obj.activeInHierarchy)
                return obj;
        }

        GameObject newObj = Instantiate(pool.prefab);
        newObj.SetActive(false);
        pool.objects.Add(newObj);
        return newObj;
    }

    public void SpawnEnemy(int poolIndex, Vector3 position)
    {
        GameObject enemy = GetEnemy(poolIndex);
        enemy.GetComponent<EnemyController>().SetTarget(pools[poolIndex].targetTransform);
        enemy.transform.position = position;
        enemy.SetActive(true);
    }
}