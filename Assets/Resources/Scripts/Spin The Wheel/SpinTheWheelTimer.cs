using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class SpinTheWheelTimer : MonoBehaviour
{
    [SerializeField] TMP_Text text_display_date;
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
    SpinTheWheelBetHistoryManager spinTheWheelBetHistoryManager;
    SpinTheWheelWinHistoryManager spinTheWheelWinHistoryManager;
    SpinTheWheelIsWinnerManager spinTheWheelIsWinnerManager;
    SpinTheWheelResultManager spinTheWheelResultManager;
    //SpinTheWheelLastTenWinHistoryManager spinTheWheelLastTenWinHistoryManager;
    SpinTheWheelSpinnerController spinTheWheelSpinnerController;

    [Header("Bet Status Panel")]
    public GameObject betNotAvailablePanel;

    void Start()
    {
        AuthTok = svd.GetSavedAuthToken();
        spinTheWheelBetHistoryManager = FindFirstObjectByType<SpinTheWheelBetHistoryManager>();
        spinTheWheelWinHistoryManager = FindFirstObjectByType<SpinTheWheelWinHistoryManager>();
        spinTheWheelIsWinnerManager = FindFirstObjectByType<SpinTheWheelIsWinnerManager>();
        spinTheWheelResultManager = FindFirstObjectByType<SpinTheWheelResultManager>();
        //spinTheWheelLastTenWinHistoryManager = FindFirstObjectByType<SpinTheWheelLastTenWinHistoryManager>();
        spinTheWheelSpinnerController = FindFirstObjectByType<SpinTheWheelSpinnerController>();
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
        string live_url = "http://13.234.117.221:2556/api/v1/user/lastgame_spinwheel";
        using (UnityWebRequest request = UnityWebRequest.Get(live_url))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", AuthTok);
            request.SetRequestHeader("userType", "User");
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = request.downloadHandler.text;
                STWDataForTime pbData = JsonUtility.FromJson<STWDataForTime>(jsonResponse);

                // Format the remaining time as minutes:seconds
                int minutes = pbData.data.remainingminutes;
                int seconds = pbData.data.remainingseconds;
                string formattedTime = string.Format("{0}:{1:00}", minutes, seconds);

                text_display.text = formattedTime;

                // Get the game ID and round ID
                current_gameId = pbData.data.currrentgame_id;
                current_generatedGameId = pbData.data.currentgamegeneratedId;
                gid_displayed_text.text = GetLastFourDigits(current_generatedGameId);
                text_display_date.text = GetDatePortion(current_generatedGameId);

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

                    if (spinTheWheelBetHistoryManager != null)
                    {
                        spinTheWheelBetHistoryManager.BetHistoryButtonClick();
                    }
                    else
                    {
                        Debug.Log("Win History Manager is Null");
                    }

                    if (spinTheWheelWinHistoryManager != null)
                    {
                        spinTheWheelWinHistoryManager.WinHistoryButtonClick();
                    }
                    else
                    {
                        Debug.Log("Bet History Manager is Null");
                    }

                    //if (spinTheWheelLastTenWinHistoryManager != null)
                    //{
                    //    spinTheWheelLastTenWinHistoryManager.LastTenWinHistoryButtonClick();
                    //}
                    //else
                    //{
                    //    Debug.Log("Last Ten Win History Manager is Null");
                    //}


                    tempStoredGame_Id = null;



                }
                if (seconds >= 0 && seconds < 11) 
                {
                    betNotAvailablePanel.SetActive(true);
                    spinTheWheelSpinnerController.SpinLogic();
                }
                else
                {
                    if (seconds >= 11 && seconds <= 60)
                    {
                        betNotAvailablePanel.SetActive(false);
                    }
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
        if (spinTheWheelResultManager != null)
        {
            spinTheWheelResultManager.GetChosenNumber();
        }

        yield return new WaitForSeconds(1f);


        if (spinTheWheelSpinnerController != null)
        {
            spinTheWheelSpinnerController.EndSpin();
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
}
[System.Serializable]
public class STWData
{
    public string currrentgame_id;
    public string currentgamegeneratedId;
    public int remainingminutes;
    public int remainingseconds;
}

[System.Serializable]
public class STWDataForTime
{
    public bool status;
    public PBData data;
}