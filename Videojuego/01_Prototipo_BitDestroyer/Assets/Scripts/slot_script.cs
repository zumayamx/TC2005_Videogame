
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSlotManager : MonoBehaviour
{
    [SerializeField] private Transform[] cardSlots; // List of transforms indicating the positions of card slots
    [SerializeField] private float xOffset = 1.0f; // Offset for the projectile's x position

    private GameObject selectedCard; // Reference to the selected card
    private Vector3 originalPosition; // Original position of the selected card
    private bool isDragging = false; // Flag to indicate if a card is being dragged

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
                    DefenseCard defenseCard = selectedCard.GetComponent<DefenseCard>();
                    if (defenseCard != null)
                    {
                        // If it's a defense card, do nothing
                        Debug.Log("Defense Card - No Action Taken");
                    }
                    else
                    {
                        // Check for the AttackCard script
                        AttackCard attackCard = selectedCard.GetComponent<AttackCard>();
                        if (attackCard != null)
                        {
                            // If it's an attack card, set onOff to true
                            attackCard.startShooting = true;
                            Debug.Log("Attack Card onOff set to true");
                        }
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
        return distance < 3f;
    }
}