using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AI_turn_manager : MonoBehaviour
{
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

    // Additional attributes from turn_normal
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

    // Start is called before the first frame update
    void Start()
    {
        turnCount = 0;
        blue_turn = true;
        UpdateTurnDisplay();
        UpdateCardsVisibility();

        buttonExit.onClick.AddListener(() => {
            ToModeElection(); 
        });

        buttonRestart.onClick.AddListener(() => {
            SceneManager.LoadScene("AI_Game");
        });
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
            textEndGame.text = "Player " + nameBlue + "defeated!" + "\n" + nameRed + " wins!";
            gameOverManager = GameObject.Find("turn_manager").GetComponent<GameOverManager>();
            gameOverManager.playerWinner = GameObject.Find("playersManager").GetComponent<PlayersManager>().playersList.players[1];
            gameOverManager.playerDefeated = GameObject.Find("playersManager").GetComponent<PlayersManager>().playersList.players[0];
            gameOverManager.playerWinnerIsBlue = false;
            gameOverManager.sendData = true;
            gameOverManager.enabled = true;
        }

        if (isRedDead) {
            panelEndGame.SetActive(true);
            string nameBlue = GameObject.Find("playersManager").GetComponent<PlayersManager>().playersList.players[0].nombre;
            string nameRed = GameObject.Find("playersManager").GetComponent<PlayersManager>().playersList.players[1].nombre;
            textEndGame.text = nameRed + "defeated!" + "\n" + "Player " + nameBlue + " wins!";
            gameOverManager = GameObject.Find("turn_manager").GetComponent<GameOverManager>();
            gameOverManager.playerWinner = GameObject.Find("playersManager").GetComponent<PlayersManager>().playersList.players[0];
            gameOverManager.playerDefeated = GameObject.Find("playersManager").GetComponent<PlayersManager>().playersList.players[1];
            gameOverManager.playerWinnerIsBlue = true;
            gameOverManager.sendData = true;
            gameOverManager.enabled = true;
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
        AI_Controller spawnerAI = controller.GetComponent<AI_Controller>();
        if (spawner != null)
        {
            spawner.energy = 5;
            spawner.UpdateEnergyBar();
        }
        if (spawnerAI != null)
        {
            spawnerAI.energy = 5;
            spawnerAI.UpdateEnergyBar();
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

    public void ShowCardsDependsPlayerTurn(bool isBlueTurn)
    {
        // If is blue turn, show me red cards
        if (isBlueTurn)
        {
            foreach (GameObject card in red_cards)
            {
                SpriteRenderer sr = card.GetComponent<SpriteRenderer>();
                Collider collider = card.GetComponent<Collider>();

                if (sr != null)
                {
                    sr.enabled = !blue_turn;
                }
                if (collider != null)
                {
                    // This is a collider of hide card prefab, not card behind it
                    collider.enabled = !blue_turn;
                }
            }
        }
        // If is red turn, show me blue cards
        else
        {
            foreach (GameObject card in blue_cards)
            {
                SpriteRenderer sr = card.GetComponent<SpriteRenderer>();
                Collider collider = card.GetComponent<Collider>();

                if (sr != null)
                {
                    sr.enabled = blue_turn;
                }
                if (collider != null)
                {
                    // This is a collider of hide card prefab, not card behind it
                    collider.enabled = blue_turn;
                }
            }
        }
    }

     private void ToModeElection()
    {
        // Clear the players list
        GameObject.Find("playersManager").GetComponent<PlayersManager>().playersList.players.Clear();
        SceneManager.LoadScene("ModeElection");
    }
}
