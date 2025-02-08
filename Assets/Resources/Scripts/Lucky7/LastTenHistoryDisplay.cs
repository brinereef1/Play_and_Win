using UnityEngine;
using TMPro;
public class LastTenHistoryDisplay : MonoBehaviour
{
    public TextMeshProUGUI GameIdText;
    public TextMeshProUGUI cardText;
    public TextMeshProUGUI winNoText;


    // Call this method to update the display
    public void SetLastTenWinData(string gameID, string card, int winNo)
    {
        GameIdText.text = gameID;
        cardText.text = card;
        winNoText.text = winNo.ToString();
    }
}