/*
- 11/06/2024

- Description:
    This script is used to manage the send of the cards played to the server.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CardSendManager : MonoBehaviour
{
    /* URL of the API */
    [SerializeField] string apiURL = "http://ec2-3-101-36-23.us-west-1.compute.amazonaws.com:3000";

    public List<(Card, int)> cardsPlayed = new List<(Card, int)>();

    ResultCard result;
    public void addCard(Card card, int id_player) {
        Debug.Log("Card into the list");
        Debug.Log(card.id);
        Debug.Log(id_player);
        cardsPlayed.Add((card, id_player));
        Debug.Log("card added");
    }

    public void sendCards(int id_match) {
        Debug.Log("Sending cards");
        foreach ((Card, int) card in cardsPlayed) {
            Debug.Log("Sending card");
            Debug.Log(card.Item1.id);
            StartCoroutine(OnSubmitCard(apiURL, card.Item1, card.Item2, id_match));
        }
    }

    private IEnumerator OnSubmitCard(string uri, Card card, int id_player, int id_match) {
       /* Create request */
       UnityWebRequest webRequest = UnityWebRequest.Post(uri + "/api/carta/jugada", 
                      "{\"id_partida\":" + id_match + "," +
                      "\"id_jugador\":" + id_player + "," +
                      "\"id_carta\":" + card.id + "}", "application/json");
            
         /* Send request */
        yield return webRequest.SendWebRequest();

        /* Check for errors */
       if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(webRequest.error);
            Debug.Log("ON ERROR OF CARD SEND");
        }
        else
        {
            /* Get the data from the response */
            string data = webRequest.downloadHandler.text;
            result = JsonUtility.FromJson<ResultCard>(data);
             /* Show the message of the response */
            Debug.Log(result.code);
         }
    }
}

public class ResultCard
{
    public string code;
}