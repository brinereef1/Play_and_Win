using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine.Networking;
using UnityEngine.UI;

public class JMBetManager : MonoBehaviour
{
    //[Header("TextToShowNumber")]
    //[SerializeField] TMP_Text date_display_text;

    //public GameObject dialogBox;
    //public TMP_InputField moneyInputField;
    //public GameObject buttonParent_single;

    //private List<string> betNumbers = new List<string>();
    //private int totalBet = 0;
    public TMP_Text responseText;
    //public List<Button> buttons_single = new List<Button>();
    [Header("BetButton")]
    public Button Bet_Button;

    [Header("InputFields")]
    public TMP_InputField club_inputField;
    public TMP_InputField crown_inputField;
    public TMP_InputField diamond_inputField;
    public TMP_InputField flag_inputField;
    public TMP_InputField heart_inputField;
    public TMP_InputField spade_inputField;
    
    JMTimer jmTimer;
    SaveUserData svd = new SaveUserData();
    private const string betApiUrl = "http://13.234.117.221:2556/api/v1/user/jhandimunda_userbet";
    private string AuthTok;
    JMWalletManager jmwalletManager;
    JMBetHistoryManager jMBetHistoryManager;
    void Start()
    {
        //dialogBox.SetActive(false);
        jmTimer = FindFirstObjectByType<JMTimer>();
        AuthTok = svd.GetSavedAuthToken();
        jmwalletManager = FindFirstObjectByType<JMWalletManager>();
        jMBetHistoryManager = FindFirstObjectByType<JMBetHistoryManager>();
        Bet_Button.onClick.AddListener(OnSubmitBet);
        //InitializeButtonsOfSinglePanel();
    }

    //public void OnButtonClick(string boxNumber)
    //{
    //    Debug.Log("BoxNo" + boxNumber);
    //    dialogBox.SetActive(true);
    //    date_display_text.text = boxNumber;
    //    betNumbers.Add(boxNumber);
    //}
    //public void OnCloseDialogBox()
    //{
    //    dialogBox.SetActive(false);
    //}
    //void InitializeButtonsOfSinglePanel()
    //{
    //    Button[] childButtons = buttonParent_single.GetComponentsInChildren<Button>();
    //    buttons_single.AddRange(childButtons);

    //    Debug.Log("Total Buttons Initialized: " + buttons_single.Count);

    //    for (int i = 0; i < buttons_single.Count; i++)
    //    {
    //        Button button = buttons_single[i];
    //        button.onClick.AddListener(() => OnButtonClick(button.name));
    //    }


    //}

    public void OnSubmitBet()
    {
        // Spade
        if (!string.IsNullOrEmpty(spade_inputField.text))
        {
            int betAmount = int.Parse(spade_inputField.text);

            if (betAmount > 0)
            {               
                string catId = GetCategories("Spade");
                StartCoroutine(SendBetToServer(betAmount, jmTimer.GetGameId(), jmTimer.GetGameRoundIdGenerated(), catId));
            }            
        }
        else
        {
            Debug.LogWarning("Please enter a valid amount.");
        }

        // Club
        if (!string.IsNullOrEmpty(club_inputField.text))
        {
            int betAmount = int.Parse(club_inputField.text);

            if (betAmount > 0)
            {
                string catId = GetCategories("Club");
                StartCoroutine(SendBetToServer(betAmount, jmTimer.GetGameId(), jmTimer.GetGameRoundIdGenerated(), catId));
            }
        }
        else
        {
            Debug.LogWarning("Please enter a valid amount.");
        }

        // Crown
        if (!string.IsNullOrEmpty(crown_inputField.text))
        {
            int betAmount = int.Parse(crown_inputField.text);

            if (betAmount > 0)
            {
                string catId = GetCategories("Crown");
                StartCoroutine(SendBetToServer(betAmount, jmTimer.GetGameId(), jmTimer.GetGameRoundIdGenerated(), catId));
            }
        }
        else
        {
            Debug.LogWarning("Please enter a valid amount.");
        }

        // Diamond
        if (!string.IsNullOrEmpty(diamond_inputField.text))
        {
            int betAmount = int.Parse(diamond_inputField.text);

            if (betAmount > 0)
            {
                string catId = GetCategories("Diamond");
                StartCoroutine(SendBetToServer(betAmount, jmTimer.GetGameId(), jmTimer.GetGameRoundIdGenerated(), catId));
            }
        }
        else
        {
            Debug.LogWarning("Please enter a valid amount.");
        }

        // Flag
        if (!string.IsNullOrEmpty(flag_inputField.text))
        {
            int betAmount = int.Parse(flag_inputField.text);

            if (betAmount > 0)
            {
                string catId = GetCategories("Flag");
                StartCoroutine(SendBetToServer(betAmount, jmTimer.GetGameId(), jmTimer.GetGameRoundIdGenerated(), catId));
            }
        }
        else
        {
            Debug.LogWarning("Please enter a valid amount.");
        }

        // Heart
        if (!string.IsNullOrEmpty(heart_inputField.text))
        {
            int betAmount = int.Parse(heart_inputField.text);

            if (betAmount > 0)
            {
                string catId = GetCategories("Heart");
                StartCoroutine(SendBetToServer(betAmount, jmTimer.GetGameId(), jmTimer.GetGameRoundIdGenerated(), catId));
            }
        }
        else
        {
            Debug.LogWarning("Please enter a valid amount.");
        }
    }
    public void SetToken(string token)
    {
        AuthTok = token;
    }

    public string GetToken()
    {
        return AuthTok;
    }
    IEnumerator SendBetToServer(int betAmount, string gameRoundId, string gameRoundIdgenerated, string categoryId)
    {
        Debug.Log("BetAmount = " + betAmount + " GameRoundId = " + gameRoundId + " GameRoundIdgenerated =" + gameRoundIdgenerated + "CategoryId : " + categoryId);

        if (AuthTok == null)
        {
            AuthTok = GetToken();
        }
        JMBet bet = new JMBet
        {
            betUnit = betAmount,
            gameRoundId = gameRoundId,
            gameRoundIdgenerated = gameRoundIdgenerated,
            categoryId = categoryId
        };

        JMBetData betData = new JMBetData()
        {
            bets = new List<JMBet> { bet }
        };

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
                jMBetHistoryManager.BetHistoryButtonClick();
                jmwalletManager.GetWalletBalance();
            }
            else
            {
                Debug.LogError("Error sending bet data: " + request.downloadHandler.text);

            }

        }

    }


    private void ClearAllInputFields()
    {
        crown_inputField.text = "";
        spade_inputField.text = "";
        club_inputField.text = "";
        flag_inputField.text = "";
        diamond_inputField.text = "";
        heart_inputField.text = "";

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
        yield return new WaitForSeconds(1);
        responseText.text = "";
    }

    public string GetCategories(string boxNumber)
    {
        string category = "";
        switch (boxNumber)
        {
            case "Club":
                category = "676290da886219eb57526ed6";
                break;
            case "Crown":
                category = "676290e6886219eb57526ee9";
                break;
            case "Spade":
                category = "676290c5886219eb57526ed0";
                break;
            case "Diamond":
                category = "676290d2886219eb57526ed3";
                break;
            case "Flag":
                category = "676290e0886219eb57526ed9";
                break;
            case "Heart":
                category = "676290a7886219eb57526eba";
                break;
            default:
                category = "";
                break;
        }
        return category;
    }
}
[System.Serializable]
public class JMBet
{
    public int betUnit { get; set; }
    public string gameRoundId { get; set; }
    public string gameRoundIdgenerated { get; set; }
    public string categoryId { get; set; }
}
[System.Serializable]
public class JMBetData
{
    public List<JMBet> bets { get; set; }
}
