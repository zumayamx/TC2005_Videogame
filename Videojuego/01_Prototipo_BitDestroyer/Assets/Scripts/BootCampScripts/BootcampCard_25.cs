/*
- José Manuel García Zumaya
- 05/06/2024

- Description:
    This script is used if the specific card is selected,
    it will show a roulette that will deal damage to either player.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootcampCard_25 : MonoBehaviour
{
    /* Panel to show the roulette, initially active false in the spawn_card_in_hand script */
    public GameObject roulettePanel;

    public bool activePanelRoulette = false; 
    public float initialRotationSpeed = 1000f; // Velocidad de rotación inicial
    private float currentRotationSpeed; // Velocidad de rotación actual
    private bool isSpinning = false;    // State of the roulette
    private float spinDuration = 3.0f;  // Duración del giro

    void Start()
    {
        Debug.Log("BootcampCard_25.cs STARTED!");
        roulettePanel = GameObject.Find("RoulettePanel");
    }
    void Update()
    {
        if (activePanelRoulette && roulettePanel != null) {
             roulettePanel.SetActive(true);
        } 
        // else {
        //     Debug.Log("Roulette Panel is null");
        // }

        if (isSpinning)
        {
            this.transform.Rotate(Vector3.forward, -currentRotationSpeed * Time.deltaTime);
        }
    }

    public void StartSpin()
    {
        if (!isSpinning)
        {
            isSpinning = true;
            currentRotationSpeed = initialRotationSpeed;
            StartCoroutine(StopSpin());
        }
    }

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

    private void DetermineResult()
    {
        float zRotation = transform.eulerAngles.z % 360;
        string result;

        if (zRotation < 90)
            result = "Rojo";
        else if (zRotation < 180)
            result = "Azul";
        else if (zRotation < 270)
            result = "Rojo";
        else
            result = "Azul";

        Debug.Log("Resultado: " + result);

        roulettePanel.SetActive(false);
    }
}