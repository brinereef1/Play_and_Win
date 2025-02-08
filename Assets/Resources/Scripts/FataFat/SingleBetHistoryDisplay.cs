using UnityEngine;
using TMPro;

public class SingleBetHistoryDisplay : MonoBehaviour
{
     public TextMeshProUGUI betAmountText;
     public TextMeshProUGUI GameIdText;
     public TextMeshProUGUI CategoryText;
    // Call this method to update the display
    public void SetBetData(int betAmount, string gameId,string category)
    {
        betAmountText.text = betAmount.ToString();
        GameIdText.text = gameId.ToString();
        CategoryText.text = category.ToString();
    }
}
