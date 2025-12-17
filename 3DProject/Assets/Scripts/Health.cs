using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    private PlayerInventory totalCoins;
    //lets you get this variable from other scripts 
    public float currentHealth { get; private set; }

    private void Awake()
    {
        currentHealth = startingHealth;
    }

    public void TakeDamage(float _damage)
    {
        //makes sure that health doesnt go under 0 or max value 
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            //player hurt
        }

        else
        {
            //player dead 
        }
    }

    //adds a heart to player health 
    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TakeDamage(1);
        }

        //if coins collected is equal to multiple of 15 and if health is not full then add life
        if(totalCoins.NumberOfCoins % 15 == 0 && currentHealth < startingHealth)
        {
            AddHealth(1);
        }
    }
}
