using UnityEngine;

public class DefenseCard : MonoBehaviour
{
    // Public variable to track the number of collisions allowed
    public int collisionCount = 5;

    // Public variable to track if the card is hidden
    public bool isHide = false;

    // This method is called when the object collides with another collider
    private void OnCollisionEnter(Collision collision)
    {
        // Decrease the collision count by 1
        collisionCount--;

        // Check if the collision count has reached 0
        if (collisionCount <= 0)
        {
            // Destroy this game object
            Destroy(gameObject);
        }
    }
}
