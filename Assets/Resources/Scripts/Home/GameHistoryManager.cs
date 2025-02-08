using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class GameHistoryManager : MonoBehaviour
{
    #region buttons
    [Header("history Buttons")]
    [SerializeField] Button GameHistory_Button;
    [SerializeField] Button BetHistory_Button;
    [SerializeField] Button WinHistory_Button;
    #endregion

    #region gamesbuttons
    [Header("GamesButtons")]
    [SerializeField] Button spinTheWheel_Button;
    [SerializeField] Button luckySeven_Button;
    [SerializeField] Button thunderBall_Button;
    [SerializeField] Button pokerSlots_Button;
    [SerializeField] Button superRoulette_Button;
    [SerializeField] Button powerBall_Button;
    [SerializeField] Button jackpot_Button;
    [SerializeField] Button jhandiMunda_Button;

    [Header("FataFat")]
    [SerializeField] Button FataFat_Button;
    [SerializeField] Button patti_Button;
    [SerializeField] Button single_Button;
    #endregion

    #region panels
    [Header("Panels")]
    [SerializeField] GameObject gameHistory_Panel;
    [SerializeField] GameObject choose_Panel;
    [SerializeField] GameObject winHistory_Panel;
    [SerializeField] GameObject betHistory_Panel;

    [Header("FataFat Panels")]
    [SerializeField] GameObject choose_patti_or_single_panel;
    #endregion

    #region back buttons
    [Header("Back Buttons")]
    [SerializeField] Button gameHistoryPanel_BackButton;
    [SerializeField] Button fatafatPanel_BackButton;
    [SerializeField] Button choosePanel_BackButton;
    [SerializeField] Button winHistory_BackButton;
    [SerializeField] Button betHistory_BackButton;
    #endregion

    #region texts
    [Header("Choose Panel Text")]
    [SerializeField] public TMP_Text game_name;
    #endregion

    [Header("WinPrefab Parent")]
    public Transform win_prefabParent;
    [Header("BetPrefab Parent")]
    public Transform bet_prefabParent;

    [Header("winPrefab")]
    public GameObject winPrefab;
    [Header("betPrefab")]
    public GameObject betPrefab;

    [Header("Script References")]
    SaveUserData svd = new SaveUserData();
    public string AuthTok;

    #region winHistory api urls
    private string spinTheWheel_win_history_api_url = "http://13.234.117.221:2556/api/v1/user/wiininghistory_spinwheel";
    private string roulette_win_history_api_url = "http://13.234.117.221:2556/api/v1/user/wiininghistory_roulette";
    private string thunderball_win_history_api_url = "http://13.234.117.221:2556/api/v1/user/userwinhistory_thunder";
    private string powerball_win_history_api_url = "http://13.234.117.221:2556/api/v1/user/userwinhistory_power";
    private string poker_win_history_api_url = "http://13.234.117.221:2556/api/v1/user/userwinhistory_poker2";
    private string jackpot_win_history_api_url = "http://13.234.117.221:2556/api/v1/user/winhistory";
    private string lucky7_win_history_api_url = "http://13.234.117.221:2556/api/v1/user/userwinhistiry_dice";
    private string jhandiMunda_win_history_api_url = "http://13.234.117.221:2556/api/v1/user/wiininghistory_jhandimunda";
    private string patti_win_history_api_url = "http://13.234.117.221:2556/api/v1/user/wiininghistory_fatafat";
    private string single_win_history_api_url = "http://13.234.117.221:2556/api/v1/user/wiininghistory_fatafatsingle";

    #endregion api urls

    #region betHistory api urls
    private string single_bet_history_api_url = "http://13.234.117.221:2556/api/v1/user/userbethistory_fatfafatsingle";
    private string patti_bet_history_api_url = "http://13.234.117.221:2556/api/v1/user/userbethistory_fatfafat";
    private string jm_bet_history_api_url = "http://13.234.117.221:2556/api/v1/user/userbethistory_jhandimunda";
    private string jackpot_bet_history_api_url = "http://13.234.117.221:2556/api/v1/user/bethistory";
    private string roulette_bet_history_api_url = "http://13.234.117.221:2556/api/v1/user/userbethistory_roulette";
    private string lucky7_bet_history_api_url = "http://13.234.117.221:2556/api/v1/user/userbethistory_dice";
    private string pokerslots_bet_history_api_url = "http://13.234.117.221:2556/api/v1/user/userbethistory_poker2";
    private string powerball_bet_history_api_url = "http://13.234.117.221:2556/api/v1/user/userbethistory_power";
    private string thunderball_bet_history_api_url = "http://13.234.117.221:2556/api/v1/user/userbethistory_thunder";
    private string spinThewheel_bet_history_api_url = "http://13.234.117.221:2556/api/v1/user/userbethistory_spinwheel";

    #endregion 
    void Start()
    {
        InitializeButtons();
        AuthTok = svd.GetSavedAuthToken();
    }
    public void SetToken(string token)
    {
        AuthTok = token;
    }
    public string GetToken()
    {
        return AuthTok;
    }
    private void InitializeButtons()
    {
        #region games buttons
        if (GameHistory_Button != null)
        {
            GameHistory_Button.onClick.AddListener(ActiveGameHistoryPanel);
        }
        else
        {
            Debug.LogError("GameHistory_button is null.");
        }

        if (gameHistoryPanel_BackButton != null)
        {
            gameHistoryPanel_BackButton.onClick.AddListener(GameHistoryPanel_BackButton);
        }
        else
        {
            Debug.LogError("gameHistoryPanel_BackButton is null.");
        }

        if (fatafatPanel_BackButton != null)
        {
            fatafatPanel_BackButton.onClick.AddListener(FataFatPanel_BackButton);

        }
        else
        {
            Debug.LogError("fatafatPanel_backbutton is null.");
        }



        if (spinTheWheel_Button != null)
        {
            spinTheWheel_Button.onClick.AddListener(onClickSpinTheWheelButton);
        }
        else
        {
            Debug.LogError("SpinTheButton is null.");
        }

        if (spinTheWheel_Button != null)
        {
            spinTheWheel_Button.onClick.AddListener(onClickSpinTheWheelButton);
        }
        else
        {
            Debug.LogError("spinTheWheel button is null.");
        }

        if (luckySeven_Button != null)
        {
            luckySeven_Button.onClick.AddListener(onClickLuckySevenButton);
        }
        else
        {
            Debug.LogError("luckySeven button is null");
        }

        if (thunderBall_Button != null)
        {
            thunderBall_Button.onClick.AddListener(onClickThunderBallButton);
        }
        else
        {
            Debug.Log("thunderball button is null.");
        }

        if (powerBall_Button != null)
        {
            powerBall_Button.onClick.AddListener(onClickPowerBallButton);
        }
        else
        {
            Debug.LogError("powerball button is null.");
        }

        if (pokerSlots_Button != null)
        {
            pokerSlots_Button.onClick.AddListener(onClickPokerSlotsButton);
        }
        else
        {
            Debug.LogError("pokerslots button is null.");
        }

        if (superRoulette_Button != null)
        {
            superRoulette_Button.onClick.AddListener(onClickRouletteButton);
        }
        else
        {
            Debug.LogError("superRoulette button is null.");
        }

        if (jhandiMunda_Button != null)
        {
            jhandiMunda_Button.onClick.AddListener(onClickJhandimundaButton);
        }
        else
        {
            Debug.LogError("jhandiMunda button is null.");
        }

        if (jackpot_Button != null)
        {
            jackpot_Button.onClick.AddListener(onClickJackpotButton);
        }
        else
        {
            Debug.LogError("jackpot button is null.");
        }


        if (FataFat_Button != null)
        {
            FataFat_Button.onClick.AddListener(onClickFataFatButton);
        }
        else
        {
            Debug.LogError("FataFat button is null.");
        }

        if (patti_Button != null)
        {
            patti_Button.onClick.AddListener(onClickPattiButton);
        }
        else
        {
            Debug.LogError("pattiButton is null.");
        }

        if (single_Button != null)
        {
            single_Button.onClick.AddListener(onClickSingleButton);
        }
        else
        {
            Debug.LogError("singleButton is null.");
        }

        if (choosePanel_BackButton != null)
        {
            choosePanel_BackButton.onClick.AddListener(ChoosePanel_BackButton);
        }
        else
        {
            Debug.LogError("ChoosePanelBackButton is null.");
        }

        #endregion games buttons

        #region bet and win history buttons initialization
        if (BetHistory_Button != null)
        {
            BetHistory_Button.onClick.AddListener(onClickBetHistoryButton);
        }
        else
        {
            Debug.LogError("BetHistory button is null.");
        }

        if (WinHistory_Button != null)
        {
            WinHistory_Button.onClick.AddListener(onClickWinHistoryButton);
        }
        else
        {
            Debug.LogError("WinHistory button is null.");
        }

        if (betHistory_BackButton != null)
        {
            betHistory_BackButton.onClick.AddListener(BetHistory_BackButton);
        }
        else
        {
            Debug.LogError("betHistory_BackButton is null.");
        }

        if (winHistory_BackButton != null)
        {

            winHistory_BackButton.onClick.AddListener(WinHistory_BackButton);
        }
        else
        {
            Debug.LogError("winHistory_BackButton is null.");
        }


        #endregion bet and win history
    }
    public void ClearWins()
    {
        if (win_prefabParent != null)
        {
            foreach (Transform child in win_prefabParent)
            {
                if (child != null)
                {
                    Destroy(child.gameObject);
                }
            }
        }
    }

    public void ClearBets()
    {
        if (bet_prefabParent != null)
        {
            foreach (Transform child in bet_prefabParent)
            {
                if (child != null)
                {
                    Destroy(child.gameObject);
                }
            }
        }
    }

    #region winHistory 
    void onClickWinHistoryButton()
    {
        Debug.Log("GameName" + game_name.text);
        string gameName = game_name.text;
        winHistory_Panel.SetActive(true);

        LoadWinHistory(gameName);
    }

    private void LoadWinHistory(string gameName)
    {
        switch (gameName)
        {
            case "Spin The Wheel":
                Debug.Log("SpinTheWheel Matched");
                LoadSpinTheWheel();
                break;
            case "Thunder Ball":
                Debug.Log("ThunderBall Matched");
                LoadThunderBall();
                break;
            case "Power Ball":
                Debug.Log("PowerBall Matched");
                LoadPowerBall();
                break;
            case "PokerSlots":
                Debug.Log("PokerSlots Matched");
                LoadPokerSlots();
                break;
            case "Lucky7":
                Debug.Log("Lucky7 Matched");
                LoadLuckySeven();
                break;
            case "Super Roulette":
                Debug.Log("Super Roulette Matched");
                LoadRoulette();
                break;
            case "Jackpot":
                Debug.Log("Jackpot Matched");
                LoadJackpot();
                break;
            case "Jhandimunda":
                Debug.Log("Jhandimunda Matched");
                LoadJhandimunda();
                break;
            case "Patti":
                Debug.Log("Patti Matched");
                LoadPatti();
                break;
            case "Single":
                Debug.Log("Single Matched");
                LoadSingle();
                break;
            default:
                Debug.Log("Game doesn't exist.");
                break;

        }
    }


    #region spinTheWheel
    private void LoadSpinTheWheel()
    {
        WinHistoryButtonClick();
    }
    public void WinHistoryButtonClick()
    {
        ClearWins();
        StartCoroutine(WinHistoryRequest());
    }

    IEnumerator WinHistoryRequest()
    {
        Debug.Log("WinHistoryCalled");
        AuthTok = GetToken();
        Debug.Log(AuthTok);

        using (UnityWebRequest request = UnityWebRequest.Get(spinTheWheel_win_history_api_url))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", AuthTok);
            request.SetRequestHeader("userType", "User");
            yield return request.SendWebRequest();
            string response = request.downloadHandler.text;
            HomeSRWinResponse superRouletteWinResponse = JsonConvert.DeserializeObject<HomeSRWinResponse>(response);
            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log(response);
                foreach (var item in superRouletteWinResponse.data)
                {
                    GameObject win = Instantiate(winPrefab, win_prefabParent);
                    var Script = win.transform.GetComponent<WinHistoryDisplayer>();

                    Script.SetWinData(item.betAmount, item.winningAmount, item.gameRoundId);
                }
            }
            else
            {
                Debug.Log("Error: " + request.error);
            }
        }


    }


    [System.Serializable]
    public class HomeSRBallDatum
    {
        public string gameRoundId { get; set; }
        public int betAmount { get; set; }
        public int winningAmount { get; set; }
    }
    [System.Serializable]
    public class HomeSRWinResponse
    {
        public bool success { get; set; }
        public List<HomeSRBallDatum> data { get; set; }
    }
    #endregion spinTheWheel

    #region ThunderBall
    private void LoadThunderBall()
    {
        ThunderBallWinHistoryButtonClick();
    }

    public void ThunderBallWinHistoryButtonClick()
    {
        ClearWins();
        StartCoroutine(ThunderBallWinHistoryRequest());
    }

    IEnumerator ThunderBallWinHistoryRequest()
    {
        Debug.Log("WinHistoryCalled");
        AuthTok = GetToken();
        Debug.Log(AuthTok);

        using (UnityWebRequest request = UnityWebRequest.Get(thunderball_win_history_api_url))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", AuthTok);
            request.SetRequestHeader("userType", "User");
            yield return request.SendWebRequest();

            string response = request.downloadHandler.text;
            Debug.Log("Response: " + response);
            HomeThunderBallResponse winResponse = JsonConvert.DeserializeObject<HomeThunderBallResponse>(response);
            if (request.result == UnityWebRequest.Result.Success)
            {

                foreach (var item in winResponse.data)
                {

                    // Instantiate the win history object
                    GameObject win = Instantiate(winPrefab, win_prefabParent);
                    var Script = win.transform.GetComponent<WinHistoryDisplayer>();

                    // Set the values including the formatted IST date
                    Script.SetWinData(item.betAmount, item.winningAmount, item.gameRoundId);
                }

            }
            else
            {
                Debug.Log("Error: " + request.error);
            }

        }
    }
    [System.Serializable]
    public class HomeThunderBallDatum
    {
        public string gameRoundId { get; set; }
        public int betAmount { get; set; }
        public int winningAmount { get; set; }
    }

    [System.Serializable]
    public class HomeThunderBallResponse
    {
        public bool success { get; set; }
        public List<HomeThunderBallDatum> data { get; set; }
    }
    #endregion thunderball

    #region PowerBall
    private void LoadPowerBall()
    {
        PowerBallWinHistoryButtonClick();
    }

    public void PowerBallWinHistoryButtonClick()
    {
        ClearWins();
        StartCoroutine(PowerBallWinHistoryRequest());
    }

    IEnumerator PowerBallWinHistoryRequest()
    {
        Debug.Log("WinHistoryCalled");
        AuthTok = GetToken();
        Debug.Log(AuthTok);

        using (UnityWebRequest request = UnityWebRequest.Get(powerball_win_history_api_url))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", AuthTok);
            request.SetRequestHeader("userType", "User");
            yield return request.SendWebRequest();
            string response = request.downloadHandler.text;
            HomePowerBallWinResponse powerBallWinResponse = JsonConvert.DeserializeObject<HomePowerBallWinResponse>(response);
            if (request.result == UnityWebRequest.Result.Success)
            {
                foreach (var item in powerBallWinResponse.data)
                {
                    GameObject win = Instantiate(winPrefab, win_prefabParent);
                    var Script = win.transform.GetComponent<WinHistoryDisplayer>();

                    Script.SetWinData(item.betAmount, item.winningAmount, item.gameRoundId);
                }
            }
            else
            {
                Debug.Log("Error: " + request.error);
            }
        }


    }
    [System.Serializable]
    public class HomePowerBallDatum
    {
        public string gameRoundId { get; set; }
        public int betAmount { get; set; }
        public int winningAmount { get; set; }
    }
    [System.Serializable]
    public class HomePowerBallWinResponse
    {
        public bool success { get; set; }
        public List<HomePowerBallDatum> data { get; set; }
    }
    #endregion PowerBall

    #region PokerSlots
    private void LoadPokerSlots()
    {
        PokerSlotsWinHistoryButtonClick();
    }
    public void PokerSlotsWinHistoryButtonClick()
    {
        ClearWins();
        StartCoroutine(PokerSlotsWinHistoryRequest());
    }
    IEnumerator PokerSlotsWinHistoryRequest()
    {
        Debug.Log("WinHistoryCalled");
        AuthTok = GetToken();
        Debug.Log(AuthTok);

        using (UnityWebRequest request = UnityWebRequest.Get(poker_win_history_api_url))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", AuthTok);
            request.SetRequestHeader("userType", "User");
            yield return request.SendWebRequest();

            string response = request.downloadHandler.text;
            HomePokerSlotWinResponse winResponse = JsonConvert.DeserializeObject<HomePokerSlotWinResponse>(response);
            if (request.result == UnityWebRequest.Result.Success)
            {

                foreach (var item in winResponse.data)
                {

                    // Instantiate the win history object
                    GameObject win = Instantiate(winPrefab, win_prefabParent);
                    var Script = win.transform.GetComponent<WinHistoryDisplayer>();

                    // Set the values including the formatted IST date
                    Script.SetWinData(item.betAmount, item.winningAmount, item.gameRoundId);
                }

            }
            else
            {
                Debug.Log("Error: " + request.error);
            }

        }

    }

    [System.Serializable]
    public class HomePokerSlotDatum
    {
        public string gameRoundId { get; set; }
        public int betAmount { get; set; }
        public int winningAmount { get; set; }
    }
    [System.Serializable]
    public class HomePokerSlotWinResponse
    {
        public bool success { get; set; }
        public List<HomePokerSlotDatum> data { get; set; }
    }
    #endregion PokerSlots

    #region Lucky7

    private void LoadLuckySeven()
    {
        LuckySevenWinHistoryButtonClick();
    }

    public void LuckySevenWinHistoryButtonClick()
    {
        ClearWins();
        StartCoroutine(LuckySevenWinHistoryRequest());
    }

    IEnumerator LuckySevenWinHistoryRequest()
    {
        Debug.Log("WinHistoryCalled");
        string AuthTok = GetToken();
        Debug.Log(AuthTok);

        using (UnityWebRequest request = UnityWebRequest.Get(lucky7_win_history_api_url))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", AuthTok);
            request.SetRequestHeader("userType", "User");
            yield return request.SendWebRequest();

            string response = request.downloadHandler.text;
            HomeWinResponse winResponse = JsonConvert.DeserializeObject<HomeWinResponse>(response);
            if (request.result == UnityWebRequest.Result.Success)
            {

                foreach (var item in winResponse.data)
                {

                    // Instantiate the win history object
                    GameObject win = Instantiate(winPrefab, win_prefabParent);
                    var Script = win.transform.GetComponent<WinHistoryDisplayer>();

                    // Set the values including the formatted IST date
                    Script.SetWinData(item.betAmount, item.winningAmount, item.gameRoundId);
                }

            }
            else
            {
                Debug.Log("Error: " + request.error);
            }

        }

    }
    [System.Serializable]
    public class HomeDatum
    {
        public string gameRoundId { get; set; }
        public int betAmount { get; set; }
        public int winningAmount { get; set; }
    }
    [System.Serializable]
    public class HomeWinResponse
    {
        public bool success { get; set; }
        public List<HomeDatum> data { get; set; }
    }

    #endregion Lucky7

    #region Super Roulette
    private void LoadRoulette()
    {
        RouletteWinHistoryButtonClick();
    }

    public void RouletteWinHistoryButtonClick()
    {
        ClearWins();
        StartCoroutine(RouletteWinHistoryRequest());
    }

    IEnumerator RouletteWinHistoryRequest()
    {
        Debug.Log("WinHistoryCalled");
        AuthTok = GetToken();
        Debug.Log(AuthTok);

        using (UnityWebRequest request = UnityWebRequest.Get(roulette_win_history_api_url))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", AuthTok);
            request.SetRequestHeader("userType", "User");
            yield return request.SendWebRequest();
            string response = request.downloadHandler.text;
            HomeeSRWinResponse superRouletteWinResponse = JsonConvert.DeserializeObject<HomeeSRWinResponse>(response);
            if (request.result == UnityWebRequest.Result.Success)
            {
                foreach (var item in superRouletteWinResponse.data)
                {
                    GameObject win = Instantiate(winPrefab, win_prefabParent);
                    var Script = win.transform.GetComponent<WinHistoryDisplayer>();

                    Script.SetWinData(item.betAmount, item.winningAmount, item.gameRoundId);
                }
            }
            else
            {
                Debug.Log("Error: " + request.error);
            }
        }
    }

    [System.Serializable]
    public class HomeeSRBallDatum
    {
        public string gameRoundId { get; set; }
        public int betAmount { get; set; }
        public int winningAmount { get; set; }
    }
    [System.Serializable]
    public class HomeeSRWinResponse
    {
        public bool success { get; set; }
        public List<HomeeSRBallDatum> data { get; set; }
    }
    #endregion Super Roulette

    #region Jackpot 

    private void LoadJackpot()
    {
        JackpotWinHistoryButtonClick();
    }
    public void JackpotWinHistoryButtonClick()
    {
        ClearWins();
        StartCoroutine(JackpotWinHistoryRequest());
    }
    IEnumerator JackpotWinHistoryRequest()
    {
        Debug.Log("WinHistoryCalled");
        AuthTok = GetToken();
        Debug.Log(AuthTok);

        using (UnityWebRequest request = UnityWebRequest.Get(jackpot_win_history_api_url))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", AuthTok);
            request.SetRequestHeader("userType", "User");
            yield return request.SendWebRequest();
            string response = request.downloadHandler.text;
            HomeRoot luckyLottoWinResponse = JsonConvert.DeserializeObject<HomeRoot>(response);
            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Response: " + response);
                foreach (var item in luckyLottoWinResponse.data)
                {
                    foreach (var user in item.chosenUsers) // Iterate over the winningUsers list
                    {
                        GameObject win = Instantiate(winPrefab, win_prefabParent);
                        var Script = win.transform.GetComponent<WinHistoryDisplayer>();

                        // Use the `user.betAmount` and `user.winningAmount`
                        Script.SetWinData(user.betAmount, user.winningAmount, item.gameRoundId);
                    }
                }
            }
            else
            {
                Debug.Log("Error: " + request.error);
            }
        }
    }
    [System.Serializable]
    public class HomeLuckyDatum
    {
        public string gameRoundId { get; set; }
        public List<WinningUser> chosenUsers { get; set; }
    }
    [System.Serializable]
    public class HomeRoot
    {
        public List<HomeLuckyDatum> data { get; set; }
    }
    [System.Serializable]
    public class HomeWinningUser
    {
        public int betAmount { get; set; }
        public int winningAmount { get; set; }
    }

    #endregion Jackpot

    #region Jhandimunda

    private void LoadJhandimunda()
    {
        JhandiMundaWinHistoryButtonClick();
    }
    public void JhandiMundaWinHistoryButtonClick()
    {
        ClearWins();
        StartCoroutine(JhandiMundaWinHistoryRequest());
    }

    IEnumerator JhandiMundaWinHistoryRequest()
    {
        Debug.Log("WinHistoryCalled");
        AuthTok = GetToken();
        Debug.Log(AuthTok);

        using (UnityWebRequest request = UnityWebRequest.Get(jhandiMunda_win_history_api_url))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", AuthTok);
            request.SetRequestHeader("userType", "User");
            yield return request.SendWebRequest();

            string response = request.downloadHandler.text;
            HomeJMWinResponse winResponse = JsonConvert.DeserializeObject<HomeJMWinResponse>(response);
            if (request.result == UnityWebRequest.Result.Success)
            {

                foreach (var item in winResponse.data)
                {

                    // Instantiate the win history object
                    GameObject win = Instantiate(winPrefab, win_prefabParent);
                    var Script = win.transform.GetComponent<WinHistoryDisplayer>();

                    // Set the values including the formatted IST date
                    Script.SetWinData(item.betAmount, item.winningAmount, item.gameRoundId);
                }

            }
            else
            {
                Debug.Log("Error: " + request.error);
            }

        }

    }

    [System.Serializable]
    public class HomeJMHistoryData
    {
        public string gameRoundId { get; set; }
        public int betAmount { get; set; }
        public int winningAmount { get; set; }
    }
    [System.Serializable]
    public class HomeJMWinResponse
    {
        public bool success { get; set; }
        public List<HomeJMHistoryData> data { get; set; }
    }

    #endregion Jhandimunda

    #region Patti
    private void LoadPatti()
    {
        PattiWinHistoryButtonClick();
    }

    public void PattiWinHistoryButtonClick()
    {
        ClearWins();
        StartCoroutine(PattiWinHistoryRequest());
    }
    IEnumerator PattiWinHistoryRequest()
    {
        Debug.Log("WinHistoryCalled");
        AuthTok = GetToken();
        Debug.Log(AuthTok);

        using (UnityWebRequest request = UnityWebRequest.Get(patti_win_history_api_url))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", AuthTok);
            request.SetRequestHeader("userType", "User");
            yield return request.SendWebRequest();
            string response = request.downloadHandler.text;
            Debug.Log("WinHistoryCalled : " + response);

            HomePattiWinResponse PattiWinResponse = JsonConvert.DeserializeObject<HomePattiWinResponse>(response);
            if (request.result == UnityWebRequest.Result.Success)
            {
                foreach (var item in PattiWinResponse.data)
                {
                    GameObject win = Instantiate(winPrefab, win_prefabParent);
                    var Script = win.transform.GetComponent<WinHistoryDisplayer>();

                    Script.SetWinData(item.betAmount, item.winningAmount, item.gameRoundId);
                }
            }
            else
            {
                Debug.Log("Error: " + request.error);
            }
        }
    }


    [System.Serializable]
    public class HomePattiBallDatum
    {
        public string gameRoundId { get; set; }
        public int betAmount { get; set; }
        public int winningAmount { get; set; }
    }
    [System.Serializable]
    public class HomePattiWinResponse
    {
        public bool success { get; set; }
        public List<HomePattiBallDatum> data { get; set; }
    }
    #endregion Patti

    #region single
    private void LoadSingle()
    {
        SingleWinHistoryButtonClick();
    }

    public void SingleWinHistoryButtonClick()
    {
        ClearWins();
        StartCoroutine(SingleWinHistoryRequest());
    }

    IEnumerator SingleWinHistoryRequest()
    {
        Debug.Log("WinHistoryCalled");
        AuthTok = GetToken();
        Debug.Log(AuthTok);

        using (UnityWebRequest request = UnityWebRequest.Get(single_win_history_api_url))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", AuthTok);
            request.SetRequestHeader("userType", "User");
            yield return request.SendWebRequest();
            string response = request.downloadHandler.text;
            Debug.Log("WinHistoryCalled : " + response);

            HomeSingleWinResponse SingleWinResponse = JsonConvert.DeserializeObject<HomeSingleWinResponse>(response);
            if (request.result == UnityWebRequest.Result.Success)
            {
                foreach (var item in SingleWinResponse.data)
                {
                    GameObject win = Instantiate(winPrefab, win_prefabParent);
                    var Script = win.transform.GetComponent<WinHistoryDisplayer>();

                    Script.SetWinData(item.betAmount, item.winningAmount, item.gameRoundId);
                }
            }
            else
            {
                Debug.Log("Error: " + request.error);
            }
        }
    }

    [System.Serializable]
    public class HomeSingleBallDatum
    {
        public string gameRoundId { get; set; }
        public int betAmount { get; set; }
        public int winningAmount { get; set; }
    }
    [System.Serializable]
    public class HomeSingleWinResponse
    {
        public bool success { get; set; }
        public List<HomeSingleBallDatum> data { get; set; }
    }
    #endregion single
    #endregion winHistory

    #region betHistory

    void onClickBetHistoryButton()
    {
        Debug.Log("GameName" + game_name.text);
        string gameName = game_name.text;
        betHistory_Panel.SetActive(true);
        LoadBetHistory(gameName);
    }

    private void LoadBetHistory(string gameName)
    {
        switch (gameName)
        {
            case "Spin The Wheel":
                Debug.Log("SpinTheWheel Matched");
                LoadSpinTheWheelBetHistory();
                break;
            case "Thunder Ball":
                Debug.Log("ThunderBall Matched");
                LoadThunderBallBetHistory();
                break;
            case "Power Ball":
                Debug.Log("PowerBall Matched");
                LoadPowerBallBetHistory();
                break;
            case "PokerSlots":
                Debug.Log("PokerSlots Matched");
                LoadPokerSlotsBetHistory();
                break;
            case "Lucky7":
                Debug.Log("Lucky7 Matched");
                LoadLuckySevenBetHistory();
                break;
            case "Super Roulette":
                Debug.Log("Super Roulette Matched");
                LoadRouletteBetHistory();
                break;
            case "Jackpot":
                Debug.Log("Jackpot Matched");
                LoadJackpotBetHistory();
                break;
            case "Jhandimunda":
                Debug.Log("Jhandimunda Matched");
                LoadJhandimundaBetHistory();
                break;
            case "Patti":
                Debug.Log("Patti Matched");
                LoadPattiBetHistory();
                break;
            case "Single":
                Debug.Log("Single Matched");
                LoadSingleBetHistory();
                break;
            default:
                Debug.Log("Game doesn't exist.");
                break;

        }
    }

    #region Single
    private void LoadSingleBetHistory()
    {
        BetHistoryButtonClick();
    }
    public void BetHistoryButtonClick()
    {
        ClearBets();
        StartCoroutine(SingleBetHistoryRequest());
    }
    IEnumerator SingleBetHistoryRequest()
    {
        Debug.Log("BetHistoryCalled");
        AuthTok = GetToken();
        Debug.Log(AuthTok);

        using (UnityWebRequest request = UnityWebRequest.Get(single_bet_history_api_url))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", AuthTok);
            request.SetRequestHeader("userType", "User");
            yield return request.SendWebRequest();
            string response = request.downloadHandler.text;
            Debug.Log("bet Response::" + response);
            HomeSingleBetResponse betResponse = JsonConvert.DeserializeObject<HomeSingleBetResponse>(response);
            if (request.result == UnityWebRequest.Result.Success)
            {
                foreach (var item in betResponse.betHistory)
                {
                    GameObject bet = Instantiate(betPrefab, bet_prefabParent);
                    var Script = bet.transform.GetComponent<BetHistoryDisplayer>();
                    Script.SetBetData(item.betAmount, item.gameRoundIdgenerated, item.categoryName, item.betUnit = 0);
                }
            }
            else
            {
                Debug.LogError("Error in downloading: " + request.error);
            }
        }
    }

    [System.Serializable]
    public class HomeSingleBetDatum
    {
        public string gameRoundIdgenerated { get; set; }
        public string categoryName { get; set; }
        public int betAmount { get; set; }
        public int betUnit { get; set; }
    }
    [System.Serializable]
    public class HomeSingleBetResponse
    {
        public bool success { get; set; }
        public List<HomeSingleBetDatum> betHistory { get; set; }
    }

    #endregion Single

    #region Patti
    private void LoadPattiBetHistory()
    {
        PattiBetHistoryButtonClick();
    }
    public void PattiBetHistoryButtonClick()
    {
        ClearBets();
        StartCoroutine(PattiBetHistoryRequest());
    }

    IEnumerator PattiBetHistoryRequest()
    {
        Debug.Log("BetHistoryCalled");
        AuthTok = GetToken();
        Debug.Log(AuthTok);

        using (UnityWebRequest request = UnityWebRequest.Get(patti_bet_history_api_url))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", AuthTok);
            request.SetRequestHeader("userType", "User");
            yield return request.SendWebRequest();
            string response = request.downloadHandler.text;
            Debug.Log("bet Response::" + response);
            HomePattiBetResponse betResponse = JsonConvert.DeserializeObject<HomePattiBetResponse>(response);
            if (request.result == UnityWebRequest.Result.Success)
            {
                foreach (var item in betResponse.betHistory)
                {
                    GameObject bet = Instantiate(betPrefab, bet_prefabParent);
                    var Script = bet.transform.GetComponent<BetHistoryDisplayer>();
                    Script.SetBetData(item.betAmount, item.gameRoundIdgenerated, item.categoryName, item.betUnit = 0);
                }
            }
            else
            {
                Debug.LogError("Error in downloading: " + request.error);
            }
        }
    }

    [System.Serializable]
    public class HomePattiBetDatum
    {
        public string gameRoundIdgenerated { get; set; }
        public string categoryName { get; set; }
        public int betAmount { get; set; }
        public int betUnit { get; set; }
    }
    [System.Serializable]
    public class HomePattiBetResponse
    {
        public bool success { get; set; }
        public List<HomePattiBetDatum> betHistory { get; set; }
    }

    #endregion Patti

    #region JhandiMunda
    private void LoadJhandimundaBetHistory()
    {
        JMBetHistoryButtonClick();
    }
    public void JMBetHistoryButtonClick()
    {
        ClearBets();
        StartCoroutine(JMBetHistoryRequest());
    }


    IEnumerator JMBetHistoryRequest()
    {
        Debug.Log("BetHistoryCalled");
        AuthTok = GetToken();
        Debug.Log(AuthTok);

        using (UnityWebRequest request = UnityWebRequest.Get(jm_bet_history_api_url))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", AuthTok);
            request.SetRequestHeader("userType", "User");
            yield return request.SendWebRequest();

            string response = request.downloadHandler.text;
            Debug.Log(response);
            HomeJMBetResponse betResponse = JsonConvert.DeserializeObject<HomeJMBetResponse>(response);
            if (request.result == UnityWebRequest.Result.Success)
            {
                foreach (var item in betResponse.betHistory)
                {
                    GameObject win = Instantiate(betPrefab, bet_prefabParent);
                    var Script = win.transform.GetComponent<BetHistoryDisplayer>();

                    Script.SetBetData(item.betAmount, item.gameRoundIdgenerated, item.categoryName, item.betUnit);
                }
            }

        }

    }

    [System.Serializable]
    public class HomeJMBetHistoryData
    {
        public string gameRoundIdgenerated { get; set; }
        public string categoryName { get; set; }
        public int betAmount { get; set; }
        public int betUnit { get; set; }
    }
    [System.Serializable]
    public class HomeJMBetResponse
    {
        public bool success { get; set; }
        public List<HomeJMBetHistoryData> betHistory { get; set; }
    }
    #endregion JhandiMunda

    #region Jackpot
    private void LoadJackpotBetHistory()
    {
        JackpotBetHistoryButtonClick();
    }
    public void JackpotBetHistoryButtonClick()
    {
        ClearBets();
        StartCoroutine(JackpotBetHistoryRequest());
    }
    IEnumerator JackpotBetHistoryRequest()
    {
        Debug.Log("BetHistoryCalled");
        AuthTok = GetToken();
        Debug.Log(AuthTok);

        using (UnityWebRequest request = UnityWebRequest.Get(jackpot_bet_history_api_url))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", AuthTok);
            request.SetRequestHeader("userType", "User");
            yield return request.SendWebRequest();
            string response = request.downloadHandler.text;
            Debug.Log("bet Response::" + response);
            HomeLuckyLotoBetResponse betResponse = JsonConvert.DeserializeObject<HomeLuckyLotoBetResponse>(response);
            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("bet Response::" + betResponse);
                foreach (var item in betResponse.betHistory)
                {
                    GameObject bet = Instantiate(betPrefab, bet_prefabParent);
                    var Script = bet.transform.GetComponent<BetHistoryDisplayer>();

                    // Use the `user.betAmount` and `user.winningAmount`
                    Script.SetBetData(item.betAmount, item.gameRoundIdgenerated, item.categoryName, item.betUnit);

                }
            }
            else
            {
                Debug.LogError("Error in downloading: " + request.error);
            }
        }
    }

    [System.Serializable]
    public class HomeLLBetHistoryData
    {
        public string gameRoundIdgenerated { get; set; }
        public string categoryName { get; set; }
        public int betAmount { get; set; }
        public int betUnit { get; set; }
    }

    [System.Serializable]
    public class HomeLuckyLotoBetResponse
    {
        public List<HomeLLBetHistoryData> betHistory { get; set; }
    }
    #endregion Jackpot

    #region Roulette
    private void LoadRouletteBetHistory()
    {
        RouletteBetHistoryButtonClick();
    }

    public void RouletteBetHistoryButtonClick()
    {
        ClearBets();
        StartCoroutine(RouletteBetHistoryRequest());
    }

    IEnumerator RouletteBetHistoryRequest()
    {
        Debug.Log("BetHistoryCalled");
        AuthTok = GetToken();
        Debug.Log(AuthTok);

        using (UnityWebRequest request = UnityWebRequest.Get(roulette_bet_history_api_url))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", AuthTok);
            request.SetRequestHeader("userType", "User");
            yield return request.SendWebRequest();
            string response = request.downloadHandler.text;
            Debug.Log("bet Response::" + response);
            HomeSPBetResponse betResponse = JsonConvert.DeserializeObject<HomeSPBetResponse>(response);
            if (request.result == UnityWebRequest.Result.Success)
            {
                foreach (var item in betResponse.betHistory)
                {
                    GameObject bet = Instantiate(betPrefab, bet_prefabParent);
                    var Script = bet.transform.GetComponent<BetHistoryDisplayer>();
                    Script.SetBetData(item.betAmount, item.gameRoundIdgenerated, item.categoryName, 0);
                }
            }
            else
            {
                Debug.LogError("Error in downloading: " + request.error);
            }
        }
    }
    [System.Serializable]
    public class HomeSPBetDatum
    {
        public string gameRoundIdgenerated { get; set; }
        public string categoryName { get; set; }
        public int betAmount { get; set; }
        public int betUnit { get; set; }
    }
    [System.Serializable]
    public class HomeSPBetResponse
    {
        public bool success { get; set; }
        public List<HomeSPBetDatum> betHistory { get; set; }
    }

    #endregion Roulette

    #region Lucky7
    private void LoadLuckySevenBetHistory()
    {
        Lucky7BetHistoryButtonClick();
    }
    public void Lucky7BetHistoryButtonClick()
    {
        ClearBets();
        StartCoroutine(Lucky7BetHistoryRequest());
    }

    IEnumerator Lucky7BetHistoryRequest()
    {
        Debug.Log("BetHistoryCalled1");
        AuthTok = GetToken();
        Debug.Log(AuthTok);

        using (UnityWebRequest request = UnityWebRequest.Get(lucky7_bet_history_api_url))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", AuthTok);
            request.SetRequestHeader("userType", "User");
            yield return request.SendWebRequest();

            string response = request.downloadHandler.text;
            Debug.Log(response);
            HomeLuckySevenBetResponse betResponse = JsonConvert.DeserializeObject<HomeLuckySevenBetResponse>(response);
            if (request.result == UnityWebRequest.Result.Success)
            {
                foreach (var item in betResponse.data)
                {
                    // Instantiate the win history object
                    GameObject win = Instantiate(betPrefab, bet_prefabParent);
                    var Script = win.transform.GetComponent<BetHistoryDisplayer>();

                    // Set the values including the formatted IST date
                    Script.SetBetData(item.betAmount, item.gameRoundIdgenerated, item.categoryName, item.betUnit);
                }
            }

        }

    }

    [System.Serializable]
    public class HomeLuckySevenBetDatum
    {
        public string gameRoundIdgenerated { get; set; }
        public string categoryName { get; set; }
        public int betAmount { get; set; }
        public int betUnit { get; set; }
    }
    [System.Serializable]
    public class HomeLuckySevenBetResponse
    {
        public bool success { get; set; }
        public List<HomeLuckySevenBetDatum> data { get; set; }
    }
    #endregion Lucky7

    #region PokerSlots
    private void LoadPokerSlotsBetHistory()
    {
        PokerSlotsBetHistoryButtonClick();
    }

    public void PokerSlotsBetHistoryButtonClick()
    {
        ClearBets();
        StartCoroutine(BetHistoryRequest());
    }


    IEnumerator BetHistoryRequest()
    {
        Debug.Log("BetHistoryCalled");
        AuthTok = GetToken();
        Debug.Log(AuthTok);

        using (UnityWebRequest request = UnityWebRequest.Get(pokerslots_bet_history_api_url))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", AuthTok);
            request.SetRequestHeader("userType", "User");
            yield return request.SendWebRequest();

            string response = request.downloadHandler.text;
            Debug.Log(response);
            HomePokerSlotBetResponse betResponse = JsonConvert.DeserializeObject<HomePokerSlotBetResponse>(response);
            if (request.result == UnityWebRequest.Result.Success)
            {
                foreach (var item in betResponse.betHistory)
                {
                    // Instantiate the win history object
                    GameObject win = Instantiate(betPrefab, bet_prefabParent);
                    var Script = win.transform.GetComponent<BetHistoryDisplayer>();

                    // Set the values including the formatted IST date
                    Script.SetBetData(item.betAmount, item.gameRoundIdgenerated, item.colorRoomCombination, item.betUnit = 0);
                }
            }

        }

    }

    [System.Serializable]
    public class HomePokerSlotBetDatum
    {
        public string gameRoundIdgenerated { get; set; }
        public string colorRoomCombination { get; set; }
        public int betAmount { get; set; }
        public int betUnit { get; set; }
    }
    [System.Serializable]
    public class HomePokerSlotBetResponse
    {
        public bool success { get; set; }
        public List<HomePokerSlotBetDatum> betHistory { get; set; }
    }
    #endregion PokerSlots

    #region PowerBall
    private void LoadPowerBallBetHistory()
    {
        PowerBallBetHistoryButtonClick();
    }

    public void PowerBallBetHistoryButtonClick()
    {
        ClearBets();
        StartCoroutine(PowerBallBetHistoryRequest());
    }
    IEnumerator PowerBallBetHistoryRequest()
    {
        Debug.Log("BetHistoryCalled");
        AuthTok = GetToken();
        Debug.Log(AuthTok);

        using (UnityWebRequest request = UnityWebRequest.Get(powerball_bet_history_api_url))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", AuthTok);
            request.SetRequestHeader("userType", "User");
            yield return request.SendWebRequest();
            string response = request.downloadHandler.text;
            Debug.Log("bet Response::" + response);
            HomePowerBallBetResponse betResponse = JsonConvert.DeserializeObject<HomePowerBallBetResponse>(response);
            if (request.result == UnityWebRequest.Result.Success)
            {
                foreach (var item in betResponse.betHistory)
                {
                    GameObject bet = Instantiate(betPrefab, bet_prefabParent);
                    var Script = bet.transform.GetComponent<BetHistoryDisplayer>();
                    Script.SetBetData(item.betAmount, item.gameRoundIdgenerated, item.categoryName, item.betUnit);
                }
            }
            else
            {
                Debug.LogError("Error in downloading: " + request.error);
            }
        }
    }

    [System.Serializable]
    public class HomePowerBallBetDatum
    {
        public string gameRoundIdgenerated { get; set; }
        public string categoryName { get; set; }
        public int betAmount { get; set; }
        public int betUnit { get; set; }
    }
    [System.Serializable]
    public class HomePowerBallBetResponse
    {
        public bool success { get; set; }
        public List<HomePowerBallBetDatum> betHistory { get; set; }
    }


    #endregion PowerBall

    #region ThunderBall
    private void LoadThunderBallBetHistory()
    {
        ThunderBallBetHistoryButtonClick();
    }
    public void ThunderBallBetHistoryButtonClick()
    {
        ClearBets();
        StartCoroutine(ThunderBallBetHistoryRequest());
    }



    IEnumerator ThunderBallBetHistoryRequest()
    {
        Debug.Log("BetHistoryCalled");
        AuthTok = GetToken();

        using (UnityWebRequest request = UnityWebRequest.Get(thunderball_bet_history_api_url))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", AuthTok);
            request.SetRequestHeader("userType", "User");
            yield return request.SendWebRequest();
            string response = request.downloadHandler.text;
            Debug.Log("BetHistoryCalled response: " + response);
            HomeThunderBallBetResponse betResponse = JsonConvert.DeserializeObject<HomeThunderBallBetResponse>(response);
            if (request.result == UnityWebRequest.Result.Success)
            {
                foreach (var item in betResponse.betHistory)
                {
                    // Instantiate the win history object
                    GameObject win = Instantiate(betPrefab, bet_prefabParent);
                    var Script = win.transform.GetComponent<BetHistoryDisplayer>();

                    // Set the values including the formatted IST date
                    Script.SetBetData(item.betAmount, item.gameRoundIdgenerated, item.categoryName, item.betUnit);
                }
            }
        }
    }
    [System.Serializable]
    public class HomeThunderBallBetDatum
    {
        public string gameRoundIdgenerated { get; set; }
        public string categoryName { get; set; }
        public int betAmount { get; set; }
        public int betUnit { get; set; }
    }
    [System.Serializable]
    public class HomeThunderBallBetResponse
    {
        public bool success { get; set; }
        public List<HomeThunderBallBetDatum> betHistory { get; set; }
    }
    #endregion ThunderBall

    #region SpinTheWheel

    private void LoadSpinTheWheelBetHistory()
    {
        SpinTheWheelBetHistoryButtonClick();
    }
    public void SpinTheWheelBetHistoryButtonClick()
    {
        ClearWins();
        StartCoroutine(SpinTheWheelBetHistoryRequest());
    }
    IEnumerator SpinTheWheelBetHistoryRequest()
    {
        Debug.Log("BetHistoryCalled");
        AuthTok = GetToken();
        Debug.Log(AuthTok);

        using (UnityWebRequest request = UnityWebRequest.Get(spinThewheel_bet_history_api_url))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", AuthTok);
            request.SetRequestHeader("userType", "User");
            yield return request.SendWebRequest();
            string response = request.downloadHandler.text;
            Debug.Log("bet Response::" + response);
            HomeSTWBetResponse betResponse = JsonConvert.DeserializeObject<HomeSTWBetResponse>(response);
            if (request.result == UnityWebRequest.Result.Success)
            {
                foreach (var item in betResponse.betHistory)
                {
                    GameObject bet = Instantiate(betPrefab, bet_prefabParent);
                    var Script = bet.transform.GetComponent<BetHistoryDisplayer>();
                    Script.SetBetData(item.betAmount, item.gameRoundIdgenerated, item.categoryName, item.betUnit);
                }
            }
            else
            {
                Debug.LogError("Error in downloading: " + request.error);
            }
        }
    }

    [System.Serializable]
    public class HomeSTWBetDatum
    {
        public string gameRoundIdgenerated { get; set; }
        public string categoryName { get; set; }
        public int betAmount { get; set; }
        public int betUnit { get; set; }

    }
    [System.Serializable]
    public class HomeSTWBetResponse
    {
        public bool success { get; set; }
        public List<HomeSTWBetDatum> betHistory { get; set; }
    }
    #endregion SpinTheWheel

    #endregion betHistory


    #region Normal Methods
    void ChangeGameName(string name)
    {
        game_name.text = name;
    }
    void onClickSpinTheWheelButton()
    {
        choose_Panel.SetActive(true);
        ChangeGameName("Spin The Wheel");
    }


    void onClickThunderBallButton()
    {
        choose_Panel.SetActive(true);
        ChangeGameName("Thunder Ball");
    }

    void onClickPowerBallButton()
    {
        choose_Panel.SetActive(true);
        ChangeGameName("Power Ball");

    }

    void onClickPokerSlotsButton()
    {
        choose_Panel.SetActive(true);
        ChangeGameName("PokerSlots");
    }

    void onClickLuckySevenButton()
    {
        choose_Panel.SetActive(true);
        ChangeGameName("Lucky7");

    }

    void onClickRouletteButton()
    {
        choose_Panel.SetActive(true);
        ChangeGameName("Super Roulette");

    }

    void onClickJackpotButton()
    {
        choose_Panel.SetActive(true);
        ChangeGameName("Jackpot");

    }

    void onClickJhandimundaButton()
    {
        choose_Panel.SetActive(true);
        ChangeGameName("Jhandimunda");

    }

    void onClickFataFatButton()
    {
        choose_patti_or_single_panel.SetActive(true);
    }

    void onClickPattiButton()
    {
        choose_Panel.SetActive(true);
        ChangeGameName("Patti");

    }

    void onClickSingleButton()
    {
        choose_Panel.SetActive(true);
        ChangeGameName("Single");
    }


    void ActiveGameHistoryPanel()
    {
        gameHistory_Panel.SetActive(true);
    }
    #endregion

    #region backButtons
    void GameHistoryPanel_BackButton()
    {
        gameHistory_Panel.SetActive(false);
    }

    void FataFatPanel_BackButton()
    {
        choose_patti_or_single_panel.SetActive(false);
    }

    void ChoosePanel_BackButton()
    {
        choose_Panel.SetActive(false);
    }

    void WinHistory_BackButton()
    {
        winHistory_Panel.SetActive(false);
    }

    void BetHistory_BackButton()
    {
        betHistory_Panel.SetActive(false);
    }
    #endregion backbuttons
}
