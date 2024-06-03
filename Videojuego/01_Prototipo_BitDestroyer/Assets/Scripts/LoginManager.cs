using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField name_player;
    public TMP_InputField password_player;

    public Button button_player_login;

    public Button button_player_register;

    [SerializeField] string apiURL = "http://localhost:3000";

    public Result result;

    public Players players;

    public PlayersManager playersManager;    

    void Start()
    {
        button_player_register.onClick.AddListener(() => StartCoroutine(OnSubmitRegister(apiURL, name_player.text, password_player.text)));
        button_player_login.onClick.AddListener(() => StartCoroutine(OnSubmitLogin(apiURL, name_player.text, password_player.text)));
    }

    private IEnumerator OnSubmitRegister(string uri, string name, string password) {
        UnityWebRequest webRequest = UnityWebRequest.Post(uri + "/api/jugador/registro", "{\"nombre\":\"" + name + "\",\"clave\":\"" + password + "\"}", "application/json");

        yield return webRequest.SendWebRequest();

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(webRequest.error);
        }
        else
        {
            string data = webRequest.downloadHandler.text;
            result = JsonUtility.FromJson<Result>(data);
        }

        Debug.Log(result.message);
    }

    private IEnumerator OnSubmitLogin(string uri, string name, string password) {
        UnityWebRequest webRequest = UnityWebRequest.Get(uri + "/api/jugador/inicio_sesion/" + name + "/" + password);

        yield return webRequest.SendWebRequest();

        bool result = false;

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(webRequest.error);
            result = false;
        }
        else
        {
            string data = webRequest.downloadHandler.text;
            result = ProcessResult(data);
        }

        if (result) {
            Debug.Log("Inicio de sesión exitoso");
        } else {
            Debug.Log("Inicio de sesión fallido, intente de nuevo");
        }
    }

    private bool ProcessResult(string data)
    {
        result = JsonUtility.FromJson<Result>(data);

        if (result.code == "SUCCESS")
        {
            players = JsonUtility.FromJson<Players>(data);
            return playersManager.AddPlayer(players.players[0]);
        }
        else
        {
            Debug.LogError(result.message);
            return false;
        }
    }
}

[System.Serializable]
public class Result 
{
    public string code;
    public string message;

}