using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

public class VerificarJugador : MonoBehaviour
{
    public TMP_InputField name_player;
    public TMP_InputField password_player;
    public Button button_player_login;
    public Button button_player_register;

    [SerializeField] string apiURL = "";

    public Result result;
    public Jugadores jugadores;

    // Start is called before the first frame update
    void Start()
    {
        // Load persistent data if available
        if (!string.IsNullOrEmpty(PersistentData.NamePlayer))
        {
            name_player.text = PersistentData.NamePlayer;
        }

        if (!string.IsNullOrEmpty(PersistentData.PasswordPlayer))
        {
            password_player.text = PersistentData.PasswordPlayer;
        }

        if (!string.IsNullOrEmpty(PersistentData.ApiURL))
        {
            apiURL = PersistentData.ApiURL;
        }

        if (PersistentData.Result != null)
        {
            result = PersistentData.Result;
        }

        if (PersistentData.Jugadores != null)
        {
            jugadores = PersistentData.Jugadores;
        }

        button_player_register.onClick.AddListener(() => StartCoroutine(OnSubmitRegister(apiURL, name_player.text, password_player.text)));
        button_player_login.onClick.AddListener(() => StartCoroutine(OnSubmitLogin(apiURL, name_player.text, password_player.text)));
    }

    private IEnumerator OnSubmitRegister(string uri, string name, string password)
    {
        UnityWebRequest webRequest = new UnityWebRequest(uri + "/api/jugador/registro", "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes("{\"nombre\":\"" + name + "\",\"clave\":\"" + password + "\"}");
        webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");

        yield return webRequest.SendWebRequest();

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(webRequest.error);
        }
        else
        {
            string data = webRequest.downloadHandler.text;
            result = JsonUtility.FromJson<Result>(data);
            PersistentData.Result = result;
        }

        Debug.Log(result.message);
    }

    private IEnumerator OnSubmitLogin(string uri, string name, string password)
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(uri + "/api/jugador/inicio_sesion/" + name + "/" + password);

        yield return webRequest.SendWebRequest();

        bool loginSuccess = false;

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(webRequest.error);
        }
        else
        {
            string data = webRequest.downloadHandler.text;
            loginSuccess = ProcessResult(data);
        }

        if (loginSuccess)
        {
            Debug.Log("Inicio de sesión exitoso");
        }
        else
        {
            Debug.LogError("Inicio de sesión fallido");
        }
    }

    private bool ProcessResult(string data)
    {
        result = JsonUtility.FromJson<Result>(data);
        PersistentData.Result = result;

        if (result.code == "SUCCESS")
        {
            jugadores = JsonUtility.FromJson<Jugadores>(data);
            PersistentData.Jugadores = jugadores;

            if (jugadores.jugadores.Count > 0)
                PersistentData.FirstPlayerName = jugadores.jugadores[0].nombre;

            if (jugadores.jugadores.Count > 1)
                PersistentData.SecondPlayerName = jugadores.jugadores[1].nombre;

            // Save input data
            PersistentData.NamePlayer = name_player.text;
            PersistentData.PasswordPlayer = password_player.text;
            PersistentData.ApiURL = apiURL;

            return true;
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

[System.Serializable]
public class Jugadores
{
    public List<Jugador> jugadores;
}

[System.Serializable]
public class Jugador
{
    public string nombre;
    public string clave;
    // Add other fields as necessary
}
