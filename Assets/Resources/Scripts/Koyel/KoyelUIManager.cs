using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class KoyelUIManager : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] Button tripura_play_btn;
    [SerializeField] Button dear_play_btn;
    [SerializeField] Button thai_play_btn;

    [Header("BackButton")]
    [SerializeField] Button back_btn;

    [Header("Time Open Buttons")]
    [SerializeField] Button morning_open_btn;
    [SerializeField] Button day_open_btn;
    [SerializeField] Button evening_open_btn;
    [SerializeField] Button night_open_btn;

    [Header("Arrow Buttons")]
    [SerializeField] Button lastDigit_Arrow_btn;
    [SerializeField] Button middleDigit_Arrow_btn;
    [SerializeField] Button lastTwoDigit_Arrow_btn;

    [Header("Time Close View")]
    [SerializeField] GameObject morning_close_view;
    [SerializeField] GameObject day_close_view;
    [SerializeField] GameObject evening_close_view;
    [SerializeField] GameObject night_close_view;

    [Header("Main Panels")]
    [SerializeField] GameObject TimeAndGame_Panel;
    [SerializeField] GameObject Time_Panel;
    [SerializeField] GameObject DigitSelectionPanel;
    [SerializeField] GameObject GamePanel;

    [Header("Bet Panels")]
    [SerializeField] GameObject lastDigitBetPanel;
    [SerializeField] GameObject middleDigitBetPanel;
    [SerializeField] GameObject lastTwoDigitBetPanel;

    [Header("Time Panels")]
    [SerializeField] GameObject time_Morning_bar;
    [SerializeField] GameObject time_Day_bar;
    [SerializeField] GameObject time_Evening_bar;
    [SerializeField] GameObject time_Night_bar;

    [Header("Text Fields")]
    [SerializeField] public TMP_Text game_name;

    [SerializeField] public TMP_Text lastDigit_text;
    [SerializeField] public TMP_Text middleDigit_text;
    [SerializeField] public TMP_Text lastTwoDigit_text;

    [SerializeField] public TMP_Text Title_text;

    private string AuthTok;
    public string GameName, ShiftName,storeGameName;
    
    SaveUserData svd = new SaveUserData();

    #region lastGameStatus Api    
    private string lastgame_api_url = "http://13.234.117.221:2556/api/v1/user/lastgame_koyel";
    #endregion
    public void SetToken(string token)
    {
        AuthTok = token;
        //Debug.Log("Transaction Manager AuthTok::"+AuthTok);
    }

    public string GetToken()
    {
        return AuthTok;
    }
    private void Start()
    {

        AuthTok = svd.GetSavedAuthToken();
        tripura_play_btn.onClick.AddListener(onClickTripuraPlayBtn);
        dear_play_btn.onClick.AddListener(onClickDearPlayBtn);
        thai_play_btn.onClick.AddListener(onClickThaiPlayBtn);

        back_btn.onClick.AddListener(BackFromHomePanel);


        #region open buttons
        morning_open_btn.onClick.AddListener(onClickOpenButton);
        day_open_btn.onClick.AddListener(onClickOpenButton);
        evening_open_btn.onClick.AddListener(onClickOpenButton);
        night_open_btn.onClick.AddListener(onClickOpenButton);
        #endregion

        #region Arrow Buttons
        lastDigit_Arrow_btn.onClick.AddListener(() => onClickArrowButton("lastDigit"));
        middleDigit_Arrow_btn.onClick.AddListener(() => onClickArrowButton("middleDigit"));
        lastTwoDigit_Arrow_btn.onClick.AddListener(() => onClickArrowButton("lastTwoDigit"));

        #endregion
    }
    void ChangeGameName(string name)
    {
        back_btn.onClick.RemoveListener(BackFromHomePanel);
        back_btn.onClick.AddListener(onClickBackButton);
        game_name.text = name;
    }
    void onClickBackButton()
    {
        TimeAndGame_Panel.SetActive(false);
        back_btn.onClick.RemoveListener(onClickBackButton);
        back_btn.onClick.AddListener(BackFromHomePanel);
        //BackFromHomePanel();
    }
    void onClickTripuraPlayBtn()
    {
        Debug.Log("TripuraPlay Button Clicked");

        ChangeGameName("Tripura");
        ShowTimePanel("Tripura");

    }
    void onClickDearPlayBtn()
    {
        Debug.Log("DearPlay Button Clicked");

        ChangeGameName("Dear");
        ShowTimePanel("Dear");
    }

    void onClickThaiPlayBtn()
    {
        Debug.Log("ThaiPlay Button Clicked");

        ChangeGameName("Thailand");
        ShowTimePanel("Thailand");
    }
    void ShowTimePanel(string gameName)
    {
        TimeAndGame_Panel.SetActive(true);
        Time_Panel.SetActive(true);
        switch (gameName)
        {
            case "Tripura":
                ActiveTimeBars(gameName);
                break;
            case "Dear":
                ActiveTimeBars(gameName);
                break;
            case "Thailand":
                ActiveTimeBars(gameName);
                break;
            default:
                Debug.Log("Wong Name..");
                break;
        }
    }

    void ActiveTimeBars(string gameName)
    {
        switch (gameName)
        {
            case "Tripura":
                time_Morning_bar.gameObject.SetActive(true);
                time_Day_bar.gameObject.SetActive(true);
                time_Evening_bar.gameObject.SetActive(true);
                time_Night_bar.gameObject.SetActive(false);
                CheckStatus(gameName);
                break;
            case "Dear":
                time_Morning_bar.gameObject.SetActive(true);
                time_Day_bar.gameObject.SetActive(true);
                time_Evening_bar.gameObject.SetActive(true);
                time_Night_bar.gameObject.SetActive(false);
                CheckStatus(gameName);
                break;
            case "Thailand":
                time_Morning_bar.gameObject.SetActive(true);
                time_Day_bar.gameObject.SetActive(true);
                time_Evening_bar.gameObject.SetActive(true);
                time_Night_bar.gameObject.SetActive(true);
                CheckStatus(gameName);
                break;
            default:
                Debug.Log("wrong digit");
                break;
        }
    }

    void CheckStatus(string gameName)
    {
        // Call the api and check the gameName and the shiftName
        StartCoroutine(GetGameNameAndShift(gameName));

    }


    private IEnumerator GetGameNameAndShift(string gameName)
    {
        AuthTok = GetToken();
        using (UnityWebRequest request = UnityWebRequest.Get(lastgame_api_url))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", AuthTok);
            request.SetRequestHeader("userType", "User");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = request.downloadHandler.text;
                Debug.Log("jsonResponse +" + jsonResponse);
                KoyelResponseDataForGameNameAndShift responseData = JsonUtility.FromJson<KoyelResponseDataForGameNameAndShift>(jsonResponse);
                //Debug.Log("ShiftName : " + responseData.data.shiftname);
                //Debug.Log("GameName : " + responseData.data.gamename);
                string gamename = responseData.data.gamename;
                string shiftName = responseData.data.shiftname;

                //gamename = GameName;
                //shiftName = ShiftName;
                if (gameName.Equals(gamename))
                {

                    if (shiftName.Equals("Evening"))
                    {
                        Debug.Log("Inside");
                        evening_open_btn.gameObject.SetActive(true);
                        evening_close_view.gameObject.SetActive(false);

                        morning_close_view.gameObject.SetActive(true);
                        day_close_view.gameObject.SetActive(true);
                        night_close_view.gameObject.SetActive(true);

                        morning_open_btn.gameObject.SetActive(false);
                        day_open_btn.gameObject.SetActive(false);
                        night_open_btn.gameObject.SetActive(false);
                    }
                    else if (shiftName.Equals("Morning"))
                    {
                        evening_open_btn.gameObject.SetActive(false);
                        evening_close_view.gameObject.SetActive(true);

                        morning_close_view.gameObject.SetActive(false);
                        morning_open_btn.gameObject.SetActive(true);

                        day_close_view.gameObject.SetActive(true);
                        day_open_btn.gameObject.SetActive(false);

                        night_close_view.gameObject.SetActive(true);
                        night_open_btn.gameObject.SetActive(false);
                    }
                    else if (shiftName.Equals("Night"))
                    {
                        evening_open_btn.gameObject.SetActive(false);
                        evening_close_view.gameObject.SetActive(true);

                        morning_close_view.gameObject.SetActive(true);
                        morning_open_btn.gameObject.SetActive(false);

                        day_close_view.gameObject.SetActive(true);
                        day_open_btn.gameObject.SetActive(false);

                        night_close_view.gameObject.SetActive(false);
                        night_open_btn.gameObject.SetActive(true);
                    }
                    else if (shiftName.Equals("Day"))
                    {
                        evening_open_btn.gameObject.SetActive(false);
                        evening_close_view.gameObject.SetActive(true);

                        morning_close_view.gameObject.SetActive(true);
                        morning_open_btn.gameObject.SetActive(false);

                        day_close_view.gameObject.SetActive(false);
                        day_open_btn.gameObject.SetActive(true);

                        night_close_view.gameObject.SetActive(true);
                        night_open_btn.gameObject.SetActive(false);
                    }
                }
                else
                {
                    morning_close_view.SetActive(true);
                    day_close_view.SetActive(true);
                    evening_close_view.SetActive(true);
                    night_close_view.SetActive(true);

                    morning_open_btn.gameObject.SetActive(false);
                    day_open_btn.gameObject.SetActive(false);
                    evening_open_btn.gameObject.SetActive(false);
                    night_open_btn.gameObject.SetActive(false);
                }
            }
            else
            {
                Debug.LogError("Request failed: " + request.error);
            }
        }
    }

    void onClickOpenButton()
    {
        DigitSelectionPanel.SetActive(true);
        Time_Panel.SetActive(false);
        back_btn.onClick.RemoveListener(onClickBackButton);
        back_btn.onClick.AddListener(BackFromDigitPanel);
    }

    void onClickArrowButton(string buttonName)
    {
        GamePanel.SetActive(true);
        DigitSelectionPanel.SetActive(false);
        Time_Panel.SetActive(false);
        back_btn.onClick.RemoveListener(onClickBackButton);
        back_btn.onClick.AddListener(BackFromGamePanel);
        
        Title_text.text = game_name.text;
        storeGameName = game_name.text;
        game_name.text = buttonName;

        switch (buttonName)
        {
            case "lastDigit":
                lastDigitBetPanel.gameObject.SetActive(true);
                middleDigitBetPanel.gameObject.SetActive(false);
                lastTwoDigitBetPanel.gameObject.SetActive(false);
                break;
            case "middleDigit":
                middleDigitBetPanel.gameObject.SetActive(true);
                lastDigitBetPanel.gameObject.SetActive(false);
                lastTwoDigitBetPanel.gameObject.SetActive(false);
                break;
            case "lastTwoDigit":
                lastTwoDigitBetPanel.gameObject.SetActive(true);
                lastDigitBetPanel.gameObject.SetActive(false);
                middleDigitBetPanel.gameObject.SetActive(false);
                break;
            default:
                Debug.Log("Wrong button");
                break;
        }


    }

    void BackFromGamePanel()
    {
        GamePanel.SetActive(false);
        DigitSelectionPanel.SetActive(true);
        Time_Panel.SetActive(false);
        game_name.text = storeGameName;
        back_btn.onClick.RemoveListener(BackFromGamePanel);
        back_btn.onClick.AddListener(BackFromDigitPanel);
    }

    void BackFromDigitPanel()
    {
        DigitSelectionPanel.SetActive(false);     
        Time_Panel.SetActive (true);
        back_btn.onClick.RemoveListener (BackFromDigitPanel);
        back_btn.onClick.AddListener(BackFromTimePanel);
    }

    void BackFromTimePanel()
    {
        back_btn.onClick.RemoveListener(BackFromTimePanel);
        back_btn.onClick.AddListener(onClickBackButton);
    }

    public void BackFromHomePanel()
    {
        Debug.Log("Loading HomePanel..");
        SceneManager.LoadScene("Home");
    }
}
[System.Serializable]
public class KoyelGameDataForGameNameAndShift
{
    public string gamename;
    public string shiftname;
}

[System.Serializable]
public class KoyelResponseDataForGameNameAndShift
{
    public KoyelGameDataForGameNameAndShift data;
}