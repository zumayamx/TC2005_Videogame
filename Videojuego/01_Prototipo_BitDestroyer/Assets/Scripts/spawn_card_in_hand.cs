using UnityEngine;
using System.IO;
using System.Collections.Generic;
using TMPro;

public class CardSpawner : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab; // The same prefab for all decks
    [SerializeField] private Transform handPosition;

    [SerializeField] private GameObject object1; // Cibersecurity deck
    [SerializeField] private GameObject object2; // Bootcamp deck
    [SerializeField] private GameObject object3; // Ciberattack deck

    [SerializeField] private Transform[] spawnPositions; // Array of spawn positions for the cards
    public GameObject attackProjectilePrefab; // The prefab for the attack projectiles
    public string direction; // 'b' for right, 'r' for left

    private int cardCount = 0; // Tracks the number of spawned cards
    public int energy = 3; // Default energy
    public TMP_Text energyText; // TMP_Text to display the energy value

    private List<int> cibersecurityIds = new List<int> { 1, 4, 9, 10, 12, 15, 23, 26, 28 };
    private List<int> bootcampIds = new List<int> { 3, 6, 22, 25, 29, 30, 31 };
    private List<int> ciberattackIds = new List<int> { 2, 5, 7, 8, 11, 13, 14, 16, 17, 18, 24, 27 };

    public static List<CardInfo> spawnedCards = new List<CardInfo>(); // Global list to store spawned cards with their associated values

    private void Start()
    {
        UpdateEnergyText();
    }

    private void Update()
    {
        // Check for mouse click
        if (Input.GetMouseButtonDown(0))
        {
            // Cast a ray from the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Check if the ray hits any of the 3D objects
            if (Physics.Raycast(ray, out hit))
            {
                // Spawn a card based on the object hit
                if (hit.collider.gameObject == object1)
                {
                    TrySpawnCard("cibersecurity", cibersecurityIds, "defense", 1); // Defense card
                }
                else if (hit.collider.gameObject == object2)
                {
                    TrySpawnCard("bootcamp", bootcampIds, "none", 2); // Bootcamp card
                }
                else if (hit.collider.gameObject == object3)
                {
                    TrySpawnCard("ciberattack", ciberattackIds, "attack", 3); // Attack card
                }
            }
        }
    }

    private void TrySpawnCard(string category, List<int> ids, string cardType, int energyCost)
    {
        if (energy < energyCost)
        {
            Debug.Log("Not enough energy to spawn card.");
            return;
        }

        // Generate a random card ID from the list
        int randomIndex = Random.Range(0, ids.Count);
        int randomId = ids[randomIndex];
        string imagePath = $"Assets/Sprites/cartas/{category}/{randomId}.png";

        // Check if hand position is occupied
        if (handPosition.childCount == 0)
        {
            SpawnCard(cardPrefab, handPosition, imagePath, randomId, cardType);
            energy -= energyCost; // Deduct energy
            UpdateEnergyText();
        }
        else if (cardCount < spawnPositions.Length)
        {
            int emptySpawnIndex = FindEmptySpawnIndex();
            if (emptySpawnIndex != -1)
            {
                SpawnCard(cardPrefab, spawnPositions[emptySpawnIndex], imagePath, randomId, cardType);
                energy -= energyCost; // Deduct energy
                UpdateEnergyText();
            }
            else
            {
                Debug.Log("All spawn positions are occupied. Cannot spawn more cards.");
            }
        }
        else
        {
            Debug.Log("Hand position is occupied and max cards are spawned. Cannot spawn card.");
        }
    }

    private int FindEmptySpawnIndex()
    {
        for (int i = 0; i < spawnPositions.Length; i++)
        {
            if (spawnPositions[i].childCount == 0)
            {
                return i;
            }
        }
        return -1;
    }

    private void SpawnCard(GameObject cardPrefab, Transform spawnPosition, string imagePath, int cardId, string cardType)
    {
        // Spawn the card at the specified position and parent it to that position
        GameObject newCard = Instantiate(cardPrefab, spawnPosition.position, Quaternion.identity, spawnPosition);

        // Apply the texture from the image path
        ApplyTextureToPrefab(newCard, imagePath);

        // Rotate the card 180 degrees around the Z axis
        newCard.transform.Rotate(0, 0, 180);

        // Get the card data from the APIConnection
        APIConnection apiConnection = FindObjectOfType<APIConnection>();
        if (apiConnection == null || apiConnection.cards == null || apiConnection.cards.cards == null)
        {
            Debug.LogError("APIConnection or card data not found.");
            return;
        }

        Card cardData = apiConnection.cards.cards.Find(card => card.id == cardId);
        if (cardData == null)
        {
            Debug.LogError("Card data not found for card ID: " + cardId);
            return;
        }

        // Assign the card value to the card's script
        CardInfo cardInfo = newCard.AddComponent<CardInfo>();
        cardInfo.cardValue = cardData.valor;

        // Add the appropriate script based on the card type
        switch (cardType)
        {
            case "defense":
                var defenseScript = newCard.AddComponent<DefenseCard>();
                if (defenseScript != null)
                {
                    defenseScript.collisionCount = cardData.valor; // Assign the valor to collisionCount
                }
                else
                {
                    Debug.LogError("DefenseCard script could not be added to the card.");
                }
                break;
            case "attack":
                var attackScript = newCard.AddComponent<AttackCard>();
                if (attackScript != null)
                {
                    attackScript.prefabToSpawn = attackProjectilePrefab;
                    attackScript.direction = direction;
                    attackScript.numberOfShots = cardData.valor; // Assign the valor to numberOfShots
                }
                else
                {
                    Debug.LogError("AttackCard script could not be added to the card.");
                }
                break;
            case "none":
                // Do nothing for Bootcamp cards
                break;
        }

        // Add the card to the global list
        spawnedCards.Add(cardInfo);

        // Update card count
        cardCount++;
    }

    private void ApplyTextureToPrefab(GameObject prefab, string imagePath)
    {
        // Load the image file data
        byte[] imageData = File.ReadAllBytes(imagePath);

        // Create a new Texture2D
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(imageData);

        // Create a new material with the Standard shader
        Material newMaterial = new Material(Shader.Find("Standard"));
        newMaterial.mainTexture = texture;

        // Apply the new material to the prefab
        Renderer prefabRenderer = prefab.GetComponent<Renderer>();
        if (prefabRenderer != null)
        {
            prefabRenderer.material = newMaterial;
        }
        else
        {
            // If the prefab has children, find all renderers and apply the material
            Renderer[] childRenderers = prefab.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in childRenderers)
            {
                renderer.material = newMaterial;
            }
        }
    }

    public void UpdateEnergyText()
    {
        if (energyText != null)
        {
            energyText.text = $"Energy: {energy}";
        }
    }
}

// A class to hold information about the spawned card
public class CardInfo : MonoBehaviour
{
    public int cardValue; // The value assigned to the card
}