using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

// This class is responsible for connecting to the API and fetching the card data
// It uses UnityWebRequest to make a GET request to the API
public class APIConnection : MonoBehaviour
{
    // The following variables are used to connect to the API
    [SerializeField] string apiURL = "http://ec2-3-101-36-23.us-west-1.compute.amazonaws.com:3000";
    [SerializeField] string cardEndpoint = "/api/cards";

    // This class is used to store the card data
    public Cards cards;

    public void Start()
    {
        // In order to fetch the card data, we need to use a coroutine
        // This is because UnityWebRequest is asynchronous
        StartCoroutine(GetCards());
    }

    
    IEnumerator GetCards() {

        UnityWebRequest www = UnityWebRequest.Get(apiURL + cardEndpoint);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success) {
            Debug.Log($"Request failed: {www.error}");
        } else {
            string data = www.downloadHandler.text;

            cards = JsonUtility.FromJson<Cards>(data);
            Debug.Log("connected");
        }
    }
}