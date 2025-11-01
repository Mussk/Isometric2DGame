
using System;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    [Header("Prefab to Spawn")] [SerializeField]
    protected GameObject prefab;

    [Header("Spawn Settings")] [SerializeField]
    protected int initialPoolSize = 10;

    [SerializeField] public Transform parent; // optional, for hierarchy organization

    protected ObjectPool _pool;

    protected virtual void Awake()
    {
        if (!prefab)
        {
            Debug.LogError($"{name}: No prefab assigned for spawning!");
            return;
        }


        _pool = new ObjectPool(prefab, initialPoolSize, parent);
    }

    protected virtual void Start()
    {
        TriggerInitialSpawn();
    }


    protected virtual void TriggerInitialSpawn()
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
