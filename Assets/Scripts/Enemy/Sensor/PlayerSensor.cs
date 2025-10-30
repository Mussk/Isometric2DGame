using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class PlayerSensor : MonoBehaviour
{
    public delegate void PlayerEnterEvent(Transform player);

    public delegate void PlayerExitEvent(Vector3 lastKnownPosition);

    public event PlayerEnterEvent OnPlayerEnter;

    public event PlayerExitEvent OnPlayerExit;

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            OnPlayerEnter?.Invoke(other.gameObject.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnPlayerExit?.Invoke(other.gameObject.transform.position);
        }
    }
}
