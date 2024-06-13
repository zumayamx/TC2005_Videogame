/*


- Description:
    This script is used to change the scene to login and set the game mode, 
    use the playerPrefs to save the game mode.

    gameMode = 2 -> Multiplayer
    gameMode = 1 -> One player
*/

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    /* Button to select one player mode */
    public Button button_one_player;

    /* Button to select multiplayer mode */
    public Button button_multiplayers;

    public SceneTransition sceneTransition; // Referencia al SceneTransition

    public float waitTime = 1f;

    void Start()
    {
        /* Add listeners to the buttons */
        button_one_player.onClick.AddListener(() => StartCoroutine(ChangeSceneByName("Login", 1)));
        button_multiplayers.onClick.AddListener(() => StartCoroutine(ChangeSceneByName("Login", 2)));
    }

    /* Change to the scene by name 
        Params: 
        - sceneName: Name of the scene
        - modoJuego: Game mode
    */
    private IEnumerator ChangeSceneByName(string sceneName, int modoJuego)
    {
        /* Set the game mode */
        PlayerPrefs.SetInt("gameMode", modoJuego - 1);

        /* Wait for the specified time */
        yield return new WaitForSeconds(waitTime);

        /* Change the scene */
        if (sceneTransition != null)
        {
            sceneTransition.LoadScene(sceneName);
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    /* Change to the scene by index 
        Params: 
        - sceneIndex: Index of the scene
        - modoJuego: Game mode
    */
    private IEnumerator ChangeSceneByIndex(int sceneIndex, int modoJuego)
    {
        /* Set the game mode */
        PlayerPrefs.SetInt("gameMode", modoJuego);

        /* Wait for the specified time */
        yield return new WaitForSeconds(waitTime);

        /* Change the scene */
        SceneManager.LoadScene(sceneIndex);
    }
}