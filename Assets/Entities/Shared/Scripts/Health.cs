using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float maxHealth;
    
    float health;
    public UnityAction onDeath;
    public UnityAction onTakeDamage;

    public void TakeDamage(float ammount)
    {
        health -= ammount;
        CheckHealth();
        onTakeDamage?.Invoke();
    }

    void CheckHealth()
    {
        if (health <= 0)
            onDeath?.Invoke();
    }
}
