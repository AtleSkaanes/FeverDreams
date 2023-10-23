using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SanityDecal : MonoBehaviour
{
    [Header("Visibility")]
    [Tooltip("Controls the minimum sanity level for the decal to appear\n Decal will apear when the sanity is lower than the level chosen")]
    [SerializeField] SanityLevel sanityLevel;

    DecalProjector decal;

    // Start is called before the first frame update
    void Start()
    {
        SanityManager.Instance.OnSanityChange += UpdateVisibility;
        decal = GetComponent<DecalProjector>();
    }

    void UpdateVisibility(SanityLevel newSanityLevel)
    {
        if (newSanityLevel <= sanityLevel)
        {
            decal.fadeFactor = 1f;
            return;
        }

        decal.fadeFactor = 0f;
    }
}
