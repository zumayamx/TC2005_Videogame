using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Jugadores
{
    public string code;
    public List<Jugador> jugadores;

}

[System.Serializable]
public class Jugador
{
    public int id;
    public string nombre;
    public int juegos_jugados;
    public int juegos_ganados;
    public string clave;

}

