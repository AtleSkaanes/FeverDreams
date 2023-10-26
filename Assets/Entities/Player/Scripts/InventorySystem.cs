using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    [SerializeField] private Transform hand;

    private List<GameObject> itemsInRange = new List<GameObject>();
    private PlayerInput input;

    void Start()
    {
        input = GetComponent<PlayerInput>();
        input.onInteract += PickUpItem;
    }

    void PickUpItem()
    {
        if (itemsInRange.Count < 1) return;

        ItemScript.ItemType itemType = itemsInRange[0].GetComponent<ItemScript>().itemType;

        if (itemType == ItemScript.ItemType.Gun)
        {
            itemsInRange[0].GetComponent<ItemScript>().PutInHand(hand);
        }

        else if (itemType == ItemScript.ItemType.Pill)
        {
            Debug.Log("InventorySystem.cs; Line 31; the function for this should be in the next pull");
            // SanityManager.Instance.HealSanity(15);
            Destroy(itemsInRange[0]);
        }

        itemsInRange.RemoveAt(0);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Item")
            itemsInRange.Add(other.gameObject);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Item")
            itemsInRange.Remove(other.gameObject);
    }
}
