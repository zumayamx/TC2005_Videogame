
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSlotManager : MonoBehaviour
{
    //public GameObject panelRoulette;
    [SerializeField] private Transform[] cardSlots; // List of transforms indicating the positions of card slots
    [SerializeField] private float xOffset = 1.0f; // Offset for the projectile's x position

    private GameObject selectedCard; // Reference to the selected card
    private Vector3 originalPosition; // Original position of the selected card
    private bool isDragging = false; // Flag to indicate if a card is being dragged

    private void Start() {
        //panelRoulette.SetActive(false);
    }

    void Update()
    {
        // Check for mouse click
        if (Input.GetMouseButtonDown(0))
        {
            // Cast a ray from the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Check if the ray hits any object
            if (Physics.Raycast(ray, out hit))
            {
                // Check if the hit object is a card
                if (hit.collider.CompareTag("Card"))
                {
                    selectedCard = hit.collider.gameObject; // Set the selected card
                    originalPosition = selectedCard.transform.position; // Record the original position
                    isDragging = true; // Start dragging the card
                    Debug.Log("Selected Card: " + selectedCard.name);
                }
            }
        }

        // Check if dragging a card
        if (isDragging)
        {
            // Update the position of the selected card to follow the mouse only in the x and y axes
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10; // Distance from the camera
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            worldPos.z = selectedCard.transform.position.z; // Retain the original z position
            selectedCard.transform.position = worldPos;
        }

        // Check if releasing the left mouse button
        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            Debug.Log("Mouse Button Released");

            bool snapped = false; // Flag to indicate if the card snapped to a slot

            // Check if the selected card is within the area of any card slot
            foreach (Transform slot in cardSlots)
            {
                if (IsWithinArea(selectedCard.transform, slot))
                {
                    // Snap the card to the position of the card slot in the x and y axes
                    Vector3 snapPos = slot.position;
                    snapPos.z = originalPosition.z; // Retain the original z position
                    selectedCard.transform.position = snapPos;

                    // Rotate the card 90 degrees
                    selectedCard.transform.Rotate(0, 0, 270);
                    snapped = true;
                    Debug.Log("Card Snapped to Slot: " + slot.name);



                    // Check for the DefenseCard script
                    // DefenseCard defenseCard = selectedCard.GetComponent<DefenseCard>();
                    // if (defenseCard != null)
                    // {
                    //     // If it's a defense card, do nothing
                    //     Debug.Log("Defense Card - No Action Taken");
                    // }
                    // else 
                    // {
                    //     // Check for the AttackCard script
                    //     AttackCard attackCard = selectedCard.GetComponent<AttackCard>();
                    //     if (attackCard != null)
                    //     {
                    //         // If it's an attack card, set onOff to true
                    //         attackCard.startShooting = true;
                    //         Debug.Log("Attack Card onOff set to true");
                    //     }
                    // }
                    // Obtener el script de DefenseCard y AttackCard de la carta seleccionada
                    DefenseCard defenseCard = selectedCard.GetComponent<DefenseCard>();
                    AttackCard attackCard = selectedCard.GetComponent<AttackCard>();
                    BootcampCard_25 bootcampCard_25 = selectedCard.GetComponent<BootcampCard_25>();
                    BootcampCard_06 bootcampCard_06 = selectedCard.GetComponent<BootcampCard_06>();
                    BootcampCard_29 botcampCard_29 = selectedCard.GetComponent<BootcampCard_29>();
                    BootcampCard_22 bootcampCard_22 = selectedCard.GetComponent<BootcampCard_22>();
                    BootcampCard_03 bootcampCard_03 = selectedCard.GetComponent<BootcampCard_03>();

                    switch (defenseCard, attackCard, bootcampCard_25, bootcampCard_06, botcampCard_29, bootcampCard_22, bootcampCard_03)
                    {
                        case (DefenseCard _, null, null, null, null, null, null):
                            // Si es una carta de defensa, no hacer nada
                            Debug.Log("Defense Card - No Action Taken");
                            break;
                        case (null, AttackCard ac, null, null, null, null, null):
                            // Si es una carta de ataque, activar el disparo
                            ac.startShooting = true;
                            Debug.Log("Attack Card onOff set to true");
                            break;
                        case (null, null, BootcampCard_25 bc, null, null, null, null):
                            // Si es la carta bootcamp tipo 25 activa su panel en el cual esta la ruleta
                            Debug.Log("Bootcamp Card - ACTIVE PANEL ROULETTE");
                            bc.activePanelRoulette = true;
                            break;
                        case (null, null, null, BootcampCard_06 bc, null, null, null):
                            // Si es la carta bootcamp tipo 06 activa el script para dar 3 puntos de vida
                            Debug.Log("Bootcamp Card - ACTIVE HEALTH");
                            bc.activateHealth = true;
                            break;
                        case (null, null, null, null, BootcampCard_29 bc, null, null):
                            // Si es la carta bootcamp tipo 29 activa el script para ocultar las cartas de defensa
                            Debug.Log("Bootcamp Card - ACTIVE HIDE DEFENSE");
                            bc.activateHide = true;
                            break;
                        case (null, null, null, null, null, BootcampCard_22 bc, null):
                            // Si es la carta bootcamp tipo 22 activa el script para mostrar las cartas del oponente
                            Debug.Log("Bootcamp Card - ACTIVE SHOW CARDS");
                            bc.desactivateHide = true;
                            break;
                        case (null, null, null, null, null, null, BootcampCard_03 bc):
                            // Si es la carta bootcamp tipo 03 activa el script para activar el panel de boost
                            Debug.Log("Bootcamp Card - ACTIVE BOOST");
                            bc.activateBoost = true;
                            break;
                        default:
                            // Este caso no debería ocurrir, pero puedes manejarlo si es necesario
                            Debug.Log("Error: No se encontró un script válido en la carta seleccionada.");
                            break;
                    }

                    break; // Exit the loop if a slot is found
                }
            }

            // If the card was not snapped to a slot, return it to its original position
            if (!snapped)
            {
                selectedCard.transform.position = originalPosition;
                Debug.Log("Card Returned to Original Position");
            }

            // Reset dragging state and selected card
            isDragging = false;
            selectedCard = null;
        }
    }

    // Method to check if a card is within the area of a card slot
    private bool IsWithinArea(Transform card, Transform slot)
    {
        // Calculate the distance between the card and the slot in the x and y axes only
        Vector3 cardPosition = new Vector3(card.position.x, card.position.y, 0);
        Vector3 slotPosition = new Vector3(slot.position.x, slot.position.y, 0);
        float distance = Vector3.Distance(cardPosition, slotPosition);

        // Check if the distance is less than a threshold value (adjust as needed)
        return distance < 1f;
    }
}
