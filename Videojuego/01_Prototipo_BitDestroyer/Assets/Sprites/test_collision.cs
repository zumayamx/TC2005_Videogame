using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 5f; // Speed of the projectile
    [SerializeField] private float lifeTime = 5f; // Time before the projectile is automatically destroyed
    public bool direction; // Direction attribute: true for left, false for right

    private void Start()
    {
        // Destroy the projectile after a certain time to prevent it from existing indefinitely
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        if (direction)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            //Debug.Log("going right");
        }
        else
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
            //Debug.Log("going left");
        }
        // Move the projectile based on its direction
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Destroy the projectile upon collision with any object
        Destroy(gameObject);
    }
}
