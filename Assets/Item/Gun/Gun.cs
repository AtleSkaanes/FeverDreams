using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Gun : ItemScript
{
    [SerializeField] float attackSpeed;
    [SerializeField] int maxAmmo;
    [SerializeField] int startAmmo;
    [SerializeField] int extraAmmo;

    int ammo;
    float cooldown = 0;

    void Awake()
    {
        ammo = startAmmo;
    }

    new void Start()
    {
        base.Start();
    }

    private void OnEnable()
    {
        InputManager.Instance.onUsePrimary += Shoot;
    }

    private void Update()
    {
        cooldown -= Time.deltaTime;
        cooldown = Mathf.Max(cooldown, 0);
    }

    public void Shoot(Ray bulletPath)
    {
        if (!IsInHand)
            return;

        if (cooldown > 0)
            return;

        if (ammo <= 0)
            return;
        
        cooldown = 1 / attackSpeed;
        ammo--;

        RaycastHit hit;
        if (Physics.Raycast(bulletPath, out hit))
        {
            if (!hit.collider.CompareTag("Enemy"))
                return;

            Debug.Log("HIT ENEMY");

            hit.collider.GetComponent<Enemy>().TakeDamage();
        }
    }
}