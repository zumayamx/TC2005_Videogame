

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersManager : MonoBehaviour
{
    public static PlayersManager playersManager;
    public Players playersList;

    private void Start() {
         if (playersManager == null) {
            playersManager = this;
            DontDestroyOnLoad(this.gameObject);
         } else {
            Destroy(this.gameObject);
         }
    }

    public bool AddPlayer(Player player) {

        if (playersList.players.Count == 0) {
            PlayerPrefs.SetInt("loginOne", 1);
            playersList.players.Add(player);
            return true;
        } 

        if (player.id != playersList.players[0].id) {
            PlayerPrefs.SetInt("loginTwo", 1);
            playersList.players.Add(player);
            return true;
        } 
        
        else {
            PlayerPrefs.SetInt("loginTwo", 0);
            Debug.Log("El jugador ya ha iniciado sesión, no se puede agregar nuevamente.");
            return false;
        }
    }
}
