
/* 
- José Manuel García Zumaya A01784238
- 02/06/2024

- Description:
    This script is used to deserialize the response of the API to get the players and the player.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Players to deserialize the players */
[System.Serializable]
public class Players
{
    /* Code of the response of API */
    public string code;

    /* Message of the response of API */
    public List<Player> players;

}

/* Player deserialize to store the player */
[System.Serializable]
public class Player
{
    public int id;
    public string nombre;
    public int juegos_jugados;
    public int juegos_ganados;
    public string clave;

}

