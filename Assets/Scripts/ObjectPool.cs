using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private readonly GameObject _prefab;
    private readonly Queue<GameObject> _pool = new Queue<GameObject>();
    private readonly Transform _parent;

    public ObjectPool(GameObject prefab, int initialSize = 10, Transform parent = null)
    {
        this._prefab = prefab;
        this._parent = parent;

        for (int i = 0; i < initialSize; i++)
        {
            GameObject obj = GameObject.Instantiate(prefab, parent);
            obj.SetActive(false);
            _pool.Enqueue(obj);
        }
    }

    public GameObject Get()
    {
        GameObject obj;
        if (_pool.Count > 0)
            obj = _pool.Dequeue();
        else
            return null;

        obj.SetActive(true);

        // Call OnReuse if any component on the prefab implements IPoolable
        foreach (var poolable in obj.GetComponents<IPoolable>())
        {
            poolable.OnReuse();
        }

        return obj;
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        _pool.Enqueue(obj);
    }
}