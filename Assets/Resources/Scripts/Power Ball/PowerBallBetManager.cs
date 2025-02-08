using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Linq;
public class PowerBallBetManager : MonoBehaviour
{
    //[Header("DialogBoxNumber Text")]
    //[SerializeField] TMP_Text dialogBoxNumber_Text;
    [Header("Bet Button")]
    public Button Bet_Button;

    [Header("InputFields")]
    public TMP_InputField zero_InputField;
    public TMP_InputField one_InputField;
    public TMP_InputField two_InputField;
    public TMP_InputField three_InputField;
    public TMP_InputField four_InputField;
    public TMP_InputField five_InputField;
    public TMP_InputField six_InputField;
    public TMP_InputField seven_InputField;
    public TMP_InputField eight_InputField;
    public TMP_InputField nine_InputField;

    [Header("Authentication Token")]
    private string AuthTok;
    [Header("Api Url")]
    private string PowerBall_LiveUrl = "http://13.234.117.221:2556/api/v1/user/createUserBet_powerball";
    [Header("Script References")]
    PowerBallTimer powerBallTimer;
    SaveUserData svd = new SaveUserData();
    PowerBallBetHistoryManager powerBallBetHistoryManager;
    PowerBallWalletManager powerBallWalletManager;

    public TMP_Text responseText;
    public GameObject buttonParent_powerBall;
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
        powerBallBetHistoryManager = FindFirstObjectByType<PowerBallBetHistoryManager>();
        powerBallTimer = FindFirstObjectByType<PowerBallTimer>();
        powerBallWalletManager = FindFirstObjectByType<PowerBallWalletManager>();
        AuthTok = svd.GetSavedAuthToken();
        Bet_Button.onClick.AddListener(OnSubmitBet);
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
    //    dialogBoxNumber_Text.text = boxNumber.Substring(1);
    //    betNumbers.Add(boxNumber);
    //}

    public void OnSubmitBet()
    {
        // zero 
        if (!string.IsNullOrEmpty(zero_InputField.text))
        {
            int betAmount = int.Parse(zero_InputField.text);
            
            if (betAmount > 0)
            {               
                string catId = GetCategories("00");              
                StartCoroutine(SendBetToServer(betAmount, powerBallTimer.GetGameId(), powerBallTimer.GetGameRoundIdGenerated(), catId));                
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

        // one 
        if (!string.IsNullOrEmpty(one_InputField.text))
        {
            int betAmount = int.Parse(one_InputField.text);

            if (betAmount > 0)
            {
                string catId = GetCategories("01");
                StartCoroutine(SendBetToServer(betAmount, powerBallTimer.GetGameId(), powerBallTimer.GetGameRoundIdGenerated(), catId));
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

        // two 
        if (!string.IsNullOrEmpty(two_InputField.text))
        {
            int betAmount = int.Parse(two_InputField.text);

            if (betAmount > 0)
            {
                string catId = GetCategories("02");
                StartCoroutine(SendBetToServer(betAmount, powerBallTimer.GetGameId(), powerBallTimer.GetGameRoundIdGenerated(), catId));
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

        // three 
        if (!string.IsNullOrEmpty(three_InputField.text))
        {
            int betAmount = int.Parse(three_InputField.text);

            if (betAmount > 0)
            {
                string catId = GetCategories("03");
                StartCoroutine(SendBetToServer(betAmount, powerBallTimer.GetGameId(), powerBallTimer.GetGameRoundIdGenerated(), catId));
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

        // four 
        if (!string.IsNullOrEmpty(four_InputField.text))
        {
            int betAmount = int.Parse(four_InputField.text);

            if (betAmount > 0)
            {
                string catId = GetCategories("04");
                StartCoroutine(SendBetToServer(betAmount, powerBallTimer.GetGameId(), powerBallTimer.GetGameRoundIdGenerated(), catId));
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

        // five 
        if (!string.IsNullOrEmpty(five_InputField.text))
        {
            int betAmount = int.Parse(five_InputField.text);

            if (betAmount > 0)
            {
                string catId = GetCategories("05");
                StartCoroutine(SendBetToServer(betAmount, powerBallTimer.GetGameId(), powerBallTimer.GetGameRoundIdGenerated(), catId));
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

        // six 
        if (!string.IsNullOrEmpty(six_InputField.text))
        {
            int betAmount = int.Parse(six_InputField.text);

            if (betAmount > 0)
            {
                string catId = GetCategories("06");
                StartCoroutine(SendBetToServer(betAmount, powerBallTimer.GetGameId(), powerBallTimer.GetGameRoundIdGenerated(), catId));
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

        // seven 
        if (!string.IsNullOrEmpty(seven_InputField.text))
        {
            int betAmount = int.Parse(seven_InputField.text);

            if (betAmount > 0)
            {
                string catId = GetCategories("07");
                StartCoroutine(SendBetToServer(betAmount, powerBallTimer.GetGameId(), powerBallTimer.GetGameRoundIdGenerated(), catId));
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

        // eight 
        if (!string.IsNullOrEmpty(eight_InputField.text))
        {
            int betAmount = int.Parse(eight_InputField.text);

            if (betAmount > 0)
            {
                string catId = GetCategories("08");
                StartCoroutine(SendBetToServer(betAmount, powerBallTimer.GetGameId(), powerBallTimer.GetGameRoundIdGenerated(), catId));
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

        // nine 
        if (!string.IsNullOrEmpty(nine_InputField.text))
        {
            int betAmount = int.Parse(nine_InputField.text);

            if (betAmount > 0)
            {
                string catId = GetCategories("09");
                StartCoroutine(SendBetToServer(betAmount, powerBallTimer.GetGameId(), powerBallTimer.GetGameRoundIdGenerated(), catId));
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
            case "00":
                category = "676280b07d85756ab8ff4ec6";
                break;
            case "01":
                category = "676280b47d85756ab8ff4ec9";
                break;
            case "02":
                category = "676280b97d85756ab8ff4ecc";
                break;
            case "03":
                category = "676280bd7d85756ab8ff4ecf";
                break;
            case "04":
                category = "676280c17d85756ab8ff4ed2";
                break;
            case "05":
                category = "676280c67d85756ab8ff4ed5";
                break;
            case "06":
                category = "676280c97d85756ab8ff4ed8";
                break;
            case "07":
                category = "676280ce7d85756ab8ff4ee2";
                break;
            case "08":
                category = "676280d27d85756ab8ff4ee5";
                break;
            case "09":
                category = "676280d57d85756ab8ff4ee8";
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
        PowerBallBet bet = new PowerBallBet
        {
            betUnit = betAmount,
            gameRoundId = gameRoundId,
            gameRoundIdgenerated = gameRoundIdgenerated,
            categoryId = categoryId
        };

        PowerBallBetData betData = new PowerBallBetData()
        {
            bets = new List<PowerBallBet> { bet }
        };

        string jsonData = JsonConvert.SerializeObject(betData);
        Debug.Log("Serialized JSON Data: " + jsonData);

        using (UnityWebRequest request = new UnityWebRequest(PowerBall_LiveUrl, "POST"))
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
                powerBallBetHistoryManager.BetHistoryButtonClick();
                powerBallWalletManager.GetWalletBalance();
            }
            else
            {
                Debug.LogError("Error sending bet data: " + request.downloadHandler.text);

            }

        }

    }

    private void ClearAllInputFields()
    {
        zero_InputField.text = "";
        one_InputField.text = "";
        two_InputField.text = "";
        three_InputField.text = "";
        four_InputField.text = "";
        five_InputField.text = "";
        six_InputField.text = "";
        seven_InputField.text = "";
        eight_InputField.text = "";
        nine_InputField.text = "";
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
public class PowerBallBet
{
    public int betUnit { get; set; }
    public string gameRoundId { get; set; }
    public string gameRoundIdgenerated { get; set; }
    public string categoryId { get; set; }
}

[System.Serializable]
public class PowerBallBetData
{
    public List<PowerBallBet> bets { get; set; }
}