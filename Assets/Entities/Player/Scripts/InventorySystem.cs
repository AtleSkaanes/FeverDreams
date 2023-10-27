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

    public int KeysCollected = 0;

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
            SanityManager.Instance.HealSanity(15);
            Destroy(itemsInRange[0]);
        }

        else if (itemType == ItemScript.ItemType.Key)
        {
            KeysCollected++;
            Destroy(itemsInRange[0]);
            Debug.Log("Keys collected: " + KeysCollected);
        }

        itemsInRange.RemoveAt(0);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Item" && !other.gameObject.GetComponent<ItemScript>().IsInHand)
            itemsInRange.Add(other.gameObject);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Item" && !other.gameObject.GetComponent<ItemScript>().IsInHand)
            itemsInRange.Remove(other.gameObject);
    }
}
