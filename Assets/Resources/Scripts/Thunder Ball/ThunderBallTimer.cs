using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class ThunderBallTimer : MonoBehaviour
{
    [Header("TextToDisplayTime")]
    [SerializeField] TMP_Text text_display;

    [Header("TextToDisplayGameId")]
    [SerializeField] TMP_Text gid_displayed_text;

    [Header("TextToDisplayDate")]
    [SerializeField] TMP_Text date_displayed_text;

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
    ThunderBallBetHistoryManager thunderBallBetHistoryManager;
    ThunderBallWinHistoryManager thunderBallWinHistoryManager;
    ThunderBallIsWinnerManager thunderBallIsWinnerManager;
    ThunderBallSelectedBall thunderBallSelectedBall;
    //ThunderBallLastTenWinHistoryManager thunderBallLastTenWinHistoryManager;
    ThunderBallSpin thunderBallSpin;
    [Header("Bet Status Panel")]
    public GameObject betNotAvailablePanel;


    void Start()
    {
        AuthTok = svd.GetSavedAuthToken();
        thunderBallBetHistoryManager = FindFirstObjectByType<ThunderBallBetHistoryManager>();
        thunderBallWinHistoryManager = FindFirstObjectByType<ThunderBallWinHistoryManager>();
        thunderBallIsWinnerManager = FindFirstObjectByType<ThunderBallIsWinnerManager>();
        thunderBallSelectedBall = FindFirstObjectByType<ThunderBallSelectedBall>();
        //thunderBallLastTenWinHistoryManager = FindFirstObjectByType<ThunderBallLastTenWinHistoryManager>();
        thunderBallSpin = FindFirstObjectByType<ThunderBallSpin>();
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
        string live_url = "http://13.234.117.221:2556/api/v1/user/lastgame_thunder";

        using (UnityWebRequest request = UnityWebRequest.Get(live_url))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", AuthTok);
            request.SetRequestHeader("userType", "User");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = request.downloadHandler.text;
                TBDataForTime psDataForTime = JsonUtility.FromJson<TBDataForTime>(jsonResponse);
                // Format the remaining time as minutes:seconds
                int minutes = psDataForTime.data.remainingminutes;
                int seconds = psDataForTime.data.remainingseconds;
                string formattedTime = string.Format("{0}:{1:00}", minutes, seconds);

                // Display the formatted time in the text
                text_display.text = formattedTime;

                // Display the current game ID
                current_gameId = psDataForTime.data.currrentgame_id;
                current_generatedGameId = psDataForTime.data.currentgamegeneratedId;
                gid_displayed_text.text = GetLastFourDigits(current_generatedGameId);
                date_displayed_text.text = GetDatePortion(current_generatedGameId);

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

                    if (thunderBallBetHistoryManager != null)
                    {
                        thunderBallBetHistoryManager.BetHistoryButtonClick();
                    }
                    else
                    {
                        Debug.Log("Win History Manager is Null");
                    }

                    if (thunderBallWinHistoryManager != null)
                    {
                        thunderBallWinHistoryManager.WinHistoryButtonClick();
                    }
                    else
                    {
                        Debug.Log("Bet History Manager is Null");
                    }

                    //if (thunderBallLastTenWinHistoryManager != null)
                    //{
                    //    thunderBallLastTenWinHistoryManager.LastTenWinHistoryButtonClick();
                    //}
                    //else
                    //{
                    //    Debug.Log("Last Ten Win History Manager is Null");
                    //}

                    tempStoredGame_Id = null;

                }

                if ((minutes == 0 && seconds < 11) && (minutes == 0 && seconds >= 0))
                {
                    betNotAvailablePanel.SetActive(true);
                }
                else
                {
                    if ((minutes >= 1 && seconds <= 30) || (minutes == 0 && seconds <= 60 && seconds > 40))
                    {
                        betNotAvailablePanel.SetActive(false);
                    }
                }

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


        if (thunderBallSelectedBall != null)
        {
            thunderBallSelectedBall.GetChosenNumber();
        }
        yield return new WaitForSeconds(1f);

        if (thunderBallSpin != null)
        {
            thunderBallSpin.StartApplyingForce();
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
public class TBData
{
    public string currrentgame_id;
    public string currentgamegeneratedId;
    public int remainingminutes;
    public int remainingseconds;
}

[System.Serializable]
public class TBDataForTime
{
    public bool status;
    public TBData data;
}