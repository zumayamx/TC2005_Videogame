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
    [SerializeField] private GameObject hideCardPrefab; // Sprite to hide the card

 // List of the hand cards of the player red, (the position have a child card), for that reason is a list of GameObject
    public List<GameObject> blue_positions; 

    public int turnBegin;

    private int turnsUpdate;

    private bool isBlueTurn;

    // Update is called once per frame
    void Update()
    {
        // Check if the turn count is different from the previous update
        isBlueTurn = GameObject.Find("turn_manager").GetComponent<turn_manager>().blue_turn;

        if (isBlueTurn) {
            // Show the defense cards but NOT desactivate the object
            ShowDefenseCards(false);
        }

        if (!isBlueTurn) {
            // Hide the defense cards.
            //If it is not the blue turn to hide the defense cards played in a turn past
            HideDefenseCards(); 
        }

         // Obtain the turn count from the turn_manager script
        turnsUpdate = GameObject.Find("turn_manager").GetComponent<turn_manager>().turnCount;
        Debug.Log("Turns Update: " + turnsUpdate);
        Debug.Log("Turn TO Show: " + (turnBegin + 4));
        // Check if it has been 2 turns since the defense cards were hide
        if (turnsUpdate == turnBegin + 4) {
            // Show the defense cards and desactivate the object
            ShowDefenseCards(true);
        }
    }

    private void HideDefenseCards() {
        foreach (GameObject handPosition in blue_positions) {

            DefenseCard[] defenseCards = handPosition.GetComponentsInChildren<DefenseCard>();
            
            if (defenseCards != null && defenseCards.Length > 0) {
                foreach (DefenseCard defenseCard in defenseCards) {
                    if (defenseCard.isHide != true) {
                        defenseCard.isHide = true;
                        GameObject defenseCardObject = defenseCard.gameObject;
                        defenseCardObject.GetComponent<MeshRenderer>().enabled = false;
                        GameObject hideCard = Instantiate(hideCardPrefab, defenseCardObject.transform.position, defenseCardObject.transform.rotation);
                    }
                }
            }
        }
    }   

    public void ShowDefenseCards(bool desactivateObject) {
        foreach (GameObject handPosition in blue_positions) {

            DefenseCard[] defenseCards = handPosition.GetComponentsInChildren<DefenseCard>();

            if (defenseCards != null && defenseCards.Length > 0) {
                foreach (DefenseCard defenseCard in defenseCards) {
                    defenseCard.isHide = false;
                    GameObject defenseCardObject = defenseCard.gameObject;
                    defenseCardObject.GetComponent<MeshRenderer>().enabled = true; 
                }
            }
        }
        GameObject[] hidePrefabs = GameObject.FindGameObjectsWithTag("defensePrefBlue");

        foreach (GameObject hidePrefab in hidePrefabs) {
            Destroy(hidePrefab);
        }
        
        if (desactivateObject) {
            this.gameObject.SetActive(false);
        }
    }
}
