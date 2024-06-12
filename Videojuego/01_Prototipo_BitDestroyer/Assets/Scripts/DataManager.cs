using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Card 
{

   // The attributes of the card class must match the structure of the JSON data
   public int id;
   public string nombre;
   public string descripcion;
   public int tipoCarta;
   public int costoEnergia;
   public int efecto;
   public int valor;
}

[System.Serializable]
public class Cards
{
   public List<Card> cards;
}