using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections;


public class KoyelTimer : MonoBehaviour
{
    [SerializeField] TMP_Text text_display_time;
    [SerializeField] TMP_Text gid_displayed_text;
    [SerializeField] TMP_Text text_display_date;
    [SerializeField] public GameObject betNotAvailable_panel;


    SaveUserData svd = new SaveUserData();

    [Header("Game Ids")]
    public string current_generatedGameId;
    public string current_gameId;



    private string AuthTok;
    private float apiCallInterval = 1f; // Set to 5 seconds , ajust as necessary
    private float timeSinceLastCall = 0f;
    private string tempStoredGame_Id = null;
    private string live_url = "http://13.234.117.221:2556/api/v1/user/lastgame_koyel";
    // DiceAnimation diceAnimation = new DiceAnimation();
    // WinHistoryManager winHistoryManager = new WinHistoryManager();
    // LastTenHistoryManager lastTenHistoryManager = new LastTenHistoryManager();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // svd = FindFirstObjectByType<SaveUserData>();
        AuthTok = svd.GetSavedAuthToken();
        // diceAnimation = FindFirstObjectByType<DiceAnimation>();
        // winHistoryManager = FindFirstObjectByType<WinHistoryManager>();
        // lastTenHistoryManager = FindFirstObjectByType<LastTenHistoryManager>();
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
    private IEnumerator GetCurrentTime()
    {
        string AuthTok = GetToken();

        using (UnityWebRequest request = UnityWebRequest.Get(live_url))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", AuthTok);
            request.SetRequestHeader("userType", "User");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = request.downloadHandler.text;
                KoyelResponseDataForTime responseData = JsonUtility.FromJson<KoyelResponseDataForTime>(jsonResponse);

                // Format the remaining time as minutes:seconds
                int minutes = responseData.data.remainingminutes;
                int seconds = responseData.data.remainingseconds;
                string formattedTime = string.Format("{0}:{1:00}", minutes, seconds);

                // Display the formatted time in the text
                // Tripura Lottery 
                text_display_time.text = formattedTime;
               
                // Display the current game ID
                current_gameId = responseData.data.currrentgame_id;
                current_generatedGameId = responseData.data.currentgamegeneratedId;
                // Tripura Lottery
                gid_displayed_text.text = GetLastFourDigits(current_generatedGameId);
                text_display_date.text = GetDatePortion(current_generatedGameId);
               //current_middle_gameIdTextDisplay.text = current_generatedGameId;
               //current_last_gameIdTextDisplay.text = current_generatedGameId;


               current_gameId = current_gameId.ToString();
                current_generatedGameId = current_generatedGameId.ToString();

                if (tempStoredGame_Id == null)
                {

                    tempStoredGame_Id = current_gameId;
                    Debug.Log("Temp Id ::" + tempStoredGame_Id);
                }

                if (current_gameId != tempStoredGame_Id)
                {



                    // if (winHistoryManager != null)
                    // {
                    //     winHistoryManager.WinHistoryButtonClick();
                    // }
                    // else
                    // {
                    //     Debug.Log("Win History Manager is Null");
                    // }

                    // if (lastTenHistoryManager != null)
                    // {
                    //     lastTenHistoryManager.LastTenWinHistoryButtonClick();
                    // }
                    // else
                    // {
                    //     Debug.Log("Last Ten Win History Manager is Null");
                    // }

                    // Result();

                    tempStoredGame_Id = null;

                }


                if ((minutes == 2 && seconds == 0) || (minutes <= 1 && seconds > 0))
                {

                    // if (diceAnimation != null)
                    // {
                    //     float remainingTime = minutes * 60 + seconds;
                    //     diceAnimation.RollDiceButton(remainingTime);
                    // }


                    // betNotAvailable_panel.SetActive(true);


                }
                else
                {
                    // betNotAvailable_panel.SetActive(false);

                }
            }
            else
            {
                Debug.LogError("Request failed: " + request.error);
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
}

[System.Serializable]
public class KoyelGameDataForTime
{
    public string currrentgame_id;
    public string currentgamegeneratedId;
    public int remainingminutes;
    public int remainingseconds;
}

[System.Serializable]
public class KoyelResponseDataForTime
{
    public bool status;
    public GameDataForTime data;
}
