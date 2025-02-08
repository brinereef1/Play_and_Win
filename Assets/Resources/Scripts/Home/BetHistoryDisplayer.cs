using TMPro;
using UnityEngine;

public class BetHistoryDisplayer : MonoBehaviour
{
    public TextMeshProUGUI betAmountText;
    public TextMeshProUGUI GameIdText;
    public TextMeshProUGUI CategoryText;
    public TextMeshProUGUI BetUnit;

    public void SetBetData(int betAmount, string gameId, string category,int betUnit)
    {
        betAmountText.text = betAmount.ToString();
        GameIdText.text = gameId.ToString();
        CategoryText.text = category.ToString();
        BetUnit.text = betUnit.ToString();

        if(BetUnit.text == "0")
        {
            BetUnit.text = "";
        }
    }
}
