using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class SingleTimer : MonoBehaviour
{
    [Header("TextToDisplayDate")]
    [SerializeField] TMP_Text time_display_text;

    [Header("TextToDisplayTime")]
    [SerializeField] TMP_Text text_display;

    [Header("TextToDisplayGameId")]
    [SerializeField] TMP_Text gid_displayed_text;

    [Header("GameID Variables")]
    public string current_gameId;
    public string current_generatedGameId;
    [Header("AuthenticationToken Variable")]
    private string AuthTok;

    [Header("Api Calling Interval Variables")]
    private float apiCallInterval = 1f;
    private float timeSinceLastCall = 0f;

    [Header("Temp Variable to Hold New Generated GameId")]
    private string tempStoredGame_Id = null;

    [Header("Referenced Scripts")]
    SaveUserData svd = new SaveUserData();


    SingleBetHistoryManager singleBetHistoryManager;
    SingleWinHistoryManager singleWinHistoryManager;
    SingleIsWinnerManager singleIsWinnerManager;
    SingleResultManager singleResultManager;
    SingleSlotMachine single_slotMachine;
    //SingleLastTenWinHistoryManager single_lastTenWinHistoryManager;

    [Header("Bet Status Panel")]
    public GameObject betNotAvailablePanel;
    // RouletteBallController rouletteBallController;

    void Start()
    {
        AuthTok = svd.GetSavedAuthToken();
        singleBetHistoryManager = FindFirstObjectByType<SingleBetHistoryManager>();
        singleWinHistoryManager = FindFirstObjectByType<SingleWinHistoryManager>();
        singleIsWinnerManager = FindFirstObjectByType<SingleIsWinnerManager>();
        //single_lastTenWinHistoryManager = FindFirstObjectByType<SingleLastTenWinHistoryManager>();
        single_slotMachine = FindFirstObjectByType<SingleSlotMachine>();
        singleResultManager = FindFirstObjectByType<SingleResultManager>();
    }

    void Update()
    {
        timeSinceLastCall += Time.deltaTime;
        if (timeSinceLastCall >= apiCallInterval)
        {
            StartCoroutine(GetCurrentTime());
            timeSinceLastCall = 0f;
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

    private IEnumerator GetCurrentTime()
    {
        string AuthTok = GetToken();
        string live_url = "http://13.234.117.221:2556/api/v1/user/lastgame_fatafatsingle";

        using (UnityWebRequest request = UnityWebRequest.Get(live_url))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", AuthTok);
            request.SetRequestHeader("userType", "User");
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = request.downloadHandler.text;
                SRDataForTime srData = JsonUtility.FromJson<SRDataForTime>(jsonResponse);
                // Format the remaining time as minutes:seconds
                int minutes = srData.data.remainingminutes;
                int seconds = srData.data.remainingseconds;
                string formattedTime = string.Format("{0}:{1:00}", minutes, seconds);

                text_display.text = formattedTime;

                // Get the game ID and round ID
                current_gameId = srData.data.currrentgame_id;
                current_generatedGameId = srData.data.currentgamegeneratedId;
                gid_displayed_text.text = GetLastFourDigits(current_generatedGameId);
                time_display_text.text = GetDatePortion(current_generatedGameId);


                current_gameId = current_gameId.ToString();
                current_generatedGameId = current_generatedGameId.ToString();

                if (tempStoredGame_Id == null)
                {
                    tempStoredGame_Id = current_gameId;
                    Debug.Log("Temp Id ::" + tempStoredGame_Id);
                }

                if (current_gameId != tempStoredGame_Id)
                {
                    if (singleBetHistoryManager != null)
                    {
                        singleBetHistoryManager.BetHistoryButtonClick();
                    }
                    else
                    {
                        Debug.Log("Win History Manager is Null");
                    }

                    if (singleWinHistoryManager != null)
                    {
                        singleWinHistoryManager.WinHistoryButtonClick();
                    }
                    else
                    {
                        Debug.Log("Bet History Manager is Null");
                    }



                    //if (single_lastTenWinHistoryManager != null)
                    //{
                    //    single_lastTenWinHistoryManager.LastTenWinHistoryButtonClick();
                    //}
                    //else
                    //{
                    //    Debug.Log("Last Ten Win History Manager is Null");
                    //}



                    StartCoroutine(NumberAndVictory());
                    tempStoredGame_Id = null;



                }
                if ((minutes == 2 && seconds == 0) || (minutes <= 1 && seconds > 0))
                {

                    if (single_slotMachine != null)
                    {
                        single_slotMachine.StartSpinning();
                        betNotAvailablePanel.SetActive(true);
                    }



                }
                else
                {
                    if (minutes != 1 && seconds != 0)
                    {
                        betNotAvailablePanel.SetActive(false);
                    }
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

            IEnumerator NumberAndVictory()
            {
                if (singleResultManager != null)
                {
                    singleResultManager.GetChosenNumber();
                }

                yield return new WaitForSeconds(1f);
                if (single_slotMachine != null)
                {
                    Debug.Log("TimeToStop");
                    single_slotMachine.StopSpinning();
                }
            }
        }
    }
    public string GetGameId()
    {
        return current_gameId;
    }

    public string GetGameRoundIdGenerated()
    {
        return current_generatedGameId;
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
}

[System.Serializable]
public class FataFatData
{
    public string currrentgame_id;
    public string currentgamegeneratedId;
    public int remainingminutes;
    public int remainingseconds;
}

[System.Serializable]
public class FataFatDataForTime
{
    public bool status;
    public FataFatData data;
}