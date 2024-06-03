
/* 
- José Manuel García Zumaya A01784238
- 02/06/2024

- Description:
    This script is used to manage the players through the game, it uses the Players object (PlayersList) to store the players
    and the PlayersManager object to manage the players through the game.
 */
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersManager : MonoBehaviour
{

    /* PlayersManager object to manage the players through the game  */
    public static PlayersManager playersManager;

    /* Players object  to store the players */
    public Players playersList;
    private void Start() {
        /* Singleton to don't destroy this game object trough the game */
         if (playersManager == null) {
            playersManager = this;
            DontDestroyOnLoad(this.gameObject);
         } else {
            Destroy(this.gameObject);
         }
    }

    /* Add a player to the list of players
        Params:
        - player: Player to add
        Returns:
        - bool: If the player was added or not
     */
    public bool AddPlayer(Player player) {

        /* If the list is empty, add the player it means first player */
        if (playersList.players.Count == 0) {
            PlayerPrefs.SetInt("loginOne", 1);
            playersList.players.Add(player);
            return true;
        } 

        /* If the player is not the first player and is not the same player, add the second player */
        if (player.id != playersList.players[0].id) {
            PlayerPrefs.SetInt("loginTwo", 1);
            playersList.players.Add(player);
            return true;
        } 
        /* If is the same player don't add */
        else {
            PlayerPrefs.SetInt("loginTwo", 0);
            Debug.Log("El jugador ya ha iniciado sesión, no se puede agregar nuevamente.");
            return false;
        }
    }
}
