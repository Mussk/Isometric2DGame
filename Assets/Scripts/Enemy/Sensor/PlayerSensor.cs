
using UnityEngine;

namespace Enemy.Sensor
{
[RequireComponent(typeof(CircleCollider2D))]
public class PlayerSensor : MonoBehaviour
{
    public delegate void PlayerEnterEvent(Transform player);

    public delegate void PlayerExitEvent(Vector2 lastKnownPosition);

    public event PlayerEnterEvent OnPlayerEnter;

    public event PlayerExitEvent OnPlayerExit;
    
    [SerializeField]
    private string playerTag = "Player";

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag(playerTag))
        {
            
            OnPlayerEnter?.Invoke(other.gameObject.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            OnPlayerExit?.Invoke(other.gameObject.transform.position);
        }
    }
}

}