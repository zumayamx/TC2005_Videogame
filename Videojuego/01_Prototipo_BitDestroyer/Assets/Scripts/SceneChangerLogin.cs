
/* 
 - José Manuel García Zumaya A01784238
 - 02/06/2024

 - Description:
    This script is used to manage the login of one or two players in the game,
    it change the color of the buttons and input fields to diferentiate the players.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChangerLogin : MonoBehaviour
{
    /* Button to login */
    public Button loginButton;
    
    /* Button to back */
    public Button back;

    /* Button to register */
    public Button registerButton;

    /* Button to go to the game */
    public Button gameButton;

    /* Input field for name player */
    public TMP_InputField name_player;

    /* Input field for password player */
    public TMP_InputField password_player;

    /* Sprites for the red theme */
    public Sprite name_player_red;
    
    public Sprite password_player_red;

    public Sprite loginButton_red;

    public Sprite registerButton_red;

    private LoginManager loginManager;

    public SceneTransition sceneTransition; // Referencia al SceneTransition

    void Start()
    {
        // Get the login manager
        loginManager = GameObject.Find("Scripter").GetComponent<LoginManager>();
        /* Add listeners to the buttons */
        loginButton.onClick.AddListener(() => ToNextLogin());
        back.onClick.AddListener(() => ToModeElection());

        if (PlayerPrefs.GetInt("gameMode") == 0)
        {
             gameButton.onClick.AddListener(() => StartCoroutine(ToGameIA()));
        }

        if (PlayerPrefs.GetInt("gameMode") == 1)
        {
            gameButton.onClick.AddListener(() => StartCoroutine(ToGame()));
        }

        /* If the game mode is one player or two players, hide the game button to the game */
        if (PlayerPrefs.GetInt("gameMode") == 1 || PlayerPrefs.GetInt("gameMode") == 0)
        {
            gameButton.gameObject.SetActive(false);
        } 
    }

    /* Change to the next login depending of the game mode */
    private void ToNextLogin()
    {
        /* If the game mode is one player or is the second player login in multiplayer mode.
            - Desactivate the login button
            - Activate the game button
         */
        if (PlayerPrefs.GetInt("gameMode") == 0) {
           // Debug.Log(PlayerPrefs.GetInt("loginTwo"));
            loginButton.gameObject.SetActive(false);
            gameButton.gameObject.SetActive(true);
        }

        /* If the game mode is multiplayer, change to next login of the second player
            - Change the game mode to "one player"
            - Change the color of the buttons and input fields
        */
        if (PlayerPrefs.GetInt("gameMode") == 1)
        {
            PlayerPrefs.SetInt("gameMode", 0);
            ChangeButtonImagesRed();
            //SceneManager.LoadScene("InicioSesion");
        }
    }

    /* Change to election mode scene if the back button is pressed */
    private void ToModeElection()
    {
        if (sceneTransition != null)
        {
            sceneTransition.LoadScene("ModeElection");
        }
        else
        {
            SceneManager.LoadScene("ModeElection");
        }
    }

    /* Change to the game scene if the next button is pressed */
    private IEnumerator ToGame()
    {
        yield return new WaitForSeconds(1);
        if (sceneTransition != null)
        {
            sceneTransition.LoadScene("Game");
        }
        else
        {
            SceneManager.LoadScene("Game");
        }
    }

    /* Change to the game IA scene if the next button is pressed */
    private IEnumerator ToGameIA() {
         yield return new WaitForSeconds(1);
        StartCoroutine(LoginAndLoadScene());
        }
    private IEnumerator LoginAndLoadScene() {
        yield return StartCoroutine(loginManager.OnSubmitLogin("http://localhost:3000", "IA", "IA123"));
        if (sceneTransition != null)
        {
            sceneTransition.LoadScene("AI_Game");
        }
        else
        {
            SceneManager.LoadScene("AI_Game");
        }
    }

    /* Change the color of the buttons and input fields to red */
    public void ChangeButtonImagesRed() {
        name_player.GetComponent<Image>().sprite = name_player_red;
        password_player.GetComponent<Image>().sprite = password_player_red;
        loginButton.GetComponent<Image>().sprite = loginButton_red;
        registerButton.GetComponent<Image>().sprite = registerButton_red;
    }
}
