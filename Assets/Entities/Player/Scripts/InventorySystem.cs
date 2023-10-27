using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    [Header("Inventory")]
    [SerializeField] private Transform hand;

    private List<GameObject> itemsInRange = new List<GameObject>();

    public int KeysCollected = 0;
    public int KeysRequired = 2;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI keysCollectedText;
    [SerializeField] private TextMeshProUGUI escapeText;

    void Start()
    {
        InputManager.Instance.onInteract += PickUpItem;
    }

    void PickUpItem()
    {
        if (itemsInRange.Count < 1) return;

        ItemScript.ItemType itemType = itemsInRange[0].GetComponent<ItemScript>().itemType;

        if (itemType == ItemScript.ItemType.Gun)
        {
            itemsInRange[0].GetComponent<Gun>().PutInHand(hand);
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
            UpdateObjectiveUI();
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



    private void UpdateObjectiveUI()
    {
        if (KeysCollected < KeysRequired)
        {
            keysCollectedText.text = "Find keys (" + KeysCollected + "/" + KeysRequired + ")";
        }
        else
        {
            keysCollectedText.text = "Find keys (" + KeysRequired + "/" + KeysRequired + ")";
            keysCollectedText.fontStyle |= FontStyles.Strikethrough;
            keysCollectedText.color = new Color(200, 200, 200);
            escapeText.gameObject.SetActive(true);
        }
    }
}
