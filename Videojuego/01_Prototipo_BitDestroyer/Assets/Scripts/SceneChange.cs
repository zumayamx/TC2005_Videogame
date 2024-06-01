using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{

    public Button button_one_player;
    public Button button_multiplayers;
    // Cambia a la escena especificada por nombre

    void Start()
    {
        button_one_player.onClick.AddListener(() => ChangeSceneByName("InicioSesion", 1));
        button_multiplayers.onClick.AddListener(() => ChangeSceneByName("InicioSesion", 2));
    }

    private void ChangeSceneByName(string sceneName, int modoJuego)
    {
        PlayerPrefs.SetInt("modoJuego", modoJuego - 1);
        SceneManager.LoadScene(sceneName);
    }

    // Cambia a la escena especificada por índice
    private void ChangeSceneByIndex(int sceneIndex, int modoJuego)
    {
        PlayerPrefs.SetInt("modoJuego", modoJuego);
        SceneManager.LoadScene(sceneIndex);
    }
}