using UnityEngine;
using TMPro;
public class SingleLastTenDisplay : MonoBehaviour
{
    public TextMeshProUGUI GameIdText;
    public TextMeshProUGUI winNoText;


    // Call this method to update the display
    public void SetLastTenWinData(string gameID, string card)
    {
        GameIdText.text = gameID;
        winNoText.text = card;
    }
}
