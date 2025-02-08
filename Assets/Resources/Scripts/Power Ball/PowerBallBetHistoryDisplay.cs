using UnityEngine;
using TMPro;
public class PowerBallBetHistoryDisplay : MonoBehaviour
{
    public TextMeshProUGUI betAmountText;
    public TextMeshProUGUI GameIdText;
    public TextMeshProUGUI CategoryText;
    public TextMeshProUGUI BetUnit;
    // Call this method to update the display
    public void SetBetData(int betAmount, string gameId, string category, int betUnit)
    {
        betAmountText.text = betAmount.ToString();
        GameIdText.text = gameId.ToString();
        CategoryText.text = category;
        BetUnit.text = betUnit.ToString();
    }

}

