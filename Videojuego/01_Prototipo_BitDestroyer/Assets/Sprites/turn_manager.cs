using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    // Bool variables to indicate if a player is dead
    public bool isBlueDead = false;
    public bool isRedDead = false;

    // Buttons and text to panel of end game
    public GameObject panelEndGame;
    public Button buttonRestart;
    public Button buttonExit;
    public TMP_Text textEndGame;

    // Object to send data to data base
    public GameOverManager gameOverManager;

    // Variable to store the start time of the match
    private DateTime startTime;

    public GameObject sceneManager;

    // Start is called before the first frame update
    void Start()
    {
        sceneManager = GameObject.Find("SceneManager");

        turnCount = 0;
        blue_turn = true;
        UpdateTurnDisplay();
        UpdateCardsVisibility();

        //Set the buttons chancge scenes
        buttonExit.onClick.AddListener(() => {
            ToModeElection(); 
        });

        buttonRestart.onClick.AddListener(() => {
            SceneManager.LoadScene("Game");
        });

        // Set the start time
        startTime = DateTime.Now;
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

        isRedDead = PlayerPrefs.GetInt("playerRedHealth") == 0 ? true : false;
        isBlueDead = PlayerPrefs.GetInt("playerBlueHealth") == 0 ? true : false;

        if (isBlueDead) {
            panelEndGame.SetActive(true);
            string nameBlue = GameObject.Find("playersManager").GetComponent<PlayersManager>().playersList.players[0].nombre;
            string nameRed = GameObject.Find("playersManager").GetComponent<PlayersManager>().playersList.players[1].nombre;
            textEndGame.text = "Player " + nameBlue + " defeated!" + "\n" + "Player " + nameRed + " wins!";
            gameOverManager = GameObject.Find("turn_manager").GetComponent<GameOverManager>();
            gameOverManager.playerWinner = GameObject.Find("playersManager").GetComponent<PlayersManager>().playersList.players[1];
            gameOverManager.playerDefeated = GameObject.Find("playersManager").GetComponent<PlayersManager>().playersList.players[0];
            gameOverManager.playerWinnerIsBlue = false;
            gameOverManager.sendData = true;
            gameOverManager.enabled = true;
            TimeSpan elapsedTime = DateTime.Now - startTime;
            string timeMatch = string.Format("{0:D2}:{1:D2}:{2:D2}", 
                                             elapsedTime.Hours, 
                                             elapsedTime.Minutes, 
                                             elapsedTime.Seconds);
            gameOverManager.timeMatchGlobal = timeMatch;
            // To evoid multiple calls to this function
            isBlueDead = false;
        }

        if (isRedDead) {
            panelEndGame.SetActive(true);
            string nameBlue = GameObject.Find("playersManager").GetComponent<PlayersManager>().playersList.players[0].nombre;
            string nameRed = GameObject.Find("playersManager").GetComponent<PlayersManager>().playersList.players[1].nombre;
            textEndGame.text = "Player " + nameRed + " defeated!" + "\n" + "Player " + nameBlue + " wins!";
            gameOverManager = GameObject.Find("turn_manager").GetComponent<GameOverManager>();
            gameOverManager.playerWinner = GameObject.Find("playersManager").GetComponent<PlayersManager>().playersList.players[0];
            gameOverManager.playerDefeated = GameObject.Find("playersManager").GetComponent<PlayersManager>().playersList.players[1];
            gameOverManager.playerWinnerIsBlue = true;
            gameOverManager.sendData = true;
            gameOverManager.enabled = true;
            TimeSpan elapsedTime = DateTime.Now - startTime;
            string timeMatch = string.Format("{0:D2}:{1:D2}:{2:D2}", 
                                             elapsedTime.Hours, 
                                             elapsedTime.Minutes, 
                                             elapsedTime.Seconds);
            gameOverManager.timeMatchGlobal = timeMatch;
            // To evoid multiple calls to this function
            isRedDead = false;
        }
    }

    public void SetTurnActive(bool isBlueTurn)
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
            spawner.energy = 5;
            spawner.UpdateEnergyBar();
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

    public void ShowCardsDependsPlayerTurn(bool isBlueTurn) {
        // If is blue turn, show me red cards
        if (isBlueTurn) {
            foreach (GameObject card in red_cards) {
                SpriteRenderer sr = card.GetComponent<SpriteRenderer>();
                Collider collider = card.GetComponent<Collider>();
                
                if (sr != null) {
                    sr.enabled = !blue_turn;
                    }
                if (collider != null) {
                    // This is a collider of hide card prefab, not card behind it
                    collider.enabled = !blue_turn;
                    }
            }
        }
        // If is red turn, show me blue cards
        else {
            foreach (GameObject card in blue_cards) {
                SpriteRenderer sr = card.GetComponent<SpriteRenderer>();
                Collider collider = card.GetComponent<Collider>();
                
                if (sr != null) {
                    sr.enabled = blue_turn;
                    }
                if (collider != null) {
                    // This is a collider of hide card prefab, not card behind it
                    collider.enabled = blue_turn;
                    }
            }
        }
    }

    private void ToModeElection()
    {
        // Clear the players list
        GameObject PlayerManager = GameObject.Find("playersManager");
        Destroy(PlayerManager);
        Debug.Log("Players list cleared");

        if (sceneManager != null)
        {
            sceneManager.GetComponent<SceneTransition>().LoadScene("ModeElection");
        }
        else
        {
            SceneManager.LoadScene("ModeElection");
        }
    }
}
