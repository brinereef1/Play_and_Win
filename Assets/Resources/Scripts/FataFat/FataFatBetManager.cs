using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
public class FataFatBetManager : MonoBehaviour
{

    public GameObject buttonParent_single;
    public GameObject buttonParent_patti;
    public GameObject dialogBox;
    public List<Button> buttons_single = new List<Button>();
    public List<Button> buttons_patti = new List<Button>();
    public int totalButtons = 230;
    public int totalButtons_patti = 10;
    public TMP_InputField moneyInputField;
    private int totalBet = 0;
    private List<string> betNumbers = new List<string>();

    void Start()
    {
        dialogBox.SetActive(false);
        InitializeButtonsOfSinglePanel();
        InitializeButtonsOfPattiPanel();
        
    }

    void InitializeButtonsOfSinglePanel()
    {
        Button[] childButtons = buttonParent_single.GetComponentsInChildren<Button>();
        buttons_single.AddRange(childButtons);

            Debug.Log("Total Buttons Initialized: " + buttons_single.Count);

            for (int i = 0; i < buttons_single.Count; i++)
            {
                Button button = buttons_single[i];
                button.onClick.AddListener(() => OnButtonClick(button.name));
            }
        

    }

    void InitializeButtonsOfPattiPanel()
    {
        Button[] childButtons = buttonParent_patti.GetComponentsInChildren<Button>();
        buttons_patti.AddRange(childButtons);

        
            Debug.Log("Total Buttons Initialized: " + buttons_patti.Count);

            for (int i = 0; i < buttons_patti.Count; i++)
            {
                Button button = buttons_patti[i];
                button.onClick.AddListener(() => OnButtonClick(button.name));
            }
        

    }
    public void OnButtonClick(string boxNumber)
    {
        Debug.Log("You clicked On Button : " + boxNumber);

        dialogBox.SetActive(true);
        betNumbers.Add(boxNumber);
    }
    public void OnCloseDialogBox()
    {
        dialogBox.SetActive(false);
    }

    public void OnSubmitBet()
    {
        if (!string.IsNullOrEmpty(moneyInputField.text))
        {
            int betAmount = int.Parse(moneyInputField.text);
            totalBet += betAmount;
            Debug.Log(betNumbers.Count() + "Total Bet");
            if (betNumbers.Count() > 0)
            {
                string value = betNumbers.Last().ToString();
                Debug.Log(value);
                
                 string catId = GetCategories(value);
                
                Debug.Log("Categories: " + catId + " For: " + value);
                // StartCoroutine(SendBetToServer(betAmount, superRouletteTimer.GetGameId(), superRouletteTimer.GetGameRoundIdGenerated(), catId));
                

                Debug.Log("Value of betAmount" + betAmount);
                // Debug.Log("Category: " + catId + " for: " + value);
                Debug.Log("Tried to clear the bet..");
                betNumbers.Clear();
            }
            // UpdateTotalBetText();

            dialogBox.SetActive(false);
            moneyInputField.text = "";
        }
        else
        {
            Debug.LogWarning("Please enter a valid amount.");
        }
    }


     public string GetCategories(string boxNumber)
    {
        string category = "";
       switch (boxNumber)
{
    case "0":
        category = "6735a17ae9624f53dc30efb3";
        break;
    case "1":
        category = "6735a17de9624f53dc30efb6";
        break;
    case "2":
        category = "6735a180e9624f53dc30efb9";
        break;
    case "3":
        category = "6735a183e9624f53dc30efbc";
        break;
    case "4":
        category = "6735a186e9624f53dc30efbf";
        break;
    case "5":
        category = "6735a189e9624f53dc30efc2";
        break;
    case "6":
        category = "6735a18be9624f53dc30efc5";
        break;
    case "7":
        category = "6735a190e9624f53dc30efc8";
        break;
    case "8":
        category = "6735a193e9624f53dc30efcb";
        break;
    case "9":
        category = "6735a196e9624f53dc30efce";
        break;
    default:
        category = "";
        break;
}
        return category;
    }
}
