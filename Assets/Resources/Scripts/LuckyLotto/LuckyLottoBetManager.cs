using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;
public class LuckyLottoBetManager : MonoBehaviour
{
    //[Header("Number Show Text")]
    //[SerializeField] TMP_Text number_show_text;

    //public GameObject dialogBox;
    //public TMP_InputField moneyInputField;
    //public GameObject buttonParent;

    //private List<string> betNumbers = new List<string>();
    [Header("BetButton")]
    public Button Bet_Button;

    [Header("InputFields")]
    public TMP_InputField set_inputField;
    public TMP_InputField pureSeq_inputField;
    public TMP_InputField seq_inputField;
    public TMP_InputField color_inputField;
    public TMP_InputField pair_inputField;
    public TMP_InputField high_inputField;
    public TMP_InputField low_inputField;

    
    public TMP_Text responseText;
    //public List<Button> buttons = new List<Button>();
    LuckyLottoTimer luckyLottoTimer;
    SaveUserData svd = new SaveUserData();
    private const string betApiUrl = "http://13.234.117.221:2556/api/v1/user/craeteuserbet";
    private string AuthTok;
    LuckyLottoBetHistoryManager luckyLottoBetHistoryManager;
    LuckyLottoWalletManager luckyLottoWalletManager;
    void Start()
    {
        //dialogBox.SetActive(false);
        luckyLottoTimer = FindFirstObjectByType<LuckyLottoTimer>();
        luckyLottoBetHistoryManager = FindFirstObjectByType<LuckyLottoBetHistoryManager>();
        luckyLottoWalletManager = FindFirstObjectByType<LuckyLottoWalletManager>();
        AuthTok = svd.GetSavedAuthToken();
        Bet_Button.onClick.AddListener(OnSubmitBet);
        //InitializeButtonsOfSinglePanel();
    }

    //public void OnButtonClick(string boxNumber)
    //{
    //    Debug.Log("BoxNo" + boxNumber);
    //    dialogBox.SetActive(true);
    //    number_show_text.text = boxNumber;
    //    betNumbers.Add(boxNumber);
    //}

    //void InitializeButtonsOfSinglePanel()
    //{
    //    Button[] childButtons = buttonParent.GetComponentsInChildren<Button>();
    //    buttons.AddRange(childButtons);

    //    Debug.Log("Total Buttons Initialized: " + buttons.Count);

    //    for (int i = 0; i < buttons.Count; i++)
    //    {
    //        Button button = buttons[i];
    //        button.onClick.AddListener(() => OnButtonClick(button.name));
    //    }
    //}

    public void OnSubmitBet()
    {
        // set
        if (!string.IsNullOrEmpty(set_inputField.text))
        {
            int betAmount = int.Parse(set_inputField.text);
         
            if (betAmount > 0)
            {             
                string catId = GetCategories("Set");               
                StartCoroutine(SendBetToServer(betAmount, luckyLottoTimer.GetGameId(), luckyLottoTimer.GetGameRoundIdGenerated(), catId));           
            }
        }
        else
        {
            Debug.LogWarning("Please enter a valid amount.");
        }

        // seq
        if (!string.IsNullOrEmpty(seq_inputField.text))
        {
            int betAmount = int.Parse(seq_inputField.text);

            if (betAmount > 0)
            {
                string catId = GetCategories("Seq");
                StartCoroutine(SendBetToServer(betAmount, luckyLottoTimer.GetGameId(), luckyLottoTimer.GetGameRoundIdGenerated(), catId));
            }
        }
        else
        {
            Debug.LogWarning("Please enter a valid amount.");
        }

        // pure seq
        if (!string.IsNullOrEmpty(pureSeq_inputField.text))
        {
            int betAmount = int.Parse(pureSeq_inputField.text);

            if (betAmount > 0)
            {
                string catId = GetCategories("Pure Seq");
                StartCoroutine(SendBetToServer(betAmount, luckyLottoTimer.GetGameId(), luckyLottoTimer.GetGameRoundIdGenerated(), catId));
            }
        }
        else
        {
            Debug.LogWarning("Please enter a valid amount.");
        }

        // Color
        if (!string.IsNullOrEmpty(color_inputField.text))
        {
            int betAmount = int.Parse(color_inputField.text);

            if (betAmount > 0)
            {
                string catId = GetCategories("Color");
                StartCoroutine(SendBetToServer(betAmount, luckyLottoTimer.GetGameId(), luckyLottoTimer.GetGameRoundIdGenerated(), catId));
            }
        }
        else
        {
            Debug.LogWarning("Please enter a valid amount.");
        }

        // Pair
        if (!string.IsNullOrEmpty(pair_inputField.text))
        {
            int betAmount = int.Parse(pair_inputField.text);

            if (betAmount > 0)
            {
                string catId = GetCategories("Pair");
                StartCoroutine(SendBetToServer(betAmount, luckyLottoTimer.GetGameId(), luckyLottoTimer.GetGameRoundIdGenerated(), catId));
            }
        }
        else
        {
            Debug.LogWarning("Please enter a valid amount.");
        }

        // High Card
        if (!string.IsNullOrEmpty(high_inputField.text))
        {
            int betAmount = int.Parse(high_inputField.text);

            if (betAmount > 0)
            {
                string catId = GetCategories("High Card");
                StartCoroutine(SendBetToServer(betAmount, luckyLottoTimer.GetGameId(), luckyLottoTimer.GetGameRoundIdGenerated(), catId));
            }
        }
        else
        {
            Debug.LogWarning("Please enter a valid amount.");
        }


        // Low Card
        if (!string.IsNullOrEmpty(low_inputField.text))
        {
            int betAmount = int.Parse(low_inputField.text);

            if (betAmount > 0)
            {
                string catId = GetCategories("Low Card");
                StartCoroutine(SendBetToServer(betAmount, luckyLottoTimer.GetGameId(), luckyLottoTimer.GetGameRoundIdGenerated(), catId));
            }
        }
        else
        {
            Debug.LogWarning("Please enter a valid amount.");
        }
    }
    IEnumerator SendBetToServer(int betAmount, string gameRoundId, string gameRoundIdgenerated, string categoryId)
    {
        Debug.Log("BetAmount = " + betAmount + " GameRoundId = " + gameRoundId + " GameRoundIdgenerated =" + gameRoundIdgenerated + "CategoryId : " + categoryId);

        if (AuthTok == null)
        {
            AuthTok = GetToken();
        }
       LuckyLottoBetData bet = new LuckyLottoBetData
        {
            betUnit = betAmount,
            gameRoundId = gameRoundId,
            gameRoundIdgenerated = gameRoundIdgenerated,
            categoryId = categoryId
        };

        LuckyLottoBet betData = new LuckyLottoBet()
        {
            bets = new List<LuckyLottoBetData> { bet }
        };;

        string jsonData = JsonConvert.SerializeObject(betData);
        Debug.Log("Serialized JSON Data: " + jsonData);

        using (UnityWebRequest request = new UnityWebRequest(betApiUrl, "POST"))
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
                luckyLottoBetHistoryManager.BetHistoryButtonClick();
                luckyLottoWalletManager.GetWalletBalance();
            }
            else
            {
                Debug.LogError("Error sending bet data: " + request.downloadHandler.text);

            }

        }

    }

    private void ClearAllInputFields()
    {
        set_inputField.text = "";
        seq_inputField.text = "";
        color_inputField.text = "";
        pureSeq_inputField.text = "";
        high_inputField.text = "";
        low_inputField.text = "";
        pair_inputField.text = "";

    }

    public void SetToken(string token)
    {
        AuthTok = token;
    }
    public string GetToken()
    {
        return AuthTok;
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

    public string GetCategories(string boxNumber)
    {
        string category = "";
        switch (boxNumber)
        {
            case "Seq":
                category = "67650962fd79e3d99f852b98";
                break;
            case "Set":
                category = "67650aa3fd79e3d99f852ce4";
                break;
            case "Pure Seq":
                category = "67650a67fd79e3d99f852c9d";
                break;
            case "Color":
                category = "67650a7dfd79e3d99f852cd1";
                break;
            case "Pair":
                category = "67650a8afd79e3d99f852ce1";
                break;
            case "High Card":
                category = "676508b8fd79e3d99f852ad2";
                break;
            case "Low Card":
                category = "676507e1fd79e3d99f852a22";
                break;
            default:
                category = "";
                break;
        }
        return category;
    }
    //public void OnCloseDialogBox()
    //{
    //    dialogBox.SetActive(false);
    //}

}
[System.Serializable]
public class LuckyLottoBetData
{
    public int betUnit { get; set; }
    public string gameRoundId { get; set; }
    public string gameRoundIdgenerated { get; set; }
    public string categoryId { get; set; }
}

[System.Serializable]
public class LuckyLottoBet
{
    public List<LuckyLottoBetData> bets;
}