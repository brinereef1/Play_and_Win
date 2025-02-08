using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Linq;
using System;
public class SpinTheWheelBetManager : MonoBehaviour
{
    //[Header("DialogBoxNumber Text")]
    //[SerializeField] TMP_Text dialogBoxNumber_Text;
    [Header("Bet Button")]
    public Button Bet_Button;
    [Header("InputFields")]
    public TMP_InputField black_InputField;
    public TMP_InputField white_InputField;
    public TMP_InputField red_InputField;

    [Header("Authentication Token")]
    private string AuthTok;

    [Header("Api Url")]
    private string spinTheWheel_CreateBetLiveUrl = "http://13.234.117.221:2556/api/v1/user/spinwheel_userbet";

    [Header("Script References")]
    SaveUserData svd = new SaveUserData();

    SpinTheWheelTimer stwTimer;
    //PowerBallBetHistoryManager powerBallBetHistoryManager;
    //PowerBallWalletManager powerBallWalletManager;

    SpinTheWheelWinHistoryManager stwWinHistoryManager;
    SpinTheWheelBetHistoryManager stwBetHistoryManager;
    SpinTheWheelWalletManager stwWalletManager;
    public TMP_Text responseText;
    //public GameObject buttonParent_powerBall;
    //private List<string> betNumbers = new List<string>();
    //public List<Button> buttons_powerball = new List<Button>();
    //public GameObject dialogBox;
    //public TMP_InputField moneyInputField;

    public void SetToken(string token)
    {
        AuthTok = token;
    }

    public string GetToken()
    {
        return AuthTok;
    }

    void Start()
    {
        //dialogBox.SetActive(false);
        stwTimer = FindFirstObjectByType<SpinTheWheelTimer>();

        stwWinHistoryManager = FindFirstObjectByType<SpinTheWheelWinHistoryManager>();
        stwBetHistoryManager = FindFirstObjectByType<SpinTheWheelBetHistoryManager>();
        stwWalletManager = FindFirstObjectByType<SpinTheWheelWalletManager>();
        Bet_Button.onClick.AddListener(OnSubmitBet);

        AuthTok = svd.GetSavedAuthToken();
        //InitializeButtonsOfPattiPanel();

    }
    //public void OnCloseDialogBox()
    //{
    //    dialogBox.SetActive(false);
    //}
    //void InitializeButtonsOfPattiPanel()
    //{
    //    Button[] childButtons = buttonParent_powerBall.GetComponentsInChildren<Button>();
    //    buttons_powerball.AddRange(childButtons);


    //    Debug.Log("Total Buttons Initialized: " + buttons_powerball.Count);

    //    for (int i = 0; i < buttons_powerball.Count; i++)
    //    {
    //        Button button = buttons_powerball[i];
    //        button.onClick.AddListener(() => OnButtonClick(button.name));
    //    }


    //}

    //public void OnButtonClick(string boxNumber)
    //{
    //    Debug.Log("BoxNo" + boxNumber);
    //    dialogBox.SetActive(true);
    //    dialogBoxNumber_Text.text = boxNumber;
    //    betNumbers.Add(boxNumber);
    //}

    public void OnSubmitBet()
    {
        // white inputField
        if (!string.IsNullOrEmpty(white_InputField.text))
        {
            int betAmount = int.Parse(white_InputField.text);

            if (betAmount > 0)
            {
                string catId = GetCategories("White");
                Debug.Log("Categories: " + catId);
                StartCoroutine(SendBetToServer(betAmount, stwTimer.GetGameId(), stwTimer.GetGameRoundIdGenerated(), catId));
            }
            else
            {
                Debug.Log("BetAmount shouldbe greater than 0.");
            }
        }
        else
        {
            Debug.LogWarning("Please enter a valid amount.");
        }

        // red inputField
        if (!string.IsNullOrEmpty(red_InputField.text))
        {
            int betAmount = int.Parse(red_InputField.text);

            if (betAmount > 0)
            {
                string catId = GetCategories("Red");
                Debug.Log("Categories: " + catId);
                StartCoroutine(SendBetToServer(betAmount, stwTimer.GetGameId(), stwTimer.GetGameRoundIdGenerated(), catId));
            }
            else
            {
                Debug.Log("BetAmount shouldbe greater than 0.");
            }
        }
        else
        {
            Debug.LogWarning("Please enter a valid amount.");
        }




        // black inputField
        if (!string.IsNullOrEmpty(black_InputField.text))
        {
            int betAmount = int.Parse(black_InputField.text);

            if (betAmount > 0)
            {
                string catId = GetCategories("Black");
                Debug.Log("Categories: " + catId);
                StartCoroutine(SendBetToServer(betAmount, stwTimer.GetGameId(), stwTimer.GetGameRoundIdGenerated(), catId));
            }
            else
            {
                Debug.Log("BetAmount shouldbe greater than 0.");
            }
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
            case "Red":
                category = "67628dddf4521eeb58b19f2a";
                break;
            case "Black":
                category = "67628dd8f4521eeb58b19f27";
                break;
            case "White":
                category = "67628dcff4521eeb58b19f24";
                break;           
            default:
                category = "";
                break;
        }

        return category;
    }
    IEnumerator SendBetToServer(int betAmount, string gameRoundId, string gameRoundIdgenerated, string categoryId)
    {
        Debug.Log("BetAmount = " + betAmount + " GameRoundId = " + gameRoundId + " GameRoundIdgenerated =" + gameRoundIdgenerated + "CategoryId : " + categoryId);

        if (AuthTok == null)
        {
            AuthTok = GetToken();
        }
        STWBet bet = new STWBet
        {
            betUnit = betAmount,
            gameRoundId = gameRoundId,
            gameRoundIdgenerated = gameRoundIdgenerated,
            categoryId = categoryId
        };

        STWBetData betData = new STWBetData()
        {
            bets = new List<STWBet> { bet }
        };

        string jsonData = JsonConvert.SerializeObject(betData);
        Debug.Log("Serialized JSON Data: " + jsonData);

        using (UnityWebRequest request = new UnityWebRequest(spinTheWheel_CreateBetLiveUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", AuthTok);
            request.SetRequestHeader("userType", "User");
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Bet data successfully sent to server: " + request.downloadHandler.text);
                ClearAllInputFields();
                ShowBetSentResponse("Bet Sent Successfully");
                
                stwWalletManager.GetWalletBalance();
                stwWinHistoryManager.WinHistoryButtonClick();
                stwBetHistoryManager.BetHistoryButtonClick();

            }
            else
            {
                Debug.LogError("Error sending bet data: " + request.downloadHandler.text);

            }

        }

    }

    private void ClearAllInputFields()
    {
        black_InputField.text = "";
        white_InputField.text = "";
        red_InputField.text = "";
    }

    public void ShowBetSentResponse(string responseText)
    {
        if (responseText != null)
        {
            this.responseText.text = responseText;
            StartCoroutine(HideResponse());
        }
    }

    IEnumerator HideResponse()
    {
        yield return new WaitForSeconds(2);
        responseText.text = "";
    }


}
[System.Serializable]
public class STWBet
{
    public int betUnit { get; set; }
    //public int betAmount { get; set; }
    public string gameRoundId { get; set; }
    public string gameRoundIdgenerated { get; set; }
    public string categoryId { get; set; }
}

[System.Serializable]
public class STWBetData
{
    public List<STWBet> bets { get; set; }
}