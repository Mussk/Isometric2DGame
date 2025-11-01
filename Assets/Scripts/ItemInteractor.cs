using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemInteractor : MonoBehaviour
{
    [SerializeField] private float interactRange = 1f;
    private PickableItem currentItem;

    private InputSystem_Actions controls;
    

    private void Awake()
    {
        controls = GetComponent<PlayerController>().InputActions;
    }

    private void OnEnable()
    {
        
        controls.Player.Interact.started += OnInteract;
    }

    private void OnDisable()
    {
        controls.Player.Interact.started -= OnInteract;
        
    }
    
    private void Update()
    {
        FindNearestItem();
    }

    private void FindNearestItem()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactRange, LayerMask.GetMask("Pickable"));
        PickableItem nearest = null;
        float nearestDist = Mathf.Infinity;

        foreach (var hit in hits)
        {
            PickableItem item = hit.GetComponent<PickableItem>();
            if (item)
            {
                float dist = Vector2.Distance(transform.position, item.transform.position);
                if (dist < nearestDist)
                {
                    nearestDist = dist;
                    nearest = item;
                }
            }
        }

        if (currentItem != nearest)
        {
            if (currentItem) currentItem.ShowTooltip(false);
            currentItem = nearest;
            if (currentItem) currentItem.ShowTooltip(true);
        }
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        Debug.Log("Interact");
        if (currentItem != null)
        {   
            currentItem.OnPickUp();
            currentItem = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}