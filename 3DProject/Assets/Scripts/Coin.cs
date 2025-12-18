using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private AudioClip coinSound;

    public void OnTriggerEnter(Collider coll)
    {
        PlayerInventory playerInventory = coll.GetComponent<PlayerInventory>();

        //if player collects coin increment coin count and deactivate
        if (playerInventory != null)
        {
            SoundManager.Instance.PlaySound(coinSound);
            playerInventory.CoinCollected();
            gameObject.SetActive(false);
        }
    }
}
