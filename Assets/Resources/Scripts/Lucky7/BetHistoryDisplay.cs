using UnityEngine;
using TMPro;
public class BetHistoryDisplay : MonoBehaviour
{
    public TextMeshProUGUI betAmountText;
    public TextMeshProUGUI GameIdText;
    public TextMeshProUGUI categoryNameText;

    // Call this method to update the display
    public void SetWinData(int betAmount, string gameId,string category)
    {
        betAmountText.text = betAmount.ToString();
        GameIdText.text = gameId.ToString();
        categoryNameText.text = category.ToString();
    }
}
