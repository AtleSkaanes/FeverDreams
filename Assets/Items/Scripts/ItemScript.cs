using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    public string itemName = "ITEM NAME";
    public bool isInHand = false;
    public Vector3 layingRotation = new Vector3(0, 0, 90);

    private void Start()
    {
        PutOnGround();
    }

    private void OnValidate()
    {
        PutOnGround();
    }

    public void PutInHand()
    {
        isInHand = true;
        transform.eulerAngles = new Vector3(0, 0, 0);
    }

    public void PutOnGround()
    {
        isInHand = false;
        transform.eulerAngles = layingRotation;
    }

    
}
