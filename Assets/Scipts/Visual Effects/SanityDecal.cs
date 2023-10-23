using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SanityDecal : MonoBehaviour
{
    [Header("Visibility")]
    [Tooltip("Controls the sanity level for the decal to appear")]
    [SerializeField] SanityLevel sanityLevel;
    [Tooltip("Controls wethere the decal should appear below or above the sanity level\nNote: The decal always appears at the specified level")]
    [SerializeField] bool appearBelow;

    DecalProjector decal;
    void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        SanityManager.Instance.OnSanityChange += UpdateVisibility;
        decal = GetComponent<DecalProjector>();
    }

    void UpdateVisibility(SanityLevel newSanityLevel)
    {
        if (newSanityLevel == sanityLevel || (newSanityLevel < sanityLevel && appearBelow) || (newSanityLevel > sanityLevel && !appearBelow))
        {
            decal.fadeFactor = 1f;
            return;
        }

        decal.fadeFactor = 0f;
    }
}
