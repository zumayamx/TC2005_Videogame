using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turn_manager : MonoBehaviour
{
    // Public variables for hand controls
    public GameObject blue_controller;
    public GameObject red_controller;

    // Public variables for slot controls
    public GameObject blue_slot;
    public GameObject red_slot;

    public SpriteRenderer sprite_inicio;
    public SpriteRenderer sprite_azul;
    public SpriteRenderer sprite_rojo;

    public bool blue_turn;
    public int turnCount;

    // Lists of cards for each side
    public List<GameObject> blue_cards;
    public List<GameObject> red_cards;

    // Start is called before the first frame update
    void Start()
    {
        turnCount = 0;
        blue_turn = true;
        UpdateTurnDisplay();
        UpdateCardsVisibility();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (blue_turn)
            {
                blue_turn = false;
                SetTurnActive(blue_turn);
            }
            else
            {
                blue_turn = true;
                SetTurnActive(blue_turn);
                turnCount++;
                Debug.Log("Total Turns Passed: " + turnCount);
            }

            UpdateCardsVisibility();
        }
    }

    void SetTurnActive(bool isBlueTurn)
    {
        sprite_azul.enabled = isBlueTurn;
        sprite_rojo.enabled = !isBlueTurn;
        blue_controller.SetActive(isBlueTurn);
        blue_slot.SetActive(isBlueTurn);
        red_controller.SetActive(!isBlueTurn);
        red_slot.SetActive(!isBlueTurn);

        // Reset energy for each side's CardSpawner script
        if (isBlueTurn)
        {
            ResetEnergy(blue_controller);
        }
        else
        {
            ResetEnergy(red_controller);
        }
    }

    void ResetEnergy(GameObject controller)
    {
        CardSpawner spawner = controller.GetComponent<CardSpawner>();
        if (spawner != null)
        {
            spawner.energy = 3;
            spawner.UpdateEnergyText();
        }
    }

    void UpdateTurnDisplay()
    {
        sprite_azul.enabled = blue_turn;
        sprite_rojo.enabled = !blue_turn;
        blue_controller.SetActive(blue_turn);
        blue_slot.SetActive(blue_turn);
        red_controller.SetActive(!blue_turn);
        red_slot.SetActive(!blue_turn);
    }

    public void UpdateCardsVisibility()
    {
        foreach (GameObject card in blue_cards)
        {
            SpriteRenderer sr = card.GetComponent<SpriteRenderer>();
            Collider collider = card.GetComponent<Collider>();
            if (sr != null)
            {
                sr.enabled = !blue_turn;
            }
            if (collider != null)
            {
                collider.enabled = !blue_turn;
            }
        }

        foreach (GameObject card in red_cards)
        {
            SpriteRenderer sr = card.GetComponent<SpriteRenderer>();
            Collider collider = card.GetComponent<Collider>();
            if (sr != null)
            {
                sr.enabled = blue_turn;
            }
            if (collider != null)
            {
                collider.enabled = blue_turn;
            }
        }
    }
}
