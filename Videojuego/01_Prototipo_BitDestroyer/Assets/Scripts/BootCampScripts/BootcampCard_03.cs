/*
- José Manuel García Zumaya
- 05/06/2024

- Description:
    This script is used to activate the panel depetending of the player turn.
    The object to activate is the one that contains the script to boost atack or defense card.

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootcampCard_03 : MonoBehaviour
{
    // Public variable to activate the 
    public bool activateBoost = false;

    // Variable to check wich player turn is
    private bool isBlueTurn = false;

    // Objects that contains the script to boost a card
    public GameObject boostManagerRed;
    public GameObject boostManagerBlue;


    void Start()
    {
        // Check wich player turn is to activate the corresponden object
        isBlueTurn = GameObject.Find("turn_manager").GetComponent<turn_manager>().blue_turn;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (activateBoost && isBlueTurn)
        {
            boostManagerBlue.SetActive(true);
            ToDestroy();
            
        }
        else if (activateBoost && !isBlueTurn)
        {
            boostManagerRed.SetActive(true);  
            ToDestroy();
        }
    }

    private void ToDestroy()
    {
        Destroy(gameObject);
    }
}
