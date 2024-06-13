using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class CardSpawner : MonoBehaviour
{
    // SoundManagerMatch object
    public GameObject soundManagerMatch;
    /* Image to display the energy bar */
    public Image EnergyBar;
    
    /* Energy value */
    public int energy = 5;

    private int maxEnergy = 5;

    /* Objects that contains the script to boost a card */
    public GameObject boostManagerRed;
    public GameObject boostManagerBlue;

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

    /* Object to send card data */
    private GameObject cardSendManager;
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
    private List<int> bootcampIds = new List<int> { 3, 6, 22, 25, 29, 31 };
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

        /* Desactivate the boost cards objects */
        boostManagerRed.SetActive(false);
        boostManagerBlue.SetActive(false);

        /* Get the componet to send card info */
        cardSendManager = GameObject.Find("turn_manager");

        /* Get the sound manager */
        soundManagerMatch = GameObject.Find("SoundManager");

        UpdateEnergyBar();
    }

    private void Update()
    {
        // Periodically check if the spawn positions are occupied
        CheckSpawnPositions();

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
                    soundManagerMatch.GetComponent<SoundManagerMatch>().PlayButtonClickSound();
                    ShowPanelElectionCards("cibersecurity", cibersecurityIds, "defense", 1);

                    //TrySpawnCard("cibersecurity", cibersecurityIds, "defense", 1); // Defense card
                }
                else if (hit.collider.gameObject == object2)
                {
                    soundManagerMatch.GetComponent<SoundManagerMatch>().PlayButtonClickSound();
                    ShowPanelElectionCards("bootcamp", bootcampIds, "none", 2);
                    //TrySpawnCard("bootcamp", bootcampIds, "none", 2); // Bootcamp card
                }
                else if (hit.collider.gameObject == object3)
                {
                    soundManagerMatch.GetComponent<SoundManagerMatch>().PlayButtonClickSound();
                    ShowPanelElectionCards("ciberattack", ciberattackIds, "attack", 3);
                    //TrySpawnCard("ciberattack", ciberattackIds, "attack", 3); // Attack card
                }
            }
        }
    }  

    private Sprite LoadNewSprite(string resourcePath)
    {
        Sprite sprite = Resources.Load<Sprite>(resourcePath);
        if (sprite == null)
        {
            Debug.LogError("Sprite not found at path: " + resourcePath);
        }
        return sprite;
    }

    private void ShowPanelElectionCards(string category, List<int> ids, string cardType, int energyCost)
    {   
        // Check if there is enough energy to spawn the card
        if (energy < energyCost)
        {
            Debug.Log("Not enough energy to spawn card.");
            return;
        }

         panelElectionCards.SetActive(true);

        // Generate a random card index from the list
        int[] randomIndexes = GetRandomIndices(ids.Count);

        int randomIdOne = ids[randomIndexes[0]];
        int randomIdTwo = ids[randomIndexes[1]];
        int randomIdThree = ids[randomIndexes[2]];

        string imagePathOne = $"Sprites/cartas/{category}/{randomIdOne}";
        string imagePathTwo = $"Sprites/cartas/{category}/{randomIdTwo}";
        string imagePathThree = $"Sprites/cartas/{category}/{randomIdThree}";

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

        // Check if hand position is occupied
        if (!IsPositionOccupied(handPosition))
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
        }

        panelElectionCards.SetActive(false);
    }

    private int FindEmptySpawnIndex()
    {
        for (int i = 0; i < spawnPositions.Length; i++)
        {
            if (!IsPositionOccupied(spawnPositions[i]))
            {
                return i;
            }
        }
        return -1;
    }

    private bool IsPositionOccupied(Transform position)
    {
        float searchRadius = 0.5f; // Increase the search radius
        Collider[] hitColliders = Physics.OverlapSphere(position.position, searchRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Card") && Mathf.Abs(hitCollider.transform.position.z - position.position.z) < 0.1f)
            {
                return true;
            }
        }
        return false;
    }

    private void CheckSpawnPositions()
    {
        for (int i = 0; i < spawnPositions.Length; i++)
        {
            if (!IsPositionOccupied(spawnPositions[i]))
            {
                // Handle the case when a card leaves the spawn position
                // Decrease cardCount when a card leaves the spawn area
                cardCount--;
            }
        }
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

        bool IsBlueTurn;
        if (GameObject.Find("turn_manager").GetComponent<turn_manager>() != null) {
            IsBlueTurn = GameObject.Find("turn_manager").GetComponent<turn_manager>().blue_turn;
        } else {
            IsBlueTurn = GameObject.Find("turn_manager").GetComponent<AI_turn_manager>().blue_turn;
        }
        
        Debug.Log(IsBlueTurn);

        if (IsBlueTurn) {
            int id_player_blue = GameObject.Find("playersManager").GetComponent<PlayersManager>().playersList.players[0].id;
            Debug.Log(id_player_blue);

            if (cardSendManager == null) {
                cardSendManager = GameObject.Find("turn_manager");
                Debug.Log(cardSendManager);
                Debug.Log("CardSendManager encontrado");
            }
            
            cardSendManager.GetComponent<CardSendManager>().addCard(cardData, id_player_blue);
           
            Debug.Log("Carta añadida");
        }

        if (!IsBlueTurn) {
            int id_player_red = GameObject.Find("playersManager").GetComponent<PlayersManager>().playersList.players[1].id;
            cardSendManager.GetComponent<CardSendManager>().addCard(cardData, id_player_red);
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
                    /* The variables passed in this block of code are GameObjects that script needs to work,
                    don't pass variables to activate or desactivate something of this scripts, the charge script for that is
                    slot_script.cs */
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
                    case 22:
                        var bootcampScript22 = newCard.AddComponent<BootcampCard_22>();
                        break;
                    case 3:
                        var bootcampScript3 = newCard.AddComponent<BootcampCard_03>();
                        bootcampScript3.boostManagerRed = boostManagerRed;
                        bootcampScript3.boostManagerBlue = boostManagerBlue;
                        break;
                    case 31:
                        var bootcampScript31 = newCard.AddComponent<BootcampCard_31>();
                        break;
                }
                break;
        }

        // Add the card to the global list
        spawnedCards.Add(cardInfo);

        // Update card count
        cardCount++;
    }

    private void ApplyTextureToPrefab(GameObject prefab, string resourcePath)
    {
        Sprite sprite = Resources.Load<Sprite>(resourcePath);
        if (sprite == null)
        {
            Debug.LogError("Sprite not found at path: " + resourcePath);
            return;
        }

        // Create a new material with the Standard shader
        Material newMaterial = new Material(Shader.Find("Standard"));
        newMaterial.mainTexture = sprite.texture;

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
            EnergyBar.fillAmount = (float)energy / maxEnergy;
            energyText.text = energy.ToString();
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
}

// A class to hold information about the spawned card
public class CardInfo : MonoBehaviour
{
    public int cardValue; // The value assigned to the card
}
