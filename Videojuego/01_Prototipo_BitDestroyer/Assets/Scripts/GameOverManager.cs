/* José Manuel García Zumaya
- 11/06/2024

- Description:
    This script is used to manage send data of players to data base.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

public class GameOverManager : MonoBehaviour
{
    // URL of the API
    [SerializeField] string apiURL = "http://localhost:3000";

    // Variable to send data
    public bool sendData = false;

    // Players to submit
    public Player playerWinner;

    public Player playerDefeated;

    public bool playerWinnerIsBlue;

    public int id_partida;

    private GameObject cardSendManager;

    public string timeMatchGlobal;

    ResultMatch result;
    
    // Start is called before the first frame update
    void Start()
    {
         cardSendManager = GameObject.Find("turn_manager");

         if (sendData && playerWinnerIsBlue && playerWinner != null && playerDefeated != null) {
            int idPlayerBlue = playerWinner.id;
            int idPlayerRed = playerDefeated.id;
            int idPlayerWinner = playerWinner.id;
            int idPlayerDefeated = playerDefeated.id;
            string timeMatch = timeMatchGlobal;
            int matchWinsWinner = playerWinner.juegos_ganados + 1;
            int mathcPlayedDefeated = playerDefeated.juegos_jugados + 1;
            int matchPlayedWinner = playerWinner.juegos_jugados + 1;   
            sendData = false; 
            StartCoroutine(OnSubmitWinner(apiURL, idPlayerBlue, idPlayerRed, timeMatch, idPlayerWinner, idPlayerDefeated, matchWinsWinner, mathcPlayedDefeated, matchPlayedWinner));
        }

        if (sendData && !playerWinnerIsBlue && playerWinner != null && playerDefeated != null) {
            int idPlayerBlue = playerDefeated.id;
            int idPlayerRed = playerWinner.id;
            int idPlayerWinner = playerWinner.id;
            int idPlayerDefeated = playerDefeated.id;
            string timeMatch = timeMatchGlobal;
            int matchWinsWinner = playerWinner.juegos_ganados + 1;
            int mathcPlayedDefeated = playerDefeated.juegos_jugados + 1;
            int matchPlayedWinner = playerWinner.juegos_jugados + 1;
            sendData = false;
            StartCoroutine(OnSubmitWinner(apiURL, idPlayerBlue, idPlayerRed, timeMatch, idPlayerWinner, idPlayerDefeated, matchWinsWinner, mathcPlayedDefeated, matchPlayedWinner));
        }
    }

    /* Coroutine to send data of players and register in data base */
    private IEnumerator OnSubmitWinner(string uri, int id_player_blue, int id_player_red, string time_match, int id_player_winner, int id_player_defeated, 
    int match_wins_winner, int match_played_defeated, int match_played_winner) {
        /* Create the request */
        UnityWebRequest webRequest = UnityWebRequest.Post(uri + "/api/partida", 
        "{\"id_player_blue\":" + id_player_blue + "," +
                      "\"id_player_red\":" + id_player_red + "," +
                      "\"time_match\":\"" + time_match + "\"," +
                      "\"id_player_winner\":" + id_player_winner + "," +
                      "\"id_player_defeated\":" + id_player_defeated + "," +
                      "\"match_wins_winner\":" + match_wins_winner + "," +
                      "\"match_played_defeated\":" + match_played_defeated + "," +
                      "\"match_played_winner\":" + match_played_winner + "}", "application/json");
        
        yield return webRequest.SendWebRequest();

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(webRequest.error);
            Debug.Log("ON ERROR OF PLAYER SEND");
        }
        else
        {
            /* Get the data from the response */
            string data = webRequest.downloadHandler.text;
            result = JsonUtility.FromJson<ResultMatch>(data);
        }

        /* Show the message of the response and send the cards played in match */
        Debug.Log(result.code);
        id_partida = result.id_match;
        Debug.Log("ID PARTIDA");
        Debug.Log(id_partida);

        if (cardSendManager != null) {
             cardSendManager.GetComponent<CardSendManager>().sendCards(id_partida);
        } 
        else {
            Debug.Log("CardSendManager not found");
        }

    }
}

[System.Serializable]
public class ResultMatch
{
    public int code;
    public int id_match;
}