using UnityEngine;
using System.IO;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class AI_Controller : MonoBehaviour
{
    /* Image to display the energy bar */
    public Image EnergyBar;

    /* Energy value */
    public int energy = 5;

    /* Objects that contains the script to boost a card */
    public GameObject boostManagerRed;
    public GameObject boostManagerBlue;

    /* Hide defense cards objects that contains the script */
    public GameObject hideCardsBlue;
    public GameObject hideCardsRed;

    /* Panel to show the roulette */
    public GameObject panelRoulette;
    public bool active = false;

    [SerializeField] private GameObject cardPrefab; // The same prefab for all decks
    [SerializeField] private Transform handPosition;

    [SerializeField] private GameObject object1; // Cibersecurity deck
    [SerializeField] private GameObject object2; // Bootcamp deck
    [SerializeField] private GameObject object3; // Ciberattack deck

    [SerializeField] private Transform[] spawnPositions; // Array of spawn positions for the cards
    public GameObject attackProjectilePrefab; // The prefab for the attack projectiles
    public string direction; // 'b' for right, 'r' for left

    private int cardCount = 0; // Tracks the number of spawned cards
    //public int energy = 3; // Default energy
    public TMP_Text energyText; // TMP_Text to display the energy value

    private List<int> cibersecurityIds = new List<int> { 1, 4, 9, 10, 12, 15, 23, 26, 28 };
    private List<int> bootcampIds = new List<int> { 3, 6, 22, 25, 29, 30, 31 };
    private List<int> ciberattackIds = new List<int> { 2, 5, 7, 8, 11, 13, 14, 16, 17, 18, 24, 27 };

    public static List<CardInfo> spawnedCards = new List<CardInfo>(); // Global list to store spawned cards with their associated values

    private void Start()
    {
        /* Desactivate the panel at the beggining of scene */
        panelRoulette.SetActive(false);

        /* Desactivate the hide defense cards objects */
        hideCardsBlue.SetActive(false);
        hideCardsRed.SetActive(false);

        /* Desactivate the boost cards objects */
        boostManagerRed.SetActive(false);
        boostManagerBlue.SetActive(false);

        UpdateEnergyBar();
    }

    private void Update()
    {
        AI();
    }

    // Método para cargar una nueva imagen como sprite
    private Sprite LoadNewSprite(string filePath)
    {
        if (File.Exists(filePath))
        {
            byte[] fileData = File.ReadAllBytes(filePath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(fileData);
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
        return null;
    }

    private void TrySpawnCard(string imagePath, int randomId, string cardType, int energyCost)
    {

        // Check if hand position is occupied /*-------------------*/
        if (handPosition.childCount == 0)
        {
            SpawnCard(cardPrefab, handPosition, imagePath, randomId, cardType);
            energy -= energyCost; // Deduct energy
            UpdateEnergyBar();
        }
        else if (cardCount < spawnPositions.Length)
        {
            int emptySpawnIndex = FindEmptySpawnIndex();
            if (emptySpawnIndex != -1)
            {
                SpawnCard(cardPrefab, spawnPositions[emptySpawnIndex], imagePath, randomId, cardType);
                energy -= energyCost; // Deduct energy
                UpdateEnergyBar();
            }
            else
            {
                Debug.Log("All spawn positions are occupied. Cannot spawn more cards.");
            }
        }
        else
        {
            Debug.Log("Hand position is occupied and max cards are spawned. Cannot spawn card.");
        } /* -------------------*/

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
                    attackScript.startShooting = true;
                    Debug.Log("Attack Card onOff set to true");
                }
                else
                {
                    Debug.LogError("AttackCard script could not be added to the card.");
                }
                break;

            /* Case for bootcamp cards */
            case "none":
                switch (cardData.id)
                {
                    /* The variables passed in this block of code are GameObjects that script needs to work,
                    don't pass variables to activate or desactivate something of this scripts, the charge script for that is
                    slot_script.cs */
                    case 25:
                        var bootcampScript25 = newCard.AddComponent<BootcampCard_25>();
                        bootcampScript25.roulettePanel = panelRoulette;
                        bootcampScript25.activePanelRoulette = true;
                        break;
                    case 6:
                        var bootcampScript6 = newCard.AddComponent<BootcampCard_06>();
                        bootcampScript6.activateHealth = true;
                        break;
                    case 29:
                        var bootcampScript29 = newCard.AddComponent<BootcampCard_29>();
                        bootcampScript29.HideCardsBlue = hideCardsBlue;
                        bootcampScript29.HideCardsRed = hideCardsRed;
                        bootcampScript29.activateHide = true;
                        break;
                    case 22:
                        var bootcampScript22 = newCard.AddComponent<BootcampCard_22>();
                        bootcampScript22.desactivateHide = true;
                        break;
                    case 3:
                        var bootcampScript3 = newCard.AddComponent<BootcampCard_03>();
                        bootcampScript3.boostManagerRed = boostManagerRed;
                        bootcampScript3.boostManagerBlue = boostManagerBlue;
                        bootcampScript3.activateBoost = true;
                        break;
                    case 31:
                        var bootcampScript31 = newCard.AddComponent<BootcampCard_31>();
                        bootcampScript31.newTurn = true;
                        break;
                }
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

    public void UpdateEnergyBar()
    {
        if (EnergyBar != null)
        {
            EnergyBar.fillAmount = (float)energy / 5;
            energyText.text = "Energy" + energy.ToString();
        }
    }

    private int[] GetRandomIndices(int upperRange)
    {
        List<int> indices = new List<int>();

        while (indices.Count < 4)
        {
            int randomIndex = Random.Range(0, upperRange);
            if (!indices.Contains(randomIndex))
            {
                indices.Add(randomIndex);
            }
        }

        return indices.ToArray();
    }

    public void AI()
    {
        if (energy > 0)
        {
            // Prioritize placing defense cards in the first three slots
            for (int i = 0; i < 3; i++)
            {
                if (spawnPositions[i].childCount == 0 || spawnPositions[i].GetComponentInChildren<DefenseCard>() == null && energy >= 1)
                {
                    int randomDefenseId = GetRandomCardId(cibersecurityIds);
                    string defenseImagePath = $"Assets/Sprites/cartas/cibersecurity/{randomDefenseId}.png";
                    SpawnCard(cardPrefab, spawnPositions[i], defenseImagePath, randomDefenseId, "defense");
                    transform.Rotate(0, 0, 90);
                    energy -= 1;
                    UpdateEnergyBar();
                    return; // End the turn after spawning a defense card
                }
            }

            // If the first three slots have defense cards, try to place attack cards
            for (int i = 0; i < 3; i++)
            {
                if (spawnPositions[i].childCount != 0 && energy >= 3)
                {
                    int random_id = UnityEngine.Random.Range(1, 3);
                    Debug.Log(random_id);
                    int randomAttackId = GetRandomCardId(ciberattackIds);
                    string attackImagePath = $"Assets/Sprites/cartas/ciberattack/{randomAttackId}.png";
                    SpawnCard(cardPrefab, spawnPositions[random_id], attackImagePath, randomAttackId, "attack");
                    energy -= 3;
                    UpdateEnergyBar();
                    return; // End the turn after spawning an attack card
                }
            }
        }
    }
    private int GetRandomCardId(List<int> cardIds)
    {
        System.Random rand = new System.Random();
        int randomIndex = rand.Next(cardIds.Count);
        return cardIds[randomIndex];
    }
}

