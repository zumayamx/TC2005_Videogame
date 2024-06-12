/*
- José Manuel García Zumaya
- 05/06/2024

- Description:
    This script is used if the specific card is selected (puerta abierta),
    it will show a roulette that will deal damage to either player.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RouletteManager : MonoBehaviour
{
    /* Panel to show the roulette, initially active false in the spawn_card_in_hand script */
    private GameObject roulettePanel;
    public Image healthBarRed; // Image to display the health bar
    private int playerHealthBoth; // Starting health of the player
    public TMP_Text healthTextRed; // TMP Text element to display health
    public Image healthBarBlue; // Image to display the health bar
    public TMP_Text healthTextBlue; // TMP Text element to display health
    public bool activePanelRoulette = false; 
    public float initialRotationSpeed = 1000f; // Velocidad de rotación inicial
    private float currentRotationSpeed; // Velocidad de rotación actual
    private bool isSpinning = false;    // State of the roulette
    private float spinDuration;  // Duración del giro

    void Start()
    {
        spinDuration = Random.Range(1f, 6f);
        Debug.Log("Roulette Manager START!");
        roulettePanel = GameObject.Find("RoulettePanel");

    }
    void Update()
    {
        if (isSpinning)
        {
            this.transform.Rotate(Vector3.forward, -currentRotationSpeed * Time.deltaTime);
        }
    }

    /* Function to start the spin of the roulette */
    public void StartSpin()
    {
        if (!isSpinning)
        {
            isSpinning = true;
            currentRotationSpeed = initialRotationSpeed;
            StartCoroutine(StopSpin());
        }
    }

    /* Function to manage the rotation of roulette */
    private IEnumerator StopSpin()
    {
        float elapsedTime = 0f;

        while (elapsedTime < spinDuration)
        {
            elapsedTime += Time.deltaTime;
            currentRotationSpeed = Mathf.Lerp(initialRotationSpeed, 0, elapsedTime / spinDuration);
            yield return null;
        }

        isSpinning = false;
        DetermineResult();
    }

    /* Function to determine the result of the roulette */
    private void DetermineResult()
    {
        /* Get the z rotation of the roulette */
        float zRotation = transform.eulerAngles.z % 360;
        string result;
        //playerHealthBoth -= 1;
        
        if (zRotation < 90) {
            playerHealthBoth = PlayerPrefs.GetInt("playerRedHealth") - 1;
            Debug.Log("Player Red Health: " + playerHealthBoth);
            PlayerPrefs.SetInt("playerRedHealth", playerHealthBoth);
            UpdateHealthBarRed();
            UpdateHealthTextRed();
            result = "Rojo";
        }
        else if (zRotation < 180) {
            playerHealthBoth = PlayerPrefs.GetInt("playerBlueHealth") - 1;
            Debug.Log("Player Blue Health: " + playerHealthBoth);
            PlayerPrefs.SetInt("playerBlueHealth", playerHealthBoth);
            UpdateHealthBarBlue();
            UpdateHealthTextBlue();
            result = "Azul";
        }
        else if (zRotation < 270) {
            playerHealthBoth = PlayerPrefs.GetInt("playerRedHealth") - 1;
            Debug.Log("Player Red Health: " + playerHealthBoth);
            PlayerPrefs.SetInt("playerRedHealth", playerHealthBoth);
            UpdateHealthBarRed();
            UpdateHealthTextRed();
            result = "Rojo";
        }
        else {
            playerHealthBoth = PlayerPrefs.GetInt("playerBlueHealth") - 1;
            Debug.Log("Player Blue Health: " + playerHealthBoth);
            PlayerPrefs.SetInt("playerBlueHealth", playerHealthBoth);
            UpdateHealthBarBlue();
            UpdateHealthTextBlue();
            result = "Azul";
        }

        Debug.Log("Resultado: " + result);

        roulettePanel.SetActive(false);

    }

    /* Method to update the heath bar to player red in game */
    void UpdateHealthBarRed()
    {
        healthBarRed.fillAmount = (float)playerHealthBoth / 20f;
    }

    /* Method to update the heath bar to player blue in game */
    void UpdateHealthBarBlue()
    {
        healthBarBlue.fillAmount = (float)playerHealthBoth / 20f;
    }
    // Method to update the health display player red
    void UpdateHealthTextRed()
    {
        healthTextRed.text =  playerHealthBoth.ToString();
    }
    // Method to update the health display player blue
    void UpdateHealthTextBlue()
    {
        healthTextBlue.text = playerHealthBoth.ToString();
    }
}