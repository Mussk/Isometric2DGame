
using System;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    [Header("Prefab to Spawn")] [SerializeField]
    private GameObject prefab;

    [Header("Spawn Settings")] [SerializeField]
    private int initialPoolSize = 10;

    [SerializeField] public Transform parent; // optional, for hierarchy organization

    private ObjectPool _pool;

    private void Awake()
    {
        if (!prefab)
        {
            Debug.LogError($"{name}: No prefab assigned for spawning!");
            return;
        }


        _pool = new ObjectPool(prefab, initialPoolSize, parent);
    }

    private void Start()
    {
        TriggerInitialSpawn();
    }


    private void TriggerInitialSpawn()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            Spawn();
        }
    }

    

    public GameObject Spawn()
    {
        if (_pool == null) return null;

        GameObject obj = _pool.Get();
        
        return obj;
    }

    
    public void Despawn(GameObject obj)
    {
        _pool?.ReturnToPool(obj);
        
    }

    public GameObject DespawnAndRespawn(GameObject objToDespawn)
    {
        Despawn(objToDespawn);
        return Spawn();
    }
}
