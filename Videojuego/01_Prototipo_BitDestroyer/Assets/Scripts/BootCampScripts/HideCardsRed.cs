/* 
- José Manuel García Zumaya
- 05/06/2024

- Description:
    This script is used to hide the defense cards of the player red;
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideCardsRed : MonoBehaviour
{
    [SerializeField] private GameObject hideCardPrefab; // Sprite to hide the card

    public List<GameObject> red_positions; // List of blue cards

    public int turnBegin;

    private int turnsUpdate;

    private bool isBlueTurn;

    // Update is called once per frame
    void Update()
    {
        // Check if the turn count is different from the previous update
        isBlueTurn = GameObject.Find("turn_manager").GetComponent<turn_manager>().blue_turn;

        if (!isBlueTurn) {
            ShowDefenseCards(false);
        }

        if (isBlueTurn) {
            // Hide the defense cards.
            //If it is not the blue turn to hide the defense cards played in a turn past
            HideDefenseCards(); //Verificar si se repite demasiadas veces
        }

         // Obtain the turn count from the turn_manager script
        turnsUpdate = GameObject.Find("turn_manager").GetComponent<turn_manager>().turnCount;
        Debug.Log("Turns Update: " + turnsUpdate);
        Debug.Log("Turn TO Show: " + (turnBegin + 4));
        // Check if it has been 2 turns since the defense cards were hide
        if (turnsUpdate == turnBegin + 4) {
            // Show the defense cards
            ShowDefenseCards(true);
        }
    }

    private void HideDefenseCards() {
        foreach (GameObject handPosition in red_positions) {

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
        foreach (GameObject handPosition in red_positions) {

            DefenseCard[] defenseCards = handPosition.GetComponentsInChildren<DefenseCard>();

            if (defenseCards != null && defenseCards.Length > 0) {
                foreach (DefenseCard defenseCard in defenseCards) {
                    defenseCard.isHide = false;
                    GameObject defenseCardObject = defenseCard.gameObject;
                    defenseCardObject.GetComponent<MeshRenderer>().enabled = true; 
                }
            }
        }
        GameObject[] hidePrefabs = GameObject.FindGameObjectsWithTag("defensePrefRed");

        foreach (GameObject hidePrefab in hidePrefabs) {
            Destroy(hidePrefab);
        }

        if (desactivateObject) {
            this.gameObject.SetActive(false);
        }
    }
}
