using UnityEngine;
using TMPro;
public class KoyelBetHistoryDisplay : MonoBehaviour
{
    public TextMeshProUGUI betAmountText;
    public TextMeshProUGUI GameIdText;

    // Call this method to update the display
    public void SetBetData(int betAmount, string gameId)
    {
        betAmountText.text = betAmount.ToString();
        GameIdText.text = gameId.ToString();
    }
}
