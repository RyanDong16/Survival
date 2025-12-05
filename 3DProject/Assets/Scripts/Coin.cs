using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public void OnTriggerEnter(Collider coll)
    {
        PlayerInventory playerInventory = coll.GetComponent<PlayerInventory>();

        //if player collects coin increment coin count and deactivate
        if (playerInventory != null)
        {
            playerInventory.CoinCollected();
            gameObject.SetActive(false);
        }
    }
}
