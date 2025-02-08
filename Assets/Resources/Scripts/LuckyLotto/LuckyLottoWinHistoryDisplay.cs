using UnityEngine;
using TMPro;

public class LuckyLottoWinHistoryDisplay : MonoBehaviour
{
    public TextMeshProUGUI betAmountText;
    public TextMeshProUGUI winAmountText;
    public TextMeshProUGUI GameIdText;

    // Call this method to update the display
    public void SetWinData(int betAmount, int winAmount, string gameId)
    {
        betAmountText.text = betAmount.ToString();
        winAmountText.text = winAmount.ToString();
        GameIdText.text = gameId;
    }
}
