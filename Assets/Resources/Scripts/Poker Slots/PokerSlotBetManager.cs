using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Linq;
public class PokerSlotBetManager : MonoBehaviour
{
    [Header("DialogBoxNumber Text")]
    [SerializeField] TMP_Text dialogBoxNumber_Text;

    [Header("Authentication Token")]
    private string AuthTok;

    [Header("Api Url")]
    private string pokerSlot_CreateBetLiveUrl = "http://13.234.117.221:2556/api/v1/user/createUserBet_poker";

    [Header("Script References")]
    PokerSlotsTimer pokerslotsTimer;
    SaveUserData svd = new SaveUserData();

    PokerSlotBetHistoryManager pokerSlotBetHistoryManager;
    PokerSlotsWalletManager pokerSlotWalletManager;

    public TMP_Text responseText;
    public GameObject buttonParent_pokerSlots;
    private List<string> betNumbers = new List<string>();
    public List<Button> buttons_pokerSlots = new List<Button>();
    public GameObject dialogBox;
    public TMP_InputField moneyInputField;
    public  string selectedColor;
    public string selectedRoom;
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
        dialogBox.SetActive(false);
        pokerSlotBetHistoryManager = FindFirstObjectByType<PokerSlotBetHistoryManager>();
        pokerslotsTimer = FindFirstObjectByType<PokerSlotsTimer>();
        pokerSlotWalletManager = FindFirstObjectByType<PokerSlotsWalletManager>();
        AuthTok = svd.GetSavedAuthToken();
        InitializeButtonsOfPattiPanel();

    }
    public void OnCloseDialogBox()
    {
        dialogBox.SetActive(false);
    }
    void InitializeButtonsOfPattiPanel()
    {
        Button[] childButtons = buttonParent_pokerSlots.GetComponentsInChildren<Button>();
        buttons_pokerSlots.AddRange(childButtons);


        Debug.Log("Total Buttons Initialized: " + buttons_pokerSlots.Count);

        for (int i = 0; i < buttons_pokerSlots.Count; i++)
        {
            Button button = buttons_pokerSlots[i];
            button.onClick.AddListener(() => OnButtonClick(button.name));
        }


    }
    public void OnButtonClick(string boxNumber)
    {
        Debug.Log("BoxNo" + boxNumber);
        dialogBox.SetActive(true);
        dialogBoxNumber_Text.text = boxNumber;
        betNumbers.Add(boxNumber);
    }

    public void OnSubmitBet()
    {
     

        if (!string.IsNullOrEmpty(moneyInputField.text))
        {
            int betAmount = int.Parse(moneyInputField.text);
            string input = dialogBoxNumber_Text.text;
            string[] parts = input.Split(' ');

            if (parts.Length == 2) 
            { 

                 selectedColor = parts[0];
                  selectedRoom = parts[1];
                //Debug.Log("Selected Color: " + selectedColor);
                //Debug.Log("Selected Room: " + selectedRoom);
            }
            Debug.Log(betNumbers.Count() + "Total Bet");
            if (betNumbers.Count() > 0)
            {
                string value = betNumbers.Last().ToString();
                

                StartCoroutine(SendBetToServer(betAmount,selectedColor, selectedRoom, pokerslotsTimer.GetGameId(), pokerslotsTimer.GetGameRoundIdGenerated()));
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


    IEnumerator SendBetToServer(int betAmount,string color, string room, string gameRoundId, string gameRoundIdgenerated)
    {
        Debug.Log("BetAmount = " + betAmount + " GameRoundId = " + gameRoundId + " GameRoundIdgenerated =" + gameRoundIdgenerated + "Room : " + room+"Color : "+color);

        if (AuthTok == null)
        {
            AuthTok = GetToken();
        }
        PokerSlotBetData betdata = new PokerSlotBetData()
        {
            selectedColor= color,
            selectedRoom= room,
            betUnit = betAmount,
        };


        PSBet bet = new PSBet
        {
            gameRoundId = gameRoundId,
            gameRoundIdgenerated = gameRoundIdgenerated,
            bets = new List<PokerSlotBetData> { betdata },
        };

        //PowerBallBetData betData = new PowerBallBetData()
        //{
        //    bets = new List<PowerBallBet> { bet }
        //};

        string jsonData = JsonConvert.SerializeObject(bet);
        Debug.Log("Serialized JSON Data: " + jsonData);

        using (UnityWebRequest request = new UnityWebRequest(pokerSlot_CreateBetLiveUrl, "POST"))
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
                pokerSlotBetHistoryManager.BetHistoryButtonClick();
                pokerSlotWalletManager.GetWalletBalance();
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
public class PokerSlotBetData
{
    public string selectedColor { get; set; }
    public string selectedRoom { get; set; }
    public int betUnit { get; set; }
}

public class PSBet
{
    public string gameRoundId { get; set; }
    public string gameRoundIdgenerated { get; set; }
    public List<PokerSlotBetData> bets { get; set; }
}
