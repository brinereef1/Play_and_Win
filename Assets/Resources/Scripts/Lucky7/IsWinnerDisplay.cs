using UnityEngine;
using TMPro;
public class IsWinnerDisplay : MonoBehaviour
{
    [SerializeField] public TMP_Text isWinText;
    [SerializeField] public TMP_Text winAmount;

    public void DisplayIsWinner(string message, int amount)
    {
        Debug.Log("Winer Displayed");
        if (message != "")
        {
            isWinText.text = message;
            winAmount.text = "Winning Amount is :"+amount;
        }
        else{
            Debug.Log("Message is null.");
        }
        
    }
}
