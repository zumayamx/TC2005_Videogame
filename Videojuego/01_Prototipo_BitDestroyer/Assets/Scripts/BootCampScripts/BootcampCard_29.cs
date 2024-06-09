/*
- José Manuel García Zumaya
- 05/06/2024

- Description:
    This script is used to activate the corresponden object depetending of the player turn.
    The object to activate is the one that contains the script to hide own  defense cards.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootcampCard_29 : MonoBehaviour
{
    public bool activateHide = false;
    private bool isBlueTurn = false;

    /* Objects that contains the script to hide the defense cards */
    public GameObject HideCardsBlue;

    public GameObject HideCardsRed;
    // Start is called before the first frame update
    void Start()
    {
        isBlueTurn = GameObject.Find("turn_manager").GetComponent<turn_manager>().blue_turn; //VERIFICAR QUE ESTO NO DE ERROR AL CAAMBIAR DE TURNOS
    }

    // Update is called once per frame
    void Update()
    {
        if (activateHide)
        {
            if (isBlueTurn)
            {
                HideCardsBlue.SetActive(true);
                HideCardsBlue.GetComponent<HideCardsBlue>().turnBegin = GameObject.Find("turn_manager").GetComponent<turn_manager>().turnCount;
            }
            else
            {
                HideCardsRed.SetActive(true);
                HideCardsRed.GetComponent<HideCardsRed>().turnBegin = GameObject.Find("turn_manager").GetComponent<turn_manager>().turnCount;
            }

            ToDestroy();
        }
    }
    private void ToDestroy()
    {
        Destroy(gameObject);
    }
}
