using UnityEngine;
using System.IO;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class CardSpawner : MonoBehaviour
{
    /* Hide defense cards objects that contains the script */
    public GameObject hideCardsBlue;
    public GameObject hideCardsRed;

    /* Panel to show the cards to choose */
    public GameObject panelElectionCards;

    /* Panel to show the roulette */
    public GameObject panelRoulette;

    /* Card buttons to choose one from any group */
    public Button cardOneElection;

    public Button cardTwoElection;

    public Button cardThreeElection;

    /* Update de image of energy */
    public Image energyImage;

    public Sprite energyImage_3;

    public Sprite energyImage_2;

    public Sprite energyImage_1;

    public Sprite energyImage_0;
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
        /* Desactivate the panel at the beggining of scene */
        panelElectionCards.SetActive(false);
        panelRoulette.SetActive(false);

        /* Desactivate the hide defense cards objects */
        hideCardsBlue.SetActive(false);
        hideCardsRed.SetActive(false);



        /* Add listener to the buttons to select one card */
        // cardOneElection.onClick.AddListener(() => {
        //     //function logic
        // });
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
                    ShowPanelElectionCards("cibersecurity", cibersecurityIds, "defense", 1);

                    //TrySpawnCard("cibersecurity", cibersecurityIds, "defense", 1); // Defense card
                }
                else if (hit.collider.gameObject == object2)
                {
                    ShowPanelElectionCards("bootcamp", bootcampIds, "none", 2);
                    //TrySpawnCard("bootcamp", bootcampIds, "none", 2); // Bootcamp card
                }
                else if (hit.collider.gameObject == object3)
                {
                    ShowPanelElectionCards("ciberattack", ciberattackIds, "attack", 3);
                    //TrySpawnCard("ciberattack", ciberattackIds, "attack", 3); // Attack card
                }
            }
        }
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
    private void ShowPanelElectionCards(string category, List<int> ids, string cardType, int energyCost)
    {   
        panelElectionCards.SetActive(true);

        if (energy < energyCost)
        {
            Debug.Log("Not enough energy to spawn card.");
            return;
        }

        // Generate a random card ID from the list
        int randomIndexOne = Random.Range(0, ids.Count);
        int randomIndexTwo = Random.Range(0, ids.Count);
        int randomIndexThree = Random.Range(0, ids.Count);

        int randomIdOne = ids[randomIndexOne];
        int randomIdTwo = ids[randomIndexTwo];
        int randomIdThree = ids[randomIndexThree];

        string imagePathOne = $"Assets/Sprites/cartas/{category}/{randomIdOne}.png";
        string imagePathTwo = $"Assets/Sprites/cartas/{category}/{randomIdTwo}.png";
        string imagePathThree = $"Assets/Sprites/cartas/{category}/{randomIdThree}.png";

        // Apply the image to the buttons
        cardOneElection.GetComponent<Image>().sprite = LoadNewSprite(imagePathOne);
        cardTwoElection.GetComponent<Image>().sprite = LoadNewSprite(imagePathTwo);
        cardThreeElection.GetComponent<Image>().sprite = LoadNewSprite(imagePathThree);

        // Remove old listeners
        cardOneElection.onClick.RemoveAllListeners();
        cardTwoElection.onClick.RemoveAllListeners();
        cardThreeElection.onClick.RemoveAllListeners();

        // Add listeners to the buttons
        cardOneElection.onClick.AddListener(() => TrySpawnCard(imagePathOne, randomIdOne, cardType, energyCost));
        cardTwoElection.onClick.AddListener(() => TrySpawnCard(imagePathTwo, randomIdTwo, cardType, energyCost));
        cardThreeElection.onClick.AddListener(() => TrySpawnCard(imagePathThree, randomIdThree, cardType, energyCost));

    }

    private void TrySpawnCard(string imagePath, int randomId, string cardType, int energyCost) {

        // Check if hand position is occupied /*-------------------*/
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
        } /* -------------------*/

        panelElectionCards.SetActive(false);
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

                /* Case for bootcamp cards */
            case "none":
                switch (cardData.id) {
                    case 25:
                        var bootcampScript25 = newCard.AddComponent<BootcampCard_25>();
                        bootcampScript25.roulettePanel = panelRoulette;
                        break;
                    case 6:
                        var bootcampScript6 = newCard.AddComponent<BootcampCard_06>();
                        break;
                    case 29:
                        var bootcampScript29 = newCard.AddComponent<BootcampCard_29>();
                        bootcampScript29.HideCardsBlue = hideCardsBlue;
                        bootcampScript29.HideCardsRed = hideCardsRed;
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

    public void UpdateEnergyText()
    {
        if (energyText != null)
        {
            /* Change the image by the energy value */
            if (energy == 3 ) {
                energyImage.sprite = energyImage_3;
            }

            if (energy == 2 ) {
                 energyImage.sprite = energyImage_2;
            }

            if (energy == 1 ) {
                 energyImage.sprite = energyImage_1;
            }

            if (energy == 0 ) {
                 energyImage.sprite = energyImage_0;
            }

            energyText.text = $"Energy: {energy}";
        }
    }
}

// A class to hold information about the spawned card
public class CardInfo : MonoBehaviour
{
    public int cardValue; // The value assigned to the card
}
