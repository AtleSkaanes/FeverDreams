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
    public float SanityLoss => (CurrentSanity * -1 + maxSanity) / maxSanity;

    public float CurrentSanity { private set; get; }
    SanityLevel currentSanityLevel;

    void Start()
    {
        CurrentSanity = maxSanity;
        currentSanityLevel = SanityLevel.Sane;
        StartCoroutine(LateStart(0.01f));
    }

    // This is gross but unity doesn't have in built LateStart script :(
    // The OnSanityChange will get called before sanityClutter script is ready
    IEnumerator LateStart(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        OnSanityChange?.Invoke(currentSanityLevel);
    }

    void Update()
    {
        CurrentSanity -= sanityDropRate * Time.deltaTime;

        // Sanity lower than level below
        if (CurrentSanity <= ((int)currentSanityLevel - 1) * sanityLevelIntervalSize)
        {
            currentSanityLevel -= 1;
            OnSanityChange?.Invoke(currentSanityLevel);
        }
        // Sanity higher than level above
        else if (CurrentSanity >= ((int)currentSanityLevel + 1) * sanityLevelIntervalSize)
        {
            currentSanityLevel += 1;
            OnSanityChange?.Invoke(currentSanityLevel);
        }
    }

    public void AttackSanity(float damage)
    {
        CurrentSanity -= damage;
        CurrentSanity = Mathf.Max(CurrentSanity, 0);
    }

    public void HealSanity(float healing)
    {
        CurrentSanity += healing;
        CurrentSanity = Mathf.Min(CurrentSanity, maxSanity);
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
