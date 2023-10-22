using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

public class Sanity : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float maxSanity;
    [Description("Sanity loss in units pr. second")]
    [SerializeField] float sanityDropRate;
    [Description("How many sanity units between each stage")]
    [SerializeField] float sanityLevelIntervalSize;

    public UnityAction<SanityLevel> OnSanityChange;

    float currentSanity;
    SanityLevel currentSanityLevel;


    void Start()
    {
        currentSanity = maxSanity;
        currentSanityLevel = SanityLevel.Sane;
    }

    void Update()
    {
        currentSanity -= sanityDropRate * Time.deltaTime;

        if (currentSanity < ((float)currentSanityLevel - 1) * sanityLevelIntervalSize)
        {
            currentSanityLevel -= 1;
            OnSanityChange?.Invoke(currentSanityLevel);
        }
    }
}

public enum SanityLevel
{
    Dead,
    Insane,
    Shaking,
    Scared,
    Sane,
}