using UnityEngine;
using UnityEngine.AI;

public class Utility
{
    public static Vector3 GetRandomNavMeshPosition(Vector3 center, float maxDistance)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * maxDistance;
        return NavMesh.SamplePosition(randomPoint, out var hit, maxDistance, NavMesh.AllAreas) ? hit.position : Vector3.zero;
    }
}
