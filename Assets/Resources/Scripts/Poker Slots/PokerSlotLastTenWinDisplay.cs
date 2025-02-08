using UnityEngine;
using TMPro;
public class PokerSlotLastTenWinDisplay : MonoBehaviour
{

    public TextMeshProUGUI GameIdText;
    public TextMeshProUGUI cardText;


    // Call this method to update the display
    public void SetLastTenWinData(string gameID, string card)
    {
        GameIdText.text = gameID;
        cardText.text = card;
    }
}
