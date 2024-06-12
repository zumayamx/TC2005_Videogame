/*
- José Manuel García Zumaya
- 06/06/2024

- Description:
    This script is used if the specific card is selected (coquita),
    it will give a three point of health to the player.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BootcampCard_06 : MonoBehaviour
{
    public Image healthBarRed; // Image to display the health bar
    public TMP_Text healthTextRed; // TMP Text element to display health
    public Image healthBarBlue; // Image to display the health bar
    public TMP_Text healthTextBlue; // TMP Text element to display health
    public bool activateHealth = false; // Flag to indicate if the health is activated
    public GameObject playerTurn; // Reference to the player object
    public GameObject hitPlayerRed; // Reference to the red player health object
    public GameObject hitPlayerBlue; // Reference to the blue player health object

    private bool isBlueTurn; // Flag to indicate if it's the blue player's turn

    // Start is called before the first frame update
    void Start()
    {
        playerTurn = GameObject.Find("turn_manager");
        isBlueTurn = playerTurn.GetComponent<turn_manager>().blue_turn;

        healthBarRed = GameObject.Find("All_Health_Red").GetComponent<Image>();
        healthTextRed = GameObject.Find("hp jugador rojo").GetComponent<TMP_Text>();

        healthBarBlue = GameObject.Find("All_Health_Blue").GetComponent<Image>();
        healthTextBlue = GameObject.Find("hp jugador azul").GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (activateHealth && isBlueTurn)
        {
            int currentHealthB = PlayerPrefs.GetInt("playerBlueHealth");
            healthToPlayerBlue(currentHealthB);
            ToDestroy();
        }
        else if (activateHealth && !isBlueTurn)
        {
            int currentHealthR = PlayerPrefs.GetInt("playerRedHealth");
            healthToPlayerRed(currentHealthR);
            ToDestroy();
        }
        
    }

    private void healthToPlayerRed(int currentHealthRed) {
        currentHealthRed += 3;
        PlayerPrefs.SetInt("playerRedHealth", currentHealthRed);
        healthTextRed.text = currentHealthRed.ToString();
        healthBarRed.fillAmount = (float)currentHealthRed / 20;
    }

    private void healthToPlayerBlue(int currentHealthBlue) {
        currentHealthBlue += 3;
        PlayerPrefs.SetInt("playerBlueHealth", currentHealthBlue);
        healthTextBlue.text = currentHealthBlue.ToString();
        healthBarBlue.fillAmount = (float)currentHealthBlue / 20;
    }

    private void ToDestroy()
    {
        Destroy(gameObject);
    }
}
