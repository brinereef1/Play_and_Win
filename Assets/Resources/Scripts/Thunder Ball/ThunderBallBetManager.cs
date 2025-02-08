using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine.UI;
using System.Linq;
using Newtonsoft.Json.Linq;
public class ThunderBallBetManager : MonoBehaviour
{

    private List<string> betNumbers = new List<string>();
    public List<Button> buttons_thunder = new List<Button>();

    public GameObject dialogBox;
    public TMP_InputField moneyInputField;
    private string AuthTok;
    private string ThunderBall_LiveUrl = "http://13.234.117.221:2556/api/v1/user/createUserBet_thunder";

    public TMP_Text showBoxNumber;
    // public TMP_Text responseText;
    ThunderBallTimer thunderBallTimer;
    SaveUserData svd = new SaveUserData();
    ThunderBallBetHistoryManager betHistoryManager;
    ThunderBallWalletManager walletManager;
    public TMP_Text responseText;
    public GameObject buttonParent_thunder;
    void Start()
    {
        dialogBox.SetActive(false);
        thunderBallTimer = FindAnyObjectByType<ThunderBallTimer>();
        betHistoryManager = FindFirstObjectByType<ThunderBallBetHistoryManager>();
        walletManager = FindFirstObjectByType<ThunderBallWalletManager>();
        AuthTok = svd.GetSavedAuthToken();
        InitializeButtonsOfPattiPanel();
    }

    void InitializeButtonsOfPattiPanel()
    {
        Button[] childButtons = buttonParent_thunder.GetComponentsInChildren<Button>();
        buttons_thunder.AddRange(childButtons);


        Debug.Log("Total Buttons Initialized: " + buttons_thunder.Count);

        for (int i = 0; i < buttons_thunder.Count; i++)
        {
            Button button = buttons_thunder[i];
            button.onClick.AddListener(() => OnButtonClick(button.name));
        }


    }

    public void OnSubmitBet()
    {
        if (!string.IsNullOrEmpty(moneyInputField.text))
        {
            int betAmount = int.Parse(moneyInputField.text);

            Debug.Log(betNumbers.Count() + "Total Bet");
            if (betNumbers.Count() > 0)
            {
                string value = betNumbers.Last().ToString();
                string catId = GetCategories(value);
                Debug.Log("Categories: " + catId + " For: " + value);
                StartCoroutine(SendBetToServer(betAmount, thunderBallTimer.GetGameId(), thunderBallTimer.GetGameRoundIdGenerated(), catId));
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

    public string GetCategories(string boxNumber)
    {
        string category = "";
        switch (boxNumber)
        {
            case "00":
                category = "6763a7248f439c7e7c2d9a51";
                break;
            case "01":
                category = "6763a7248f439c7e7c2d9a52";
                break;
            case "02":
                category = "6763a7248f439c7e7c2d9a53";
                break;
            case "03":
                category = "6763a7248f439c7e7c2d9a54";
                break;
            case "04":
                category = "6763a7248f439c7e7c2d9a55";
                break;
            case "05":
                category = "6763a7248f439c7e7c2d9a56";
                break;
            case "06":
                category = "6763a7248f439c7e7c2d9a57";
                break;
            case "07":
                category = "6763a7248f439c7e7c2d9a58";
                break;
            case "08":
                category = "6763a7248f439c7e7c2d9a59";
                break;
            case "09":
                category = "6763a7248f439c7e7c2d9a5a";
                break;
            case "10":
                category = "6763a7248f439c7e7c2d9a5b";
                break;
            case "11":
                category = "6763a7248f439c7e7c2d9a5c";
                break;
            case "12":
                category = "6763a7248f439c7e7c2d9a5d";
                break;
            case "13":
                category = "6763a7248f439c7e7c2d9a5e";
                break;
            case "14":
                category = "6763a7248f439c7e7c2d9a5f";
                break;
            case "15":
                category = "6763a7248f439c7e7c2d9a60";
                break;
            case "16":
                category = "6763a7248f439c7e7c2d9a61";
                break;
            case "17":
                category = "6763a7248f439c7e7c2d9a62";
                break;
            case "18":
                category = "6763a7248f439c7e7c2d9a63";
                break;
            case "19":
                category = "6763a7248f439c7e7c2d9a64";
                break;
            case "20":
                category = "6763a7248f439c7e7c2d9a65";
                break;
            case "21":
                category = "6763a7248f439c7e7c2d9a66";
                break;
            case "22":
                category = "6763a7248f439c7e7c2d9a67";
                break;
            case "23":
                category = "6763a7248f439c7e7c2d9a68";
                break;
            case "24":
                category = "6763a7248f439c7e7c2d9a69";
                break;
            case "25":
                category = "6763a7248f439c7e7c2d9a6a";
                break;
            case "26":
                category = "6763a7248f439c7e7c2d9a6b";
                break;
            case "27":
                category = "6763a7248f439c7e7c2d9a6c";
                break;
            case "28":
                category = "6763a7248f439c7e7c2d9a6d";
                break;
            case "29":
                category = "6763a7248f439c7e7c2d9a6e";
                break;
            case "30":
                category = "6763a7248f439c7e7c2d9a6f";
                break;
            case "31":
                category = "6763a7248f439c7e7c2d9a70";
                break;
            case "32":
                category = "6763a7248f439c7e7c2d9a71";
                break;
            case "33":
                category = "6763a7248f439c7e7c2d9a72";
                break;
            case "34":
                category = "6763a7248f439c7e7c2d9a73";
                break;
            case "35":
                category = "6763a7248f439c7e7c2d9a74";
                break;
            case "36":
                category = "6763a7248f439c7e7c2d9a75";
                break;
            case "37":
                category = "6763a7248f439c7e7c2d9a76";
                break;
            case "38":
                category = "6763a7248f439c7e7c2d9a77";
                break;
            case "39":
                category = "6763a7248f439c7e7c2d9a78";
                break;
            case "40":
                category = "6763a7248f439c7e7c2d9a79";
                break;
            case "41":
                category = "6763a7248f439c7e7c2d9a7a";
                break;
            case "42":
                category = "6763a7248f439c7e7c2d9a7b";
                break;
            case "43":
                category = "6763a7248f439c7e7c2d9a7c";
                break;
            case "44":
                category = "6763a7248f439c7e7c2d9a7d";
                break;
            case "45":
                category = "6763a7248f439c7e7c2d9a7e";
                break;
            case "46":
                category = "6763a7248f439c7e7c2d9a7f";
                break;
            case "47":
                category = "6763a7248f439c7e7c2d9a80";
                break;
            case "48":
                category = "6763a7248f439c7e7c2d9a81";
                break;
            case "49":
                category = "6763a7248f439c7e7c2d9a82";
                break;
            case "50":
                category = "6763a7248f439c7e7c2d9a83";
                break;
            case "51":
                category = "6763a7248f439c7e7c2d9a84";
                break;
            case "52":
                category = "6763a7248f439c7e7c2d9a85";
                break;
            case "53":
                category = "6763a7248f439c7e7c2d9a86";
                break;
            case "54":
                category = "6763a7248f439c7e7c2d9a87";
                break;
            case "55":
                category = "6763a7248f439c7e7c2d9a88";
                break;
            case "56":
                category = "6763a7248f439c7e7c2d9a89";
                break;
            case "57":
                category = "6763a7248f439c7e7c2d9a8a";
                break;
            case "58":
                category = "6763a7248f439c7e7c2d9a8b";
                break;
            case "59":
                category = "6763a7248f439c7e7c2d9a8c";
                break;
            case "60":
                category = "6763a7248f439c7e7c2d9a8d";
                break;
            case "61":
                category = "6763a7248f439c7e7c2d9a8e";
                break;
            case "62":
                category = "6763a7248f439c7e7c2d9a8f";
                break;
            case "63":
                category = "6763a7248f439c7e7c2d9a90";
                break;
            case "64":
                category = "6763a7248f439c7e7c2d9a91";
                break;
            case "65":
                category = "6763a7248f439c7e7c2d9a92";
                break;
            case "66":
                category = "6763a7248f439c7e7c2d9a93";
                break;
            case "67":
                category = "6763a7248f439c7e7c2d9a94";
                break;
            case "68":
                category = "6763a7248f439c7e7c2d9a95";
                break;
            case "69":
                category = "6763a7248f439c7e7c2d9a96";
                break;
            case "70":
                category = "6763a7248f439c7e7c2d9a97";
                break;
            case "71":
                category = "6763a7248f439c7e7c2d9a98";
                break;
            case "72":
                category = "6763a7248f439c7e7c2d9a99";
                break;
            case "73":
                category = "6763a7248f439c7e7c2d9a9a";
                break;
            case "74":
                category = "6763a7248f439c7e7c2d9a9b";
                break;         
            case "75":
                category = "6763a7248f439c7e7c2d9a9c";
                break;
            case "76":
                category = "6763a7248f439c7e7c2d9a9d";
                break;
            case "77":
                category = "6763a7248f439c7e7c2d9a9e";
                break;
            case "78":
                category = "6763a7248f439c7e7c2d9a9f";
                break;
            case "79":
                category = "6763a7248f439c7e7c2d9aa0";
                break;
            case "80":
                category = "6763a7248f439c7e7c2d9aa1";
                break;
            case "81":
                category = "6763a7248f439c7e7c2d9aa2";
                break;
            case "82":
                category = "6763a7248f439c7e7c2d9aa3";
                break;
            case "83":
                category = "6763a7248f439c7e7c2d9aa4";
                break;
            case "84":
                category = "6763a7248f439c7e7c2d9aa5";
                break;
            case "85":
                category = "6763a7248f439c7e7c2d9aa6";
                break;
            case "86":
                category = "6763a7248f439c7e7c2d9aa7";
                break;
            case "87":
                category = "6763a7248f439c7e7c2d9aa8";
                break;
            case "88":
                category = "6763a7248f439c7e7c2d9aa9";
                break;
            case "89":
                category = "6763a7248f439c7e7c2d9aaa";
                break;
            case "90":
                category = "6763a7248f439c7e7c2d9aab";
                break;
            case "91":
                category = "6763a7248f439c7e7c2d9aac";
                break;
            case "92":
                category = "6763a7248f439c7e7c2d9aad";
                break;
            case "93":
                category = "6763a7248f439c7e7c2d9aae";
                break;
            case "94":
                category = "6763a7248f439c7e7c2d9aaf";
                break;
            case "95":
                category = "6763a7248f439c7e7c2d9ab0";
                break;
            case "96":
                category = "6763a7248f439c7e7c2d9ab1";
                break;
            case "97":
                category = "6763a7248f439c7e7c2d9ab2";
                break;
            case "98":
                category = "6763a7248f439c7e7c2d9ab3";
                break;
            case "99":
                category = "6763a7248f439c7e7c2d9ab4";
                break;
            default:
                category = "";
                break;
        }

        return category;
    }


    public void OnButtonClick(string boxNumber)
    {
        Debug.Log("BoxNo" + boxNumber);
        dialogBox.SetActive(true);
        showBoxNumber.text = boxNumber;

        betNumbers.Add(boxNumber);
    }

    IEnumerator SendBetToServer(int betAmount, string gameRoundId, string gameRoundIdgenerated, string categoryId)
    {
        Debug.Log("BetAmount = " + betAmount + " GameRoundId = " + gameRoundId + " GameRoundIdgenerated =" + gameRoundIdgenerated + "CategoryId : " + categoryId);

        if (AuthTok == null)
        {
            AuthTok = GetToken();
        }
        ThunderBallBet bet = new ThunderBallBet
        {
            betUnit = betAmount,
            gameRoundId = gameRoundId,
            gameRoundIdgenerated = gameRoundIdgenerated,
            categoryId = categoryId
        };

        ThunderBallBetData betData = new ThunderBallBetData()
        {
            bets = new List<ThunderBallBet> { bet }
        };

        string jsonData = JsonConvert.SerializeObject(betData);
        Debug.Log("Serialized JSON Data: " + jsonData);

        using (UnityWebRequest request = new UnityWebRequest(ThunderBall_LiveUrl, "POST"))
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
                betHistoryManager.BetHistoryButtonClick();
                walletManager.GetWalletBalance();
            }
            else
            {
                Debug.LogError("Error sending bet data: " + request.downloadHandler.text);

            }

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
    public void OnCloseDialogBox()
    {
        dialogBox.SetActive(false);
    }
}
[System.Serializable]
public class ThunderBallBet
{
    public int betUnit { get; set; }
    public string gameRoundId { get; set; }
    public string gameRoundIdgenerated { get; set; }
    public string categoryId { get; set; }
}

[System.Serializable]
public class ThunderBallBetData
{
    public List<ThunderBallBet> bets { get; set; }
}