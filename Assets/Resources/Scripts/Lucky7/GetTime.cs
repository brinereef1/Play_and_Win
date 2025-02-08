using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections;
using Newtonsoft.Json;
using System.Collections.Generic;

public class GetTime : MonoBehaviour
{


    [SerializeField] TMP_Text text_display;
    [SerializeField] TMP_Text gid_displayed_text;
    [SerializeField] TMP_Text text_display_date;
    [SerializeField] public GameObject betNotAvailable_panel;


    SaveUserData svd = new SaveUserData();
    public string current_generatedGameId;
    public string current_gameId;
    private string AuthTok;
    private float apiCallInterval = 1f; // Set to 5 seconds , ajust as necessary
    private float timeSinceLastCall = 0f;
    private string tempStoredGame_Id = null;
    private string live_url = "http://13.234.117.221:2556/api/v1";
    DiceAnimation diceAnimation = new DiceAnimation();

    WinHistoryManager winHistoryManager = new WinHistoryManager();
    //LastTenHistoryManager lastTenHistoryManager = new LastTenHistoryManager();
    BetHistoryManager betHistoryManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // svd = FindFirstObjectByType<SaveUserData>();
        AuthTok = svd.GetSavedAuthToken();
        diceAnimation = FindFirstObjectByType<DiceAnimation>();
        winHistoryManager = FindFirstObjectByType<WinHistoryManager>();
        //lastTenHistoryManager = FindFirstObjectByType<LastTenHistoryManager>();
        betHistoryManager = FindFirstObjectByType<BetHistoryManager>();
        // Result();
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

        string getTime_url = live_url + "/user/lastgame_dicegame";

        using (UnityWebRequest request = UnityWebRequest.Get(getTime_url))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", AuthTok);
            request.SetRequestHeader("userType", "User");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = request.downloadHandler.text;
                ResponseDataForTime responseData = JsonUtility.FromJson<ResponseDataForTime>(jsonResponse);

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
                text_display_date.text  = GetDatePortion(current_generatedGameId);

                current_gameId = current_gameId.ToString();
                current_generatedGameId = current_generatedGameId.ToString();

                if (tempStoredGame_Id == null)
                {

                    tempStoredGame_Id = current_gameId;
                    Debug.Log("Temp Id ::" + tempStoredGame_Id);
                }

                if (current_gameId != tempStoredGame_Id)
                {
                    Result();

                    if (winHistoryManager != null)
                    {
                        winHistoryManager.WinHistoryButtonClick();
                    }
                    else
                    {
                        Debug.Log("Win History Manager is Null");
                    }

                    //if (lastTenHistoryManager != null)
                    //{
                    //    lastTenHistoryManager.LastTenWinHistoryButtonClick();
                    //}
                    //else
                    //{
                    //    Debug.Log("Last Ten Win History Manager is Null");
                    //}

                    if (betHistoryManager != null)
                    {
                        betHistoryManager.BetHistoryButtonClick();
                    }
                    else
                    {
                        Debug.Log("Bet History Manager is Null");
                    }


               

                    tempStoredGame_Id = null;

                }


                if (seconds >= 0 && seconds < 11)
                {

                    if (diceAnimation != null)
                    {
                        float remainingTime = seconds;
                        diceAnimation.RollDiceButton(remainingTime);
                    }

                    // Debug.Log("Dice is rollign.");
                    betNotAvailable_panel.SetActive(true);


                }
                else
                {
                    if (seconds >= 11 && seconds <= 60)
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
    public void Result()
    {
        StartCoroutine(SendRequestForResult());
    }




    IEnumerator SendRequestForResult()
    {

        string result_live_url = live_url + "/user/lastgameinfo_dicegame";
        if (AuthTok == "")
        {
            AuthTok = GetToken();
        }

        using (UnityWebRequest request = UnityWebRequest.Get(result_live_url))
        {

            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", AuthTok);
            request.SetRequestHeader("userType", "User");

            yield return request.SendWebRequest();
            string jsonResponse = request.downloadHandler.text;
            //Debug.Log("Response from lucky7 get time: " + jsonResponse);
            Result dataForResult = JsonConvert.DeserializeObject<Result>(jsonResponse);

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log(dataForResult.data.showDice[0]);
                Debug.Log(dataForResult.data.showDice[1]);


                diceAnimation.AlignTheDices(dataForResult.data.showDice[0], dataForResult.data.showDice[1]);

            }
            else
            {
                Debug.LogError("Request failed: " + request.error);
            }

        }

        yield return null;
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
public class GameDataForTime
{
    public string currrentgame_id;
    public string currentgamegeneratedId;
    public int remainingminutes;
    public int remainingseconds;
}

[System.Serializable]
public class ResponseDataForTime
{
    public bool status;
    public GameDataForTime data;
}

[System.Serializable]
public class DataForResult
{
    public List<int> showDice { get; set; }
    public string chosenCard { get; set; }
}

[System.Serializable]
public class Result
{
    public bool status { get; set; }
    public DataForResult data { get; set; }
}