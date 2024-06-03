
/* 
- José Manuel García Zumaya A01784238
- 02/06/2024

- Description:
    This script is used to change the scene to login and set the game mode, 
    use the playerPrefs to save the game mode.

    gameMode = 2 -> Multiplayer
    gameMode = 1 -> One player
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    /* Button to select one player mode */
    public Button button_one_player;

    /* Button to select multiplayer mode */
    public Button button_multiplayers;

    void Start()
    {
        /* Add listeners to the buttons */
        button_one_player.onClick.AddListener(() => ChangeSceneByName("Login", 1));
        button_multiplayers.onClick.AddListener(() => ChangeSceneByName("Login", 2));
    }

    /* Change to the scene by name 
        Params: 
        - sceneName: Name of the scene
        - modoJuego: Game mode
    */
    private void ChangeSceneByName(string sceneName, int modoJuego)
    {
        /* Set the game mode */
        PlayerPrefs.SetInt("gameMode", modoJuego - 1);

        /* Change the scene */
        SceneManager.LoadScene(sceneName);
    }

    /* Change to the scene by index 
        Params: 
        - sceneIndex: Index of the scene
        - modoJuego: Game mode
    */
    private void ChangeSceneByIndex(int sceneIndex, int modoJuego)
    {
        /* Set the game mode */
        PlayerPrefs.SetInt("gameMode", modoJuego);

        /* Change the scene */
        SceneManager.LoadScene(sceneIndex);
    }
}