using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SceneChangerLogin : MonoBehaviour
{
    public Button next;
    public Button back;
    // Start is called before the first frame update
    void Start()
    {
        next.onClick.AddListener(() => ToPlayGame());
        back.onClick.AddListener(() => ToModeElection());
    }

    private void ToPlayGame()
    {
        if (PlayerPrefs.GetInt("modoJuego") == 1)
        {
            PlayerPrefs.SetInt("modoJuego", 0);
            SceneManager.LoadScene("InicioSesion");
        }
        else
        {
             SceneManager.LoadScene("SampleScene");
        }
    }

    private void ToModeElection()
    {
        SceneManager.LoadScene("EleccionModo");
    }
}
