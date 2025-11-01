using UnityEngine;

public interface IPoolable
{
    /// <summary>
    /// Called when the object is spawned/reused from the pool.
    /// Reset state here (health, position, velocity, etc.)
    /// </summary>
    void OnReuse();
    
}

