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

    /// <summary>
    /// Get the current sanity loss in percent | 
    /// <returns> Max sanity: 0 | Min sanity: 1</returns>
    /// </summary>
    public float SanityLoss { get 
        {
            return (CurrentSanity * -1 + maxSanity) / maxSanity;
        }}
    public float CurrentSanity { private set; get; }
    SanityLevel currentSanityLevel;

    void Start()
    {
        CurrentSanity = maxSanity;
        currentSanityLevel = SanityLevel.Sane;
    }

    void Update()
    {
        CurrentSanity -= sanityDropRate * Time.deltaTime;

        if (CurrentSanity < ((float)currentSanityLevel - 1) * sanityLevelIntervalSize)
        {
            currentSanityLevel -= 1;
            OnSanityChange?.Invoke(currentSanityLevel);
        }
    }

    public void AttackSanity(float damage)
    {
        CurrentSanity -= damage;
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