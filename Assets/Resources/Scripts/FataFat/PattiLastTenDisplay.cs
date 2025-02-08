using UnityEngine;
using TMPro;
public class PattiLastTenDisplay : MonoBehaviour
{
    public TextMeshProUGUI GameIdText;
    public TextMeshProUGUI winNoText;


    // Call this method to update the display
    public void SetLastTenWinData(string gameID, string winNo)
    {
        GameIdText.text = gameID;
        winNoText.text = winNo;
    }
}
