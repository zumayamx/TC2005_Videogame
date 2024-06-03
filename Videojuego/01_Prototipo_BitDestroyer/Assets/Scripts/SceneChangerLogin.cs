using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChangerLogin : MonoBehaviour
{
    public Button loginButton;
    
    public Button back;

    public Button registerButton;

    public Button gameButton;

    public TMP_InputField name_player;

    public TMP_InputField password_player;

    public Sprite name_player_red;

    public Sprite password_player_red;

    public Sprite loginButton_red;

    public Sprite registerButton_red;

    void Start()
    {
        loginButton.onClick.AddListener(() => ToNextLogin());
        back.onClick.AddListener(() => ToModeElection());
        gameButton.onClick.AddListener(() => ToGame());

        if (PlayerPrefs.GetInt("modoJuego") == 1 || PlayerPrefs.GetInt("modoJuego") == 0)
        {
            gameButton.gameObject.SetActive(false);
        } 

    }

    private void ToNextLogin()
    {
        if (PlayerPrefs.GetInt("modoJuego") == 0) {
            Debug.Log(PlayerPrefs.GetInt("loginTwo"));
            loginButton.gameObject.SetActive(false);
            gameButton.gameObject.SetActive(true);
        }

        if (PlayerPrefs.GetInt("modoJuego") == 1)
        {
            PlayerPrefs.SetInt("modoJuego", 0);
            ChangeButtonImagesRed();
            //SceneManager.LoadScene("InicioSesion");
        }
    }

    private void ToModeElection()
    {
        SceneManager.LoadScene("ModeElection");
    }

    private void ToGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void ChangeButtonImagesRed() {
        name_player.GetComponent<Image>().sprite = name_player_red;
        password_player.GetComponent<Image>().sprite = password_player_red;
        loginButton.GetComponent<Image>().sprite = loginButton_red;
        registerButton.GetComponent<Image>().sprite = registerButton_red;
    }
}
