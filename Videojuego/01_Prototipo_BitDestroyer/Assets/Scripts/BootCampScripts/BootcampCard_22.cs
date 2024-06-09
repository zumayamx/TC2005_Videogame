/*
- José Manuel García Zumaya
- 05/06/2024

- Description:
    This script is used to activate to show the cards of opposite player,
    even if the opoosite player have bootcamp card 29 (TOR) activated.
    This script call to functions of another scripts to show the cards.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootcampCard_22 : MonoBehaviour
{
    // Public variable to activate the show of the defense cards
    public bool desactivateHide = false;

    // Variable to check wich player turn is
    private bool isBlueTurn = false;

    // Start is called before the first frame update
    void Start()
    {
        // Check wich player turn is to call the corresponden function
        isBlueTurn = GameObject.Find("turn_manager").GetComponent<turn_manager>().blue_turn;  
    }
    // Update is called once per frame
    void Update()
    {
        if (desactivateHide && isBlueTurn)
        {
            showCardsRed();
            ToDestroy();
        }
        else if (desactivateHide && !isBlueTurn)
        {
            showCardsBlue();
            ToDestroy();
        }
  
    }

    // Function to show the defense cards of the blue player
    private void showCardsBlue() {
        // Find the object to show the defense cards
        GameObject hideCardsBlue = GameObject.Find("HideCardsBlue");

        if (hideCardsBlue != null) {
            Debug.Log("HideCardsBlue: " + hideCardsBlue);
            // Call the function to show the defense cards and desactivate the object
            hideCardsBlue.GetComponent<HideCardsBlue>().ShowDefenseCards(true);
        }
    }
    // Function to show the defense cards of the red player
    private void showCardsRed() {
        // Find the object to show the defense cards
        GameObject hideCardsRed = GameObject.Find("HideCardsRed");

        if (hideCardsRed != null) {
            Debug.Log("HideCardsRed: " + hideCardsRed);
            // Call the function to show the defense cards and desactivate the object
             hideCardsRed.GetComponent<HideCardsRed>().ShowDefenseCards(true);
        }
    }

    private void ToDestroy()
    {
        Destroy(gameObject);
    }
}
