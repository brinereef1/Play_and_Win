using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine.Networking;
public class SuperRouletteBetManager : MonoBehaviour
{
    [Header("NumberShowText")]
    [SerializeField] TMP_Text numberShowText;
    [Header("DialogBox Properties")]
    public GameObject dialogBox;
    public TMP_InputField moneyInputField;

    [Header("ResponseText")]
    public TMP_Text responseText;

    private int totalBet = 0;
    private List<string> betNumbers = new List<string>();
    private const string betApiUrl = "http://13.234.117.221:2556/api/v1/user/roulette_userbet";
    private string AuthTok;
    SuperRouletteTimer superRouletteTimer;
    SaveUserData svd = new SaveUserData();
    SuperRouletteBetHistoryManager betHistoryManager;
    RouletteWalletManager rouletteWalletManager;
    void Start()
    {
        dialogBox.SetActive(false);
        superRouletteTimer = FindFirstObjectByType<SuperRouletteTimer>();
        betHistoryManager = FindFirstObjectByType<SuperRouletteBetHistoryManager>();
        rouletteWalletManager = FindFirstObjectByType<RouletteWalletManager>();
        AuthTok = svd.GetSavedAuthToken();
    }
    #region functions
    public void OnCloseDialogBox()
    {
        dialogBox.SetActive(false);
    }

    public void OnButtonClick(string boxNumber)
    {
        Debug.Log("BoxNo" + boxNumber);
        dialogBox.SetActive(true);
        numberShowText.text = boxNumber;
        betNumbers.Add(boxNumber);
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
                StartCoroutine(SendBetToServer(betAmount, superRouletteTimer.GetGameId(), superRouletteTimer.GetGameRoundIdGenerated(), catId));

                // Debug.Log("Category: " + catId + " for: " + value);
                Debug.Log("Tried to clear the bet..");
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
        var categories = new Dictionary<string, string>
    {
        { "0", "6763fa6d00571a829daef2f5" },
        { "1", "6763fa7600571a829daef2f8" },
        { "2", "6763faa715ea831a52ac14ba" },
        { "3", "6763faae15ea831a52ac14bd" },
        { "4", "6763fab515ea831a52ac14c0" },
        { "5", "6763fabf15ea831a52ac14cb" },
        { "6", "6763fac715ea831a52ac14ce" },
        { "7", "6763fad115ea831a52ac14e5" },
        { "8", "6763fadd15ea831a52ac14f5" },
        { "9", "6763fae415ea831a52ac14f8" },
        { "10", "6763faf315ea831a52ac14fb" },
        { "11", "6763fafa15ea831a52ac150b" },
        { "12", "6763fb0115ea831a52ac150e" },
        { "13", "6763fb0815ea831a52ac1511" },
        { "14", "6763fb1015ea831a52ac152c" },
        { "15", "6763fb1815ea831a52ac153c" },
        { "16", "6763fb1f15ea831a52ac153f" },
        { "17", "6763fb2b15ea831a52ac1542" },
        { "18", "6763fb3515ea831a52ac1552" },
        { "19", "6763fb4315ea831a52ac1555" },
        { "20", "6763fb4915ea831a52ac1562" },
        { "21", "6763fb5415ea831a52ac1572" },
        { "22", "6763fb5b15ea831a52ac1575" },
        { "23", "6763fb6715ea831a52ac1578" },
        { "24", "6763fb6e15ea831a52ac157b" },
        { "25", "6763fb7415ea831a52ac158b" },
        { "26", "6763fb7a15ea831a52ac158e" },
        { "27", "6763fb8215ea831a52ac1591" },
        { "28", "6763fb8f15ea831a52ac15bf" },
        { "29", "6763fb9715ea831a52ac15c2" },
        { "30", "6763fba015ea831a52ac15c5" },
        { "31", "6763fba815ea831a52ac15c8" },
        { "32", "6763fbb115ea831a52ac15d8" },
        {"33",  "6763fbc215ea831a52ac15f7" },
        {"34",  "6763fbc915ea831a52ac15fa" },
        {"35",  "6763fbd015ea831a52ac160a" },
        {"36",  "6763fbd915ea831a52ac160d" },
        { "first12", "6763fa1b00571a829daef27e" },
        { "second12", "6763fa2200571a829daef291" },
        { "third12", "6763fa2900571a829daef29c" },
        { "black", "6763fa4400571a829daef2b2" },
        { "red", "6763fa4c00571a829daef2b5" },
        { "even", "6763fa3800571a829daef2a2" },
        { "odd", "6763fa3100571a829daef29f" },
        { "first18", "6763fa5600571a829daef2b8" },
        { "second18", "6763fa5d00571a829daef2e5" }
    };

        return categories.TryGetValue(boxNumber, out var category) ? category : string.Empty;
    }

    #endregion functions
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
        spBet bet = new spBet
        {
            betUnit = betAmount,
            gameRoundId = gameRoundId,
            gameRoundIdgenerated = gameRoundIdgenerated,
            categoryId = categoryId
        };

        BetData betData = new BetData()
        {
            bets = new List<spBet> { bet }
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
                betHistoryManager.BetHistoryButtonClick();
                rouletteWalletManager.GetWalletBalance();
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
}
[System.Serializable]
public class spBet
{
    public int betUnit{ get; set; }
    public string gameRoundId { get; set; }
    public string gameRoundIdgenerated { get; set; }
    public string categoryId { get; set; }
}
[System.Serializable]
public class BetData
{
    public List<spBet> bets { get; set; }
}