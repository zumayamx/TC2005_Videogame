
/* 
- José Manuel García Zumaya A01784238 
- 02/06/2024

- Description:
    This script is used to manage the login and register of the players in the game,
    it uses the library Networking to send the query data to the server and get the response.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

public class LoginManager : MonoBehaviour
{
    /* Input field for name player */
    public TMP_InputField name_player;

    /* Input field for password player */
    public TMP_InputField password_player;

    /* Button to login */
    public Button button_player_login;

    /* Button to register */
    public Button button_player_register;

    /* URL of the API */
    [SerializeField] string apiURL = "http://localhost:3000";

    /* Result of the query */
    public Result result;

    /* Players object  to deserialize the response */
    public Players players;

    /* PlayersManager object to manage the players through the game  */
    public PlayersManager playersManager;    

    void Start()
    {
        /* Add listeners to the buttons */
        button_player_register.onClick.AddListener(() => StartCoroutine(OnSubmitRegister(apiURL, name_player.text, password_player.text)));
        button_player_login.onClick.AddListener(() => StartCoroutine(OnSubmitLogin(apiURL, name_player.text, password_player.text)));
    }

    /* Coroutine to send the register query 
        Params:
        - uri: URL of the API
        - name: Name of the player
        - password: Password of the player
        Returns:
        - IEnumerator
      */
    private IEnumerator OnSubmitRegister(string uri, string name, string password) {
        /* Create the request */
        UnityWebRequest webRequest = UnityWebRequest.Post(uri + "/api/jugador/registro", "{\"nombre\":\"" + name + 
        "\",\"clave\":\"" + password + "\"}", "application/json");

        /* Send the request */
        yield return webRequest.SendWebRequest();

        /* Check if the request was successful */
        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(webRequest.error);
        }
        else
        {
            /* Get the data from the response */
            string data = webRequest.downloadHandler.text;
            result = JsonUtility.FromJson<Result>(data);
        }
        
        /* Show the message of the response */
        Debug.Log(result.message);
    }

    /* Coroutine to send the login query 
        Params:
        - uri: URL of the API
        - name: Name of the player
        - password: Password of the player
        Returns:
        - IEnumerator
      */
    public IEnumerator OnSubmitLogin(string uri, string name, string password) {
        /* Create the request */
        UnityWebRequest webRequest = UnityWebRequest.Get(uri + "/api/jugador/inicio_sesion/" + name + "/" + password);

        /* Send the request */
        yield return webRequest.SendWebRequest();

        /* Define a variable for show a message */
        bool result = false;

        /* Check if the request was successful */
        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(webRequest.error);
            result = false;
        }
        else
        {
            /* Get the data from the response */
            string data = webRequest.downloadHandler.text;

            /* Process the data */
            result = ProcessResult(data);
        }

        /* Show the message of the response */
        if (result) {
            Debug.Log("Inicio de sesión exitoso");
        } else {
            Debug.Log("Inicio de sesión fallido, intente de nuevo");
        }
    }

    /* Process the data from the response 
        Params:
        - data: Data from the response
        Returns:
        - bool
      */
    private bool ProcessResult(string data)
    {
        /* Deserialize the data to format Result */
        result = JsonUtility.FromJson<Result>(data);

        /* If the result.code is SUCCESS do: */
        if (result.code == "SUCCESS")
        {
            /* Deserialize the data to format Players */
            players = JsonUtility.FromJson<Players>(data);

            /* Add the player to the PlayersManager */
            return playersManager.AddPlayer(players.players[0]);
        }
        else
        {
            /* Show the error message of the response and NOT deserialize the to players format */
            Debug.LogError(result.message);
            return false;
        }
    }
}

/* Class to deserialize the data of response in JSON format */
[System.Serializable]
public class Result 
{
    public string code;
    public string message;

}