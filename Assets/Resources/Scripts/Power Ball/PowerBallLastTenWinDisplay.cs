using UnityEngine;
using TMPro;
public class PowerBallLastTenWinDisplay : MonoBehaviour
{
    
    public TextMeshProUGUI GameIdText;
    public TextMeshProUGUI ballNumberText;


    // Call this method to update the display
    public void SetLastTenWinData(string gameID, string ballNumber)
    {

        GameIdText.text = gameID;

        if (ballNumber.Length == 1)
        {
            ballNumber = "0" + ballNumber;
        }

        ballNumberText.text = ballNumber.Substring(1);
    }
}
