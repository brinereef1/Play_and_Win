using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections;
using Newtonsoft.Json;
using System.Collections.Generic;

public class LuckyLottoTimer : MonoBehaviour
{

    [Header("DisplayTimeText")]
    [SerializeField] TMP_Text date_display_text;

    [SerializeField] TMP_Text text_display;
    [SerializeField] TMP_Text gid_displayed_text;
    [SerializeField] public GameObject betNotAvailable_panel;
    [SerializeField] public GameObject defaultCards_panel;

    SaveUserData svd = new SaveUserData();
    public string current_generatedGameId;
    public string current_gameId;
    private string AuthTok;
    private float apiCallInterval = 1f; // Set to 5 seconds , ajust as necessary
    private float timeSinceLastCall = 0f;
    private string tempStoredGame_Id = null;
    private string live_url = "http://13.234.117.221:2556/api/v1";
    LuckyLottoSlotMachine luckyLottoSlotMachine;
    LuckyLottoWinHistoryManager luckyLottoWinHistoryManager;
    LuckyLottoBetHistoryManager luckyLottoBetHistoryManager;
    //LuckyLottoLastTenWinHistoryManager luckyLottoLastTenWinHistoryManager;
    LuckyLottoResultManger luckyLottoResultManger;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // svd = FindFirstObjectByType<SaveUserData>();
        defaultCards_panel.SetActive(true);
        AuthTok = svd.GetSavedAuthToken();
        luckyLottoBetHistoryManager = FindFirstObjectByType<LuckyLottoBetHistoryManager>();
        luckyLottoWinHistoryManager = FindFirstObjectByType<LuckyLottoWinHistoryManager>();
        //luckyLottoLastTenWinHistoryManager = FindFirstObjectByType<LuckyLottoLastTenWinHistoryManager>();
        luckyLottoSlotMachine = FindFirstObjectByType<LuckyLottoSlotMachine>();
        luckyLottoResultManger = FindFirstObjectByType<LuckyLottoResultManger>();
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastCall += Time.deltaTime;
        if (timeSinceLastCall >= apiCallInterval)
        {
            StartCoroutine(GetCurrentTime());
            timeSinceLastCall = 0f; // Reset timer after making the call
        }
    }

    public void SetToken(string token)
    {
        AuthTok = token;
        //Debug.Log("Transaction Manager AuthTok::"+AuthTok);
    }

    public string GetToken()
    {
        return AuthTok;
    }

    private IEnumerator GetCurrentTime()
    {
        string AuthTok = GetToken();

        string getTime_url = live_url + "/user/lastgame";

        using (UnityWebRequest request = UnityWebRequest.Get(getTime_url))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", AuthTok);
            request.SetRequestHeader("userType", "User");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = request.downloadHandler.text;
                LuckyLottoResponseDataForTime responseData = JsonUtility.FromJson<LuckyLottoResponseDataForTime>(jsonResponse);

                // Format the remaining time as minutes:seconds
                int minutes = responseData.data.remainingminutes;
                int seconds = responseData.data.remainingseconds;
                string formattedTime = string.Format("{0}:{1:00}", minutes, seconds);

                // Display the formatted time in the text
                text_display.text = formattedTime;

                // Display the current game ID
                current_gameId = responseData.data.currrentgame_id;
                current_generatedGameId = responseData.data.currentgamegeneratedId;
                gid_displayed_text.text = GetLastFourDigits(current_generatedGameId);
                date_display_text.text = GetDatePortion(current_generatedGameId);

                current_gameId = current_gameId.ToString();
                current_generatedGameId = current_generatedGameId.ToString();

                if (tempStoredGame_Id == null)
                {

                    tempStoredGame_Id = current_gameId;
                    Debug.Log("Temp Id ::" + tempStoredGame_Id);
                }

                if (current_gameId != tempStoredGame_Id)
                {
                    StartCoroutine(NumberAndVictory());


                    if (luckyLottoBetHistoryManager != null)
                    {
                        luckyLottoBetHistoryManager.BetHistoryButtonClick();
                    }
                    else
                    {
                        Debug.Log("Win History Manager is Null");
                    }

                    if (luckyLottoWinHistoryManager != null)
                    {
                        luckyLottoWinHistoryManager.WinHistoryButtonClick();
                    }
                    else
                    {
                        Debug.Log("Bet History Manager is Null");
                    }

                    //if (luckyLottoLastTenWinHistoryManager != null)
                    //{
                    //    luckyLottoLastTenWinHistoryManager.LastTenWinHistoryButtonClick();
                    //}
                    //else
                    //{
                    //    Debug.Log("Last Ten Win History Manager is Null");
                    //}



                    tempStoredGame_Id = null;

                }


                if (minutes == 0 && seconds < 11)
                {

                    if (luckyLottoSlotMachine != null)
                    {
                        luckyLottoSlotMachine.StartSpinning();
                    }
                    betNotAvailable_panel.SetActive(true);
                    defaultCards_panel.SetActive(false);

                }
                else
                {
                    if ((minutes == 1 && seconds <= 30) || (minutes == 0 && seconds >= 40))
                    {

                        betNotAvailable_panel.SetActive(false);
                       
                    }
                 

                }
            }
            else
            {
                Debug.LogError("Request failed: " + request.error);
            }
        }


}


    IEnumerator NumberAndVictory()
    {
        if (luckyLottoResultManger != null)
        {
            luckyLottoResultManger.GetChosenNumber();
        }

        yield return new WaitForSeconds(.5f); // changes made here
        if (luckyLottoSlotMachine != null)
        {
            Debug.Log("TimeToStop");
            luckyLottoSlotMachine.StopSpinning();
        }

    }
    string GetLastFourDigits(string input)
    {
        // Check if the string has at least 4 characters
        if (input.Length >= 4)
        {
            return input.Substring(input.Length - 4); // Extract last 4 characters
        }
        else
        {
            return input; // Return the full string if it's shorter than 4 characters
        }
    }

    string GetDatePortion(string input)
    {
        // Split the string using '/' as the delimiter
        string[] parts = input.Split('/');
        if (parts.Length > 0)
        {
            return parts[0]; // Return the first part (the date)
        }
        return string.Empty; // Return an empty string if splitting fails
    }

    public string GetGameId()
    {
        return current_gameId;
    }

    public string GetGameRoundIdGenerated()
    {
        return current_generatedGameId;
    }
}

[System.Serializable]
public class LuckyLottoGameDataForTime
{
    public string currrentgame_id;
    public string currentgamegeneratedId;
    public int remainingminutes;
    public int remainingseconds;
}

[System.Serializable]
public class LuckyLottoResponseDataForTime
{
    public bool status;
    public LuckyLottoGameDataForTime data;
}
