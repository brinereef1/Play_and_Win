using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SingleBetManager : MonoBehaviour
{
    [Header("ShowTheBoxNumber")]
    [SerializeField] TMP_Text showTheBoxNumber_text;
    public GameObject dialogBox;
    public TMP_InputField moneyInputField;
    public GameObject buttonParent_single;

    private List<string> betNumbers = new List<string>();
    private int totalBet = 0;
    public TMP_Text responseText;
    public List<Button> buttons_single = new List<Button>();

    SingleTimer singleTimer;
    SaveUserData svd = new SaveUserData();
    private const string betApiUrl = "http://13.234.117.221:2556/api/v1/user/fatafat_single_userbet";
    private string AuthTok;
    FATAFATWalletManager fATAFATWalletManager;
    void Start()
    {
        dialogBox.SetActive(false);
        singleTimer = FindFirstObjectByType<SingleTimer>();
        AuthTok = svd.GetSavedAuthToken();
        fATAFATWalletManager = FindFirstObjectByType<FATAFATWalletManager>();
        InitializeButtonsOfSinglePanel();
    }

    public void OnButtonClick(string boxNumber)
    {
        Debug.Log("BoxNo" + boxNumber);
        dialogBox.SetActive(true);
        showTheBoxNumber_text.text = boxNumber;
        betNumbers.Add(boxNumber);
    }
    public void OnCloseDialogBox()
    {
        dialogBox.SetActive(false);
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
                string catId = GetCategories(value);
                Debug.Log("Categories: " + catId + " For: " + value);


                StartCoroutine(SendBetToServer(betAmount, singleTimer.GetGameId(), singleTimer.GetGameRoundIdGenerated(), catId));
                betNumbers.Clear();
            }

            dialogBox.SetActive(false);
            moneyInputField.text = "";
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
        SingleBet bet = new SingleBet
        {
            betAmount = betAmount,
            gameRoundId = gameRoundId,
            gameRoundIdgenerated = gameRoundIdgenerated,
            categoryId = categoryId
        };

        SingleBetData betData = new SingleBetData()
        {
            bets = new List<SingleBet> { bet }
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
                ShowBetSentResponse("Bet Sent Successfully");
                fATAFATWalletManager.GetWalletBalance();
            }
            else
            {
                Debug.LogError("Error sending bet data: " + request.downloadHandler.text);

            }

        }

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
            case "0":
                category = "6763b217b46f9394b99e0700";
                break;
            case "1":
                category = "6763b21cb46f9394b99e0703";
                break;
            case "2":
                category = "6763b21fb46f9394b99e0706";
                break;
            case "3":
                category = "6763b223b46f9394b99e0709";
                break;
            case "4":
                category = "6763b227b46f9394b99e070c";
                break;
            case "5":
                category = "6763b22bb46f9394b99e0727";
                break;
            case "6":
                category = "6763b22eb46f9394b99e072a";
                break;
            case "7":
                category = "6763b232b46f9394b99e072d";
                break;
            case "8":
                category = "6763b237b46f9394b99e073d";
                break;
            case "9":
                category = "6763b23ab46f9394b99e0740";
                break;
            default:
                category = "";
                break;
        }
        return category;
    }
}
[System.Serializable]
public class SingleBet
{
    public int betAmount { get; set; }
    public string gameRoundId { get; set; }
    public string gameRoundIdgenerated { get; set; }
    public string categoryId { get; set; }
}
[System.Serializable]
public class SingleBetData
{
    public List<SingleBet> bets { get; set; }
}
