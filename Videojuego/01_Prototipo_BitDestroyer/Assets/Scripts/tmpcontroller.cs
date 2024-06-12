using UnityEngine;
using TMPro;

public class UpdatePlayerNames : MonoBehaviour
{
    public TMP_Text firstPlayerText;
    public TMP_Text secondPlayerText;

    void Start()
    {
        // Update the text fields with the player names from PersistentData
        firstPlayerText.text = string.IsNullOrEmpty(PersistentData.FirstPlayerName) ? "No player available" : PersistentData.FirstPlayerName;
        secondPlayerText.text = string.IsNullOrEmpty(PersistentData.SecondPlayerName) ? "No second player" : PersistentData.SecondPlayerName;
    }
}
