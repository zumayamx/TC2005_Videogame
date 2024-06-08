/*
- José Manuel García Zumaya
- 05/06/2024

- Description:
    This script is used if the specific card is selected,
    it will show a roulette that will deal damage to either player.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootcampCard_25 : MonoBehaviour
{
    /* Panel to show the roulette, initially active false in the spawn_card_in_hand script */
    public GameObject roulettePanel;
    public bool activePanelRoulette = false; 
    
    void Start()
    {
        Debug.Log("BootcampCard_25.cs STARTED IN SLOT!");

        if (roulettePanel == null) {
            roulettePanel = GameObject.Find("RoulettePanel");
        } 
        else {
            Debug.Log("Roulette Panel already assigned!");
        }
    }
    void Update()
    {
        if (activePanelRoulette)
        {
            roulettePanel.SetActive(true);
            ToDestroy();
        }
    }

    private void ToDestroy()
    {
        Destroy(gameObject);
    }
}