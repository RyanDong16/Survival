using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public static event Action OnPlayerDeath;
    [SerializeField] private float startingHealth;
    [SerializeField] private AudioClip hurtSound;

    private PlayerInventory totalCoins;
    public bool stopDamage;

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

        if (currentHealth >= 0)
        {
            //player hurt
            SoundManager.Instance.PlaySound(hurtSound);
        }

        else
        {
            //player dead 
            OnPlayerDeath?.Invoke();
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
            SoundManager.Instance.PlaySound(hurtSound);
        }

       // if coins collected is equal to multiple of 15 and if health is not full then add life
        //if (totalCoins.NumberOfCoins % 15 == 0 && currentHealth < startingHealth)
        //{
        //    AddHealth(1);
        //}
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bite"))
        {
            StartCoroutine(TookDamage());
        }

        if (other.gameObject.CompareTag("Water"))
        {
            OnPlayerDeath?.Invoke();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Hit: " + collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Penguin"))
        {
            StartCoroutine(TookDamage());
        }
        if (collision.gameObject.CompareTag("Tiger"))
        {
            StartCoroutine(TookDamage());
        }
    }

    private IEnumerator TookDamage()
    {
        if (!stopDamage)
        {
            stopDamage = true;
            TakeDamage(1);
            SoundManager.Instance.PlaySound(hurtSound);
            yield return new WaitForSeconds(1f);
            stopDamage = false;
        }
    }
}

