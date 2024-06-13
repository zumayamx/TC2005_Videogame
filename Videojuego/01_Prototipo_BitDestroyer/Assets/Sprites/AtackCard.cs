using UnityEngine;

public class AttackCard : MonoBehaviour
{
    // Prefab to spawn
    public GameObject prefabToSpawn;

    // Offset for spawning (X axis only)
    public float spawnOffset = 3.0f;
    public int ID;

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

    // Lists of IDs for each lane type
    private int[] fisicoIds = { 1, 14, 6, 27, 13 };
    private int[] wifiIds = { 11, 24, 7, 8 };
    private int[] radioIds = { 17, 18, 5, 16 };

    private bool shotsDeducted = false;

    void Update()
    {
        // Continuously check the lane
        CheckLane();

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

    void CheckLane()
    {
        float yPos = transform.position.y;

        if (!shotsDeducted)
        {
            if (IsFisicoID(ID))
            {
                if (yPos > 9 || yPos < 12)
                {
                    DeductShots();
                }
            }
            else if (IsWifiID(ID))
            {
                if (yPos > 6|| yPos < 9)
                {
                    DeductShots();
                }
            }
            else if (IsRadioID(ID))
            {
                if (yPos > 3 || yPos < 6)
                {
                    DeductShots();
                }
            }
        }
    }

    bool IsFisicoID(int id)
    {
        foreach (int fisicoId in fisicoIds)
        {
            if (fisicoId == id)
            {
                return true;
            }
        }
        return false;
    }

    bool IsWifiID(int id)
    {
        foreach (int wifiId in wifiIds)
        {
            if (wifiId == id)
            {
                return true;
            }
        }
        return false;
    }

    bool IsRadioID(int id)
    {
        foreach (int radioId in radioIds)
        {
            if (radioId == id)
            {
                return true;
            }
        }
        return false;
    }

    void DeductShots()
    {
        if (numberOfShots > 2)
        {
            numberOfShots -= 2;
            shotsDeducted = true; // Ensure shots are only deducted once
        }
    }

    public void BoostAttack(int boostAmount)
    {
        // Increase the number of shots
        numberOfShots += boostAmount;
    }
}
