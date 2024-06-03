using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Players
{
    public string code;
    public List<Player> players;

}

[System.Serializable]
public class Player
{
    public int id;
    public string nombre;
    public int juegos_jugados;
    public int juegos_ganados;
    public string clave;

}

