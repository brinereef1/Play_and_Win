using UnityEngine;
using TMPro;
public class ThunderBallLastTenWinDisplay : MonoBehaviour
{

    public TextMeshProUGUI GameIdText;
    public TextMeshProUGUI ballNumberText;


    // Call this method to update the display
    public void SetLastTenWinData(string gameID, string ballNumber)
    {
        GameIdText.text = gameID;
        ballNumberText.text = ballNumber;
    }
}
