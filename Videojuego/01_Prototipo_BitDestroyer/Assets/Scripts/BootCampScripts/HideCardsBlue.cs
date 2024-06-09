/* 
- José Manuel García Zumaya
- 05/06/2024

- Description:
    This script is used to hide the defense cards of the player blue;
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideCardsBlue : MonoBehaviour
{
    public Sprite hideCard; // Sprite to hide the card
    public List<GameObject> blue_cards; // List of blue cards

    private int turnsCount;

    private int turnUpdate;

    private bool isBlueTurn;

    // Start is called before the first frame update
    void Start()
    {
        // Get the turn count from the turn_manager script
        turnsCount = GameObject.Find("turn_manager").GetComponent<turn_manager>().turnCount;
        Debug.Log("Turns Count: " + turnsCount);
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the turn count is different from the previous update
        isBlueTurn = GameObject.Find("turn_manager").GetComponent<turn_manager>().blue_turn;

        if (!isBlueTurn) {
            // Hide the defense cards.
            //If it is not the blue turn to hide the defense cards played in a turn past
            HideDefenseCards();
        }

        // Check if it has been 2 turns since the defense cards were hide
        turnsCount = GameObject.Find("turn_manager").GetComponent<turn_manager>().turnCount;
        if (turnsCount == turnsCount + 4) {
            // Show the defense cards
            ShowDefenseCards();
        }
    }

    private void HideDefenseCards()
    {
        foreach (GameObject handPosition in blue_cards) {
           
        }
    }

    private void ShowDefenseCards()
    {
        // foreach (GameObject card in blue_cards)
        // {
        //     card.GetComponent<SpriteRenderer>().sprite = card.GetComponent<Card>().cardSprite;
        // }

        // this.GameObject.SetActive(false);
    }
}
