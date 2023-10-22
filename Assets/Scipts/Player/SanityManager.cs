using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.Events;

public class SanityManager : Singleton<SanityManager>
{
    [Header("Settings")]
    [SerializeField] float maxSanity;
    [Tooltip("Sanity loss in units pr. second")]
    [SerializeField] float sanityDropRate;
    [Tooltip("How many sanity units between each stage")]
    [SerializeField] float sanityLevelIntervalSize;

    public UnityAction<SanityLevel> OnSanityChange;

    public float currentSanity { private set; get; }
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

    public void AttackSanity(float damage)
    {
        currentSanity -= damage;
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