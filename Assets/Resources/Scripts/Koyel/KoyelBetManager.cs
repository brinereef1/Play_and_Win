using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine.Networking;
using System;
using UnityEngine.UI;

public class KoyelBetManager : MonoBehaviour
{
    public GameObject dialogBox;
    public TMP_InputField moneyInputField;
    private List<string> betNumbers = new List<string>();

    public List<Button> buttons_lastDigit = new List<Button>();
    public List<Button> buttons_middleDigit = new List<Button>();
    public List<Button> buttons_lastTwoDigit = new List<Button>();

    private int totalBet = 0;
    public TMP_Text responseText;
    public TMP_Text showBoxNumber;

    KoyelTimer koyelTimer;
    SaveUserData svd = new SaveUserData();
    KoyelBetHistoryManager _KoyelBetHistoryManager;
    KoyelWalletManager _KoyelWalletManager;

    public GameObject buttonParent_LastDigit;
    public GameObject buttonParent_MiddleDigit;
    public GameObject buttonParent_LastTwoDigit;

    private const string betApiUrl = "http://13.234.117.221:2556/api/v1/user/koyel_userbet";
    private string AuthTok;
    void Start()
    {
        dialogBox.SetActive(false);
        koyelTimer = FindFirstObjectByType<KoyelTimer>();
        _KoyelBetHistoryManager = FindFirstObjectByType<KoyelBetHistoryManager>();
        _KoyelWalletManager = FindFirstObjectByType<KoyelWalletManager>();

        AuthTok = svd.GetSavedAuthToken();
        InitializeButtonsOfLastDigit();
        InitializeButtonsOfMiddleDigit();
        InitializeButtonsOfLastTwoDigit();
    }

    private void InitializeButtonsOfMiddleDigit()
    {
        Button[] childButtons = buttonParent_MiddleDigit.GetComponentsInChildren<Button>();
        buttons_middleDigit.AddRange(childButtons);


        Debug.Log("Total Buttons Initialized: " + buttons_middleDigit.Count);

        for (int i = 0; i < buttons_middleDigit.Count; i++)
        {
            Button button = buttons_middleDigit[i];
            button.onClick.AddListener(() => OnButtonClick(button.name));
        }
    }

    private void InitializeButtonsOfLastTwoDigit()
    {
        Button[] childButtons = buttonParent_LastTwoDigit.GetComponentsInChildren<Button>();
        buttons_lastTwoDigit.AddRange(childButtons);


        Debug.Log("Total Buttons Initialized: " + buttons_lastTwoDigit.Count);

        for (int i = 0; i < buttons_lastTwoDigit.Count; i++)
        {
            Button button = buttons_lastTwoDigit[i];
            button.onClick.AddListener(() => OnButtonClick(button.name));
        }
    }

    private void InitializeButtonsOfLastDigit()
    {
        Button[] childButtons = buttonParent_LastDigit.GetComponentsInChildren<Button>();
        buttons_lastDigit.AddRange(childButtons);


        Debug.Log("Total Buttons Initialized: " + buttons_lastDigit.Count);

        for (int i = 0; i < buttons_lastDigit.Count; i++)
        {
            Button button = buttons_lastDigit[i];
            button.onClick.AddListener(() => OnButtonClick(button.name));
        }
    }

    public void OnButtonClick(string boxNumber)
    {
        Debug.Log("BoxNo" + boxNumber);
        dialogBox.SetActive(true);
        showBoxNumber.text = boxNumber;
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
  
            Debug.Log(betNumbers.Count() + "Total Bet");
            if (betNumbers.Count() > 0)
            {
                string value = betNumbers.Last().ToString();
                string catId = GetCategories(value);
                Debug.Log("Categories: " + catId + " For: " + value);


                StartCoroutine(SendBetToServer(betAmount, koyelTimer.GetGameId(), koyelTimer.GetGameRoundIdGenerated(), catId));
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
        KoyelBet bet = new KoyelBet
        {
            betAmount = betAmount,
            gameRoundId = gameRoundId,
            gameRoundIdgenerated = gameRoundIdgenerated,
            categoryId = categoryId
        };

        KoyelBetData betData = new KoyelBetData()
        {
            bets = new List<KoyelBet> { bet }
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
                _KoyelWalletManager.GetWalletBalance();
                _KoyelBetHistoryManager.BetHistoryButtonClick();
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
            case "00":
                category = "67666c8e770cada55fb428a6";
                break;
            case "01":
                category = "67666c8e770cada55fb428a7";
                break;
            case "02":
                category = "67666c8e770cada55fb428a8";
                break;
            case "03":
                category = "67666c8e770cada55fb428a9";
                break;
            case "04":
                category = "67666c8e770cada55fb428aa";
                break;
            case "05":
                category = "67666c8e770cada55fb428ab";
                break;
            case "06":
                category = "67666c8e770cada55fb428ac";
                break;
            case "07":
                category = "67666c8e770cada55fb428ad";
                break;
            case "08":
                category = "67666c8e770cada55fb428ae";
                break;
            case "09":
                category = "67666c8e770cada55fb428af";
                break;
            case "10":
                category = "67666c8e770cada55fb428b0";
                break;
            case "11":
                category = "67666c8e770cada55fb428b1";
                break;
            case "12":
                category = "67666c8e770cada55fb428b2";
                break;
            case "13":
                category = "67666c8e770cada55fb428b3";
                break;
            case "14":
                category = "67666c8e770cada55fb428b4";
                break;
            case "15":
                category = "67666c8e770cada55fb428b5";
                break;
            case "16":
                category = "67666c8e770cada55fb428b6";
                break;
            case "17":
                category = "67666c8e770cada55fb428b7";
                break;
            case "18":
                category = "67666c8e770cada55fb428b8";
                break;
            case "19":
                category = "67666c8e770cada55fb428b9";
                break;
            case "20":
                category = "67666c8e770cada55fb428ba";
                break;
            case "21":
                category = "67666c8e770cada55fb428bb";
                break;
            case "22":
                category = "67666c8e770cada55fb428bc";
                break;
            case "23":
                category = "67666c8e770cada55fb428bd";
                break;
            case "24":
                category = "67666c8e770cada55fb428be";
                break;
            case "25":
                category = "67666c8e770cada55fb428bf";
                break;
            case "26":
                category = "67666c8e770cada55fb428c0";
                break;
            case "27":
                category = "67666c8e770cada55fb428c1";
                break;
            case "28":
                category = "67666c8e770cada55fb428c2";
                break;
            case "29":
                category = "67666c8e770cada55fb428c3";
                break;
            case "30":
                category = "67666c8e770cada55fb428c4";
                break;
            case "31":
                category = "67666c8e770cada55fb428c5";
                break;
            case "32":
                category = "67666c8e770cada55fb428c6";
                break;
            case "33":
                category = "67666c8e770cada55fb428c7";
                break;
            case "34":
                category = "67666c8e770cada55fb428c8";
                break;
            case "35":
                category = "67666c8e770cada55fb428c9";
                break;
            case "36":
                category = "67666c8e770cada55fb428ca";
                break;
            case "37":
                category = "67666c8e770cada55fb428cb";
                break;
            case "38":
                category = "67666c8e770cada55fb428cc";
                break;
            case "39":
                category = "67666c8e770cada55fb428cd";
                break;
            case "40":
                category = "67666c8e770cada55fb428ce";
                break;
            case "41":
                category = "67666c8e770cada55fb428cf";
                break;
            case "42":
                category = "67666c8e770cada55fb428d0";
                break;
            case "43":
                category = "67666c8e770cada55fb428d1";
                break;
            case "44":
                category = "67666c8e770cada55fb428d2";
                break;
            case "45":
                category = "67666c8e770cada55fb428d3";
                break;
            case "46":
                category = "67666c8e770cada55fb428d4";
                break;
            case "47":
                category = "67666c8e770cada55fb428d5";
                break;
            case "48":
                category = "67666c8e770cada55fb428d6";
                break;
            case "49":
                category = "67666c8e770cada55fb428d7";
                break;
            case "50":
                category = "67666c8e770cada55fb428d8";
                break;
            case "51":
                category = "67666c8e770cada55fb428d9";
                break;
            case "52":
                category = "67666c8e770cada55fb428da";
                break;
            case "53":
                category = "67666c8e770cada55fb428db";
                break;
            case "54":
                category = "67666c8e770cada55fb428dc";
                break;
            case "55":
                category = "67666c8e770cada55fb428dd";
                break;
            case "56":
                category = "67666c8e770cada55fb428de";
                break;
            case "57":
                category = "67666c8e770cada55fb428df";
                break;
            case "58":
                category = "67666c8e770cada55fb428e0";
                break;
            case "59":
                category = "67666c8e770cada55fb428e1";
                break;
            case "60":
                category = "67666c8e770cada55fb428e2";
                break;
            case "61":
                category = "67666c8e770cada55fb428e3";
                break;
            case "62":
                category = "67666c8e770cada55fb428e4";
                break;
            case "63":
                category = "67666c8e770cada55fb428e5";
                break;
            case "64":
                category = "67666c8e770cada55fb428e6";
                break;
            case "65":
                category = "67666c8e770cada55fb428e7";
                break;
            case "66":
                category = "67666c8e770cada55fb428e8";
                break;
            case "67":
                category = "67666c8e770cada55fb428e9";
                break;
            case "68":
                category = "67666c8e770cada55fb428ea";
                break;
            case "69":
                category = "67666c8e770cada55fb428eb";
                break;
            case "70":
                category = "67666c8e770cada55fb428ec";
                break;
            case "71":
                category = "67666c8e770cada55fb428ed";
                break;
            case "72":
                category = "67666c8e770cada55fb428ee";
                break;
            case "73":
                category = "67666c8e770cada55fb428ef";
                break;
            case "74":
                category = "67666c8e770cada55fb428f0";
                break;
            case "75":
                category = "67666c8e770cada55fb428f1";
                break;
            case "76":
                category = "67666c8e770cada55fb428f2";
                break;
            case "77":
                category = "67666c8e770cada55fb428f3";
                break;
            case "78":
                category = "67666c8e770cada55fb428f4";
                break;
            case "79":
                category = "67666c8e770cada55fb428f5";
                break;
            case "80":
                category = "67666c8e770cada55fb428f6";
                break;
            case "81":
                category = "67666c8e770cada55fb428f7";
                break;
            case "82":
                category = "67666c8e770cada55fb428f8";
                break;
            case "83":
                category = "67666c8e770cada55fb428f9";
                break;
            case "84":
                category = "67666c8e770cada55fb428fa";
                break;
            case "85":
                category = "67666c8e770cada55fb428fb";
                break;
            case "86":
                category = "67666c8e770cada55fb428fc";
                break;
            case "87":
                category = "67666c8e770cada55fb428fd";
                break;
            case "88":
                category = "67666c8e770cada55fb428fe";
                break;
            case "89":
                category = "67666c8e770cada55fb428ff";
                break;
            case "90":
                category = "67666c8e770cada55fb42900";
                break;
            case "91":
                category = "67666c8e770cada55fb42901";
                break;
            case "92":
                category = "67666c8e770cada55fb42902";
                break;
            case "93":
                category = "67666c8e770cada55fb42903";
                break;
            case "94":
                category = "67666c8e770cada55fb42904";
                break;
            case "95":
                category = "67666c8e770cada55fb42905";
                break;
            case "96":
                category = "67666c8e770cada55fb42906";
                break;
            case "97":
                category = "67666c8e770cada55fb42907";
                break;
            case "98":
                category = "67666c8e770cada55fb42908";
                break;
            case "99":
                category = "67666c8e770cada55fb42909";
                break;
            default:
                break;
        }

        return category;
    }
}
[System.Serializable]
public class KoyelBet
{
    public int betAmount { get; set; }
    public string gameRoundId { get; set; }
    public string gameRoundIdgenerated { get; set; }
    public string categoryId { get; set; }
}
[System.Serializable]
public class KoyelBetData
{
    public List<KoyelBet> bets { get; set; }
}