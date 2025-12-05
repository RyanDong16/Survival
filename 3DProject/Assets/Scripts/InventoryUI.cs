using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    private TextMeshProUGUI cointText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cointText = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateCointText(PlayerInventory playerInventory)
    {
        cointText.text = playerInventory.NumberOfCoins.ToString();
    }
   
}
