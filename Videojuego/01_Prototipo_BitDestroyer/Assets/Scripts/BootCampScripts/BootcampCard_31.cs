
/*
- José Manuel García Zumaya
- A017842388

- Description: 
    This script is used to give a new turn to player, 
   it reset the energy to normal value = 10.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootcampCard_31 : MonoBehaviour
{
    // bool to indicate if a new turn is happening
    public bool newTurn = false;

    // Flag to indicate if it's blue player's turn
    private bool isBlueTurn; 

    // Start is called before the first frame update
    void Start()
    {
        // Check what turn player is
        isBlueTurn = GameObject.Find("turn_manager").GetComponent<turn_manager>().blue_turn;
    }

    // Update is called once per frame
    void Update()
    {

        if (newTurn && isBlueTurn) {
            Debug.Log("New turn for blue player");
            GameObject turnManager = GameObject.Find("turn_manager");
            turnManager.GetComponent<turn_manager>().SetTurnActive(isBlueTurn);
            ToDestroy();
        }

        if (newTurn && !isBlueTurn) {
            Debug.Log("New turn for red player");
            GameObject turnManager = GameObject.Find("turn_manager");
            turnManager.GetComponent<turn_manager>().SetTurnActive(isBlueTurn);
            ToDestroy();
        }
        
    }

    // Function to destroy the card
    private void ToDestroy() {
        Destroy(gameObject);
    }
}
