using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class ItemScript : MonoBehaviour
{
    public string ItemName = "ITEM NAME";
    [HideInInspector] public bool IsInHand = false;
    public Vector3 LayingRotation = new(0, 0, 90);
    public ItemType itemType;


    protected void Start()
    {
        PutOnGround();
    }

    private void OnValidate()
    {
        PutOnGround();
    }

    public void PutInHand()
    {
        IsInHand = true;
        transform.eulerAngles = new Vector3(0, 180, 0);
    }

    public void PutInHand(Transform hand)
    {
        IsInHand = true;
        transform.SetParent(hand);
        transform.localPosition = Vector3.zero;
        transform.eulerAngles = new Vector3(0, 180, 0);
    }

    public void PutOnGround()
    {
        IsInHand = false;
        transform.eulerAngles = LayingRotation;
    }

    public enum ItemType
    {
        Gun,
        Pill,
        Key,
        Exit
    }
}
