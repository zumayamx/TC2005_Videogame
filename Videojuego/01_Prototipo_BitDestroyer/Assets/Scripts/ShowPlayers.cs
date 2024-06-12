/* 
- José Manuel García Zumaya A01784238
- 02/06/2024

- Description:
   This script is used to show the players in the game scene, in addition the players
   profile to.
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ShowPlayers : MonoBehaviour
{

    /* Text to show the players name */
    public TMP_Text playerOne_Name;
    public TMP_Text playerTwo_Name;

    void Start()
    {
        playerOne_Name.text = GameObject.Find("playersManager").GetComponent<PlayersManager>().playersList.players[0].nombre;
        playerTwo_Name.text = GameObject.Find("playersManager").GetComponent<PlayersManager>().playersList.players[1].nombre;
    }
}
