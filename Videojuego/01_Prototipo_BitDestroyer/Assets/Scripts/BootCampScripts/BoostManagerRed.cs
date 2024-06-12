/*
- José Manuel García Zumaya
- 05/06/2024

- Description:
    This script is used instantiate the buttons that define a boost for a one card in
    a hand list cards of the player red. It also apply a boost of three point according
    to selected card.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class BoostManagerRed : MonoBehaviour
{
    // Panel to instantiate the buttons
    public GameObject boostPanelRed;
    // Prefab to instantiate the buttons
    public GameObject boostButtonPrefab;

    // List of the hand cards of the player red, (the position have a child card), for that reason is a list of GameObject
    public List<GameObject> red_positions; 

    // Start is called before the first frame update
    void Start()
    {
        // Hide the panel to boost the cards, (begin in false)
        boostPanelRed.SetActive(false);
        // Instantiate the buttons of card to boost (begin in false boost manager)
        InstantiateBoostButtons();
    }

    private void InstantiateBoostButtons() {
        // List to save the defense scripts cards
        List<DefenseCard> defenseCardsList = new List<DefenseCard>();
        // List to save the attack scripts cards
        List<AttackCard> attackCardsList = new List<AttackCard>();
        
        // Delete the buttons of the last turn
        foreach (Transform child in transform) {
            // This destroy the buttons of the last turn
            Destroy(child.gameObject);
        }

        foreach (GameObject handPosition in red_positions) {

            DefenseCard[] defenseCards = handPosition.GetComponentsInChildren<DefenseCard>();
            AttackCard[] attackCards = handPosition.GetComponentsInChildren<AttackCard>();

            if (defenseCards.Length > 0) {
                // Add the defense card to the list, (only one card) because the position only can have one card
                defenseCardsList.Add(defenseCards[0]);
            }

            if (attackCards.Length > 0) {
                // Add the attack card to the list, (only one card) because the position only can have one card
                attackCardsList.Add(attackCards[0]);
            }

            if (defenseCards.Length == 0 && attackCards.Length == 0) {
                Debug.Log("No defense or attack card in the positon slot");
            }
        }
        if (defenseCardsList.Count > 0 || attackCardsList.Count > 0) {
            boostPanelRed.SetActive(true);
        }
        else {
            Debug.Log("No defense or attack cards in the slots");
        }

        if (defenseCardsList.Count > 0) {
            foreach (DefenseCard defenseCard in defenseCardsList) {
                // Instantiate the button to boost the card like children of panel
                GameObject boostButtonObj = Instantiate(boostButtonPrefab, boostPanelRed.transform);
                Button boostButton = boostButtonObj.GetComponent<Button>();
                // Transform the texture to sprite
                Texture2D texture = defenseCard.gameObject.GetComponent<MeshRenderer>().material.mainTexture as Texture2D;
                Rect rect = new Rect(0, 0, texture.width, texture.height);
                Vector2 pivot = new Vector2(0.5f, 0.5f);
                Sprite spriteDefense = Sprite.Create(texture, rect, pivot);
                boostButton.GetComponent<Image>().sprite = spriteDefense;
                // Boost the defense card by 3 points when the button is clicked through his boostDefense method
                boostButton.onClick.AddListener(() => ApplyDefenseBoost(defenseCard));
            }
        }

        if (attackCardsList.Count > 0) {
            foreach (AttackCard attackCard in attackCardsList) {
                 // Instantiate the button to boost the card like children of panel
                GameObject boostButtonObj = Instantiate(boostButtonPrefab, boostPanelRed.transform);
                Button boostButton = boostButtonObj.GetComponent<Button>();
                // Transform the texture to sprite
                Texture2D texture = attackCard.gameObject.GetComponent<MeshRenderer>().material.mainTexture as Texture2D;
                Rect rect = new Rect(0, 0, texture.width, texture.height);
                Vector2 pivot = new Vector2(0.5f, 0.5f);
                Sprite spriteAttack = Sprite.Create(texture, rect, pivot);
                boostButton.GetComponent<Image>().sprite = spriteAttack;
                // Boost the attack card by 3 points when the button is clicked through his boostAttack method
                boostButton.onClick.AddListener(() => ApplyAttackBoost(attackCard));
            }
        }
    }

    // Function to apply attack boost and desactivate the panel
    private void ApplyAttackBoost(AttackCard attackCard) {
        attackCard.BoostAttack(3);
        Debug.Log("Boost Attack Applied");
        boostPanelRed.SetActive(false);
    }

    // Function to apply defense boost and desactivate the panel
    private void ApplyDefenseBoost(DefenseCard defenseCard) {
        defenseCard.BoostDefense(3);
        Debug.Log("Boost Defense Applied");
        boostPanelRed.SetActive(false);
    }
}
