using System;
using UnityEngine;
using UnityEngine.UI;

public class PickableItem : MonoBehaviour
{
    public ItemData itemData;            // reference to ScriptableObject
    [SerializeField] private GameObject tooltipUI;
    
    public static event Action<string> OnItemPicked;
    
    private void Start()
    {
        if (tooltipUI) tooltipUI.SetActive(false);
    }

    public void ShowTooltip(bool show)
    {
        if (tooltipUI) tooltipUI.SetActive(show);
    }

    public void OnPickUp()
    {
        Debug.Log($"Picked up {itemData.itemName}");
        OnItemPicked?.Invoke(itemData.itemName);
        Destroy(gameObject); 
    }
}
