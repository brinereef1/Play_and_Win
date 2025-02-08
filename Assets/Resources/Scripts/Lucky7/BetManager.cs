using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Networking;
using System.Text;
using Newtonsoft.Json;

public class BetManager : MonoBehaviour
{
    [Header("Bet InputFields")]
    [SerializeField] TMP_InputField twoTosix_InputField;
    [SerializeField] TMP_InputField seven_InputField;
    [SerializeField] TMP_InputField eightTotwelve_InputField;
    [Header("Response")]
    [SerializeField] public TMP_Text responseText;
    private string AuthTok;
    private string live_url = "http://13.234.117.221:2556/api/v1/";
    public string gameRoundId;
    public string gameRoundIdgenerated;
    public string temp_gameRoundId = null;
    GetTime getTime;

    SaveUserData svd = new SaveUserData();
    BetHistoryManager betHistoryManager;
    Lucky7WalletManager lucky7WalletManager;

    void Start()
    {
        getTime = FindFirstObjectByType<GetTime>();
        // Initialize the Auth token
        AuthTok = svd.GetSavedAuthToken();
        betHistoryManager = FindFirstObjectByType<BetHistoryManager>();
        lucky7WalletManager = FindFirstObjectByType<Lucky7WalletManager>();
    }


    // Method to handle the bet button click
    public void BetButton()
    {
        Debug.Log("bet ");
        // Get the bet amounts from the input fields
        int twoTosixBet = string.IsNullOrEmpty(twoTosix_InputField.text) ? 0 : int.Parse(twoTosix_InputField.text);
        int sevenBet = string.IsNullOrEmpty(seven_InputField.text) ? 0 : int.Parse(seven_InputField.text);
        int eightTotwelveBet = string.IsNullOrEmpty(eightTotwelve_InputField.text) ? 0 : int.Parse(eightTotwelve_InputField.text);

        // If the user has entered a value for the 'twoTosix' category
        if (twoTosixBet > 0)
        {
            int betAmount = Mathf.RoundToInt(twoTosixBet);
            string categoryId = "67627b3e69c5f28a27e1c4ab"; // Category ID for 'twoTosix'
            PlaceBet(getTime.GetGameId(), getTime.GetGameRoundIdGenerated(), categoryId, betAmount);
        }

        // If the user has entered a value for the 'seven' category
        if (sevenBet > 0)
        {
            int betAmount = Mathf.RoundToInt(sevenBet);
            string categoryId = "67627b4d69c5f28a27e1c4ae"; // Category ID for 'seven'
            PlaceBet(getTime.GetGameId(), getTime.GetGameRoundIdGenerated(), categoryId, betAmount);

        }

        // If the user has entered a value for the 'eightTotwelve' category
        if (eightTotwelveBet > 0)
        {
            int betAmount = Mathf.RoundToInt(eightTotwelveBet);
            string categoryId = "67627b4d69c5f28a27e1c4ae"; // Category ID for 'eightTotwelve'
            PlaceBet(getTime.GetGameId(), getTime.GetGameRoundIdGenerated(), categoryId, betAmount);

        }
    }

    // Method to set the authentication token
    public void SetToken(string token)
    {
        AuthTok = token;
        Debug.Log("Authentication token set.");
    }

    // Method to get the authentication token
    public string GetToken()
    {
        return AuthTok;
    }

    // Method to place a bet
    public void PlaceBet(string gameRoundId, string gameRoundIdgenerated, string categoryId, int betUnit)
    {
        // Create a new Bet object
        Bet bet = new Bet
        {
            gameRoundId = gameRoundId,
            gameRoundIdgenerated = gameRoundIdgenerated,
            categoryId = categoryId,
            betUnit = betUnit
        };

        Debug.Log($"Placing Bet - GameRound ID: {bet.gameRoundId}, Generated ID: {bet.gameRoundIdgenerated}, Category ID: {bet.categoryId}, Amount: {bet.betUnit}");

        // Serialize the Bet object to JSON
        string jsonBet = JsonConvert.SerializeObject(bet);
        Debug.Log("Bet JSON: " + jsonBet);

        // Start the coroutine to send the bet to the server
        StartCoroutine(SendBetToServer(jsonBet));
    }

    // Coroutine to send the bet to the server
    private IEnumerator SendBetToServer(string jsonBet)
    {
        if (AuthTok != null)
        {
            AuthTok = GetToken();
        }

        string bet_url = live_url + "user/craeteuserbet_dice";

        // Create a new UnityWebRequest
        UnityWebRequest request = new UnityWebRequest(bet_url, "POST");

        // Convert the JSON string to bytes
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBet);

        // Set the request body
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        // Set the request headers
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", AuthTok);
        request.SetRequestHeader("userType", "User");

        // Send the request and wait for the response
        yield return request.SendWebRequest();

        // Check for errors
        if (request.result == UnityWebRequest.Result.Success)
        {
            eightTotwelve_InputField.text = "";
            seven_InputField.text = "";
            twoTosix_InputField.text = "";

            Debug.Log("Bet successfully sent: " + request.downloadHandler.text);
            ShowBetSentResponse("Bet Sent Successfully");

            betHistoryManager.BetHistoryButtonClick();
            lucky7WalletManager.GetWalletBalance();
        }
        else
        {
            Debug.LogError("Error sending bet: " + request.error);
            ShowBetSentResponse("Bet Sent Failed!");

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

// Serializable class for Bet
[System.Serializable]
public class Bet
{
    public string gameRoundId { get; set; }
    public string gameRoundIdgenerated { get; set; }
    public string categoryId { get; set; }
    public float betUnit { get; set; }
}
