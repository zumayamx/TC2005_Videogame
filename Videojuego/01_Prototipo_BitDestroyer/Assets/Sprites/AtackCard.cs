using UnityEngine;

public class AttackCard : MonoBehaviour
{
    // Prefab to spawn
    public GameObject prefabToSpawn;

    // Offset for spawning (X axis only)
    public float spawnOffset = 3.0f;

    // Number of times to shoot
    public int numberOfShots = 5;

    // Public variable to start shooting
    public bool startShooting = false;

    // Time interval between shots
    public float shootInterval = 1.0f;

    // Direction: "r" for right, "b" for left
    public string direction = "r";

    // Counter for the number of shots fired
    private int shotsFired = 0;

    // Timer to track shooting intervals
    private float shootTimer = 0.0f;

    void Update()
    {
        if (startShooting)
        {
            shootTimer += Time.deltaTime;
            if (shootTimer >= shootInterval)
            {
                Shoot();
                shootTimer = 0.0f; // Reset the timer
            }
        }
    }

    void Shoot()
    {
        if (shotsFired < numberOfShots)
        {
            Vector3 spawnPosition = transform.position;

            // Determine the offset based on direction
            if (direction == "r")
            {
                spawnPosition += new Vector3(-spawnOffset, 0, 0);
            }
            else if (direction == "b")
            {
                spawnPosition += new Vector3(spawnOffset, 0, 0);
            }

            GameObject bullet = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

            // Pass the direction to the Projectile script
            Projectile projectile = bullet.GetComponent<Projectile>();
            if (projectile != null)
            {
                projectile.direction = direction == "b";
            }

            shotsFired++;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void BoostAttack(int boostAmount)
    {
        // Increase the number of shots
        numberOfShots += boostAmount;
    }
}
