using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SanityClutter : MonoBehaviour
{
    [Header("Visibility")]
    [Tooltip("Controls the sanity level for the object to appear")]
    [SerializeField] SanityLevel sanityLevel;
    [Tooltip("Controls wethere the object should appear below or above the sanity level\nNote: The object always appears at the specified level")]
    [SerializeField] bool appearBelow;

    Renderer meshRenderer;
    Collider meshCollider;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        SanityManager.Instance.OnSanityChange += UpdateVisibility;
        meshRenderer = GetComponent<Renderer>();
        meshCollider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
    }

    void UpdateVisibility(SanityLevel newSanityLevel)
    {
        if (newSanityLevel == sanityLevel || (newSanityLevel < sanityLevel && appearBelow) || (newSanityLevel > sanityLevel && !appearBelow))
        {
            meshRenderer.enabled = true;
            meshCollider.enabled = true;
            rb.isKinematic = false;
            return;
        }

        meshRenderer.enabled = false;
        meshCollider.enabled = false;
        rb.isKinematic = true;

    }
}
