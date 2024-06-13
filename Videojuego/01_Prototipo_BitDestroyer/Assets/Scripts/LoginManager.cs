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
    [SerializeField] private string apiURL = "http://ec2-3-101-36-23.us-west-1.compute.amazonaws.com:3000";

    /* Result of the query */
    public Result result;

    /* Players object  to deserialize the response */
    public Players players;

    /* PlayersManager object to manage the players through the game  */
    public PlayersManager playersManager;    

    void Start()
    {
        Debug.Log("Start() called");
        /* Add listeners to the buttons */
        button_player_register.onClick.AddListener(() => {
            Debug.Log("Register button clicked");
            StartCoroutine(OnSubmitRegister(apiURL, name_player.text, password_player.text));
        });
        button_player_login.onClick.AddListener(() => {
            Debug.Log("Login button clicked");
            StartCoroutine(OnSubmitLogin(apiURL, name_player.text, password_player.text));
        });
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
        Debug.Log("OnSubmitRegister() called with uri: " + uri + ", name: " + name + ", password: " + password);
        /* Create the request */
        UnityWebRequest webRequest = UnityWebRequest.Post(uri + "/api/jugador/registro", "{\"nombre\":\"" + name + 
        "\",\"clave\":\"" + password + "\"}", "application/json");

        Debug.Log("Register URL: " + uri + "/api/jugador/registro");
        Debug.Log("Register Payload: " + "{\"nombre\":\"" + name + "\",\"clave\":\"" + password + "\"}");

        /* Send the request */
        yield return webRequest.SendWebRequest();

        /* Check if the request was successful */
        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Register URL: " + uri + "/api/jugador/registro");
            Debug.LogError("Register Error: " + webRequest.error);
        }
        else
        {
            /* Get the data from the response */
            string data = webRequest.downloadHandler.text;
            Debug.Log("Register Response: " + data);
            result = JsonUtility.FromJson<Result>(data);
        }
        
        /* Show the message of the response */
        Debug.Log("Register Result Message: " + result.message);
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
        Debug.Log("OnSubmitLogin() called with uri: " + uri + ", name: " + name + ", password: " + password);
        /* Create the request */
        UnityWebRequest webRequest = UnityWebRequest.Get(uri + "/api/jugador/inicio_sesion/" + name + "/" + password);

       

        /* Send the request */
        yield return webRequest.SendWebRequest();

        /* Define a variable for show a message */
        bool result = false;

        /* Check if the request was successful */
        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Login URL: " + uri + "/api/jugador/inicio_sesion/" + name + "/" + password);
            Debug.LogError("Login Error: " + webRequest.error);
            result = false;
        }
        else
        {
            /* Get the data from the response */
            string data = webRequest.downloadHandler.text;
            Debug.Log("Login Response: " + data);

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
        Debug.Log("ProcessResult() called with data: " + data);
        /* Deserialize the data to format Result */
        result = JsonUtility.FromJson<Result>(data);

        /* If the result.code is SUCCESS do: */
        if (result.code == "SUCCESS")
        {
            Debug.Log("Result code SUCCESS");
            /* Deserialize the data to format Players */
            players = JsonUtility.FromJson<Players>(data);
            Debug.Log("Players deserialized, count: " + players.players.Count);

            /* Add the player to the PlayersManager */
            return playersManager.AddPlayer(players.players[0]);
        }
        else
        {
            /* Show the error message of the response and NOT deserialize the to players format */
            Debug.LogError("Error message: " + result.message);
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
