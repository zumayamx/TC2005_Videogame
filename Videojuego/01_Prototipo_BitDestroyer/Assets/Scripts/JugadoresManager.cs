

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JugadoresManager : MonoBehaviour
{
    public static JugadoresManager jugadoresManager;
    public Jugadores jugadoresList;

    private void Start() {
         if (jugadoresManager == null) {
            jugadoresManager = this;
            DontDestroyOnLoad(this.gameObject);
         } else {
            Destroy(this.gameObject);
         }
    }

    public bool AddPlayer(Jugador jugador) {

        if (jugadoresList.jugadores.Count == 0) {
            PlayerPrefs.SetInt("loginOne", 1);
            jugadoresList.jugadores.Add(jugador);
            return true;
        } 

        if (jugador.id != jugadoresList.jugadores[0].id) {
            PlayerPrefs.SetInt("loginTwo", 1);
            jugadoresList.jugadores.Add(jugador);
            return true;
        } 
        
        else {
            PlayerPrefs.SetInt("loginTwo", 0);
            Debug.Log("El jugador ya ha iniciado sesión, no se puede agregar nuevamente.");
            return false;
        }
    }
}
