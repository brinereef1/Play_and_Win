using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class LoginViewModel : MonoBehaviour
{
    HomeUIManager _uiManager;
    string AuthToken;

    #region Lucky7
    Lucky7WalletManager lucky7WalletManager = new Lucky7WalletManager();
    WinHistoryManager winHistoryManager = new WinHistoryManager();
    BetHistoryManager betHistoryManager = new BetHistoryManager();
    BetManager betManager = new BetManager();
    LastTenHistoryManager lastHistoryManager = new LastTenHistoryManager();

    #endregion Lucky7

    #region PokerSlots
    PokerSlotsWalletManager pokerSlotsWalletManager = new PokerSlotsWalletManager();
    SpinnerWheel sWheel = new SpinnerWheel();
    PokerSlotBetManager pokerSlotBetManager = new PokerSlotBetManager();
    PokerSlotIsWinnerManager slotIsWinnerManagerManager = new PokerSlotIsWinnerManager();
    PokerSlotWinHistoryManager slotWinHistoryManager = new PokerSlotWinHistoryManager();
    PokerSlotBetHistoryManager pokerSlotBetHistoryManager = new PokerSlotBetHistoryManager();
    PokerSlotLastTenWinnersManager pokerSlotLastTenWinnersManager = new PokerSlotLastTenWinnersManager();

    #endregion PokerSlots

    #region ThunderBall
    ThunderBallWalletManager thunderBallWalletManager = new ThunderBallWalletManager();
    ThunderBallTimer thunderBallTimer = new ThunderBallTimer();
    ThunderBallWinHistoryManager thunderBallWinHistoryManager = new ThunderBallWinHistoryManager();
    ThunderBallBetHistoryManager thunderBallBetHistoryManager = new ThunderBallBetHistoryManager();

    ThunderBallLastTenWinHistoryManager thunderBallLastTenWinHistoryManager = new ThunderBallLastTenWinHistoryManager();
    ThunderBallIsWinnerManager thunderBallIsWinnerManager = new ThunderBallIsWinnerManager();
    ThunderBallSelectedBall thunderBallSelectedBall = new ThunderBallSelectedBall();
    #endregion ThunderBall

    #region PowerBall
    PowerBallWalletManager powerBallWalletManager = new PowerBallWalletManager();
    PowerBallBetManager powerBallBetManager = new PowerBallBetManager();
    PowerBallWinHistoryManager powerBallWinHistoryManager = new PowerBallWinHistoryManager();
    PowerBallBetHistoryManager powerBallBetHistoryManager = new PowerBallBetHistoryManager();
    PowerBallIsWinnerManager powerBallIsWinnerManager = new PowerBallIsWinnerManager();
    PowerBallSelectedBall powerBallSelectedBall = new PowerBallSelectedBall();
    PowerBallLastTenWinManager powerBallLastTenWinManager = new PowerBallLastTenWinManager();
    #endregion PowerBall

    #region SuperRoulette
    RouletteWalletManager rouletteWalletManager = new RouletteWalletManager();
    SuperRouletteBetHistoryManager superRouletteBetHistoryManager = new SuperRouletteBetHistoryManager();
    #endregion SuperRoulette

    WalletManager walletManager;
    GameHistoryManager gameHistoryManager;
    #region FataFat
    FATAFATWalletManager fATAFATWalletManager = new FATAFATWalletManager();
    PattiBetManager pattiBetManager = new PattiBetManager(); // for set token
    SingleBetManager singleBetManager = new SingleBetManager(); // for set token


    // history
    PattiBetHistoryManager pattiBetHistoryManager = new PattiBetHistoryManager();
    SingleBetHistoryManager singleBetHistoryManager = new SingleBetHistoryManager();

    PattiWinHistoryManager pattiWinHistoryManager = new PattiWinHistoryManager();
    SingleWinHistoryManager singleWinHistoryManager = new SingleWinHistoryManager();
    //history
    PattiResultManager pattiResultManager = new PattiResultManager();
    SingleResultManager singleResultManager = new SingleResultManager();

    PattiIsWinnerManager pattiIsWinnerManager = new PattiIsWinnerManager();
    SingleIsWinnerManager singleIsWinnerManager = new SingleIsWinnerManager();

    PattiLastTenWinHistoryManager pattiLastTenWinHistoryManager = new PattiLastTenWinHistoryManager();
    SingleLastTenWinHistoryManager singleLastTenWinHistoryManager = new SingleLastTenWinHistoryManager();

    FATAFATWalletManager fwalletManager = new FATAFATWalletManager();


    #endregion FataFat

    #region JhandiMunda
    JMWalletManager jMWalletManager = new JMWalletManager();
    JMBetHistoryManager jMBetHistoryManager = new JMBetHistoryManager();
    JMWinHistoryManager jMWinHistoryManager = new JMWinHistoryManager();
    JMBetManager jMBetManager = new JMBetManager();
    JMIsWinnerManager jMIsWinnerManager = new JMIsWinnerManager();
    JMLastTenHistoryManager jMLastTenHistoryManager = new JMLastTenHistoryManager();

    JMResultManager jMResultManager = new JMResultManager();


    #endregion JhandiMunda

    #region LuckyLotto
    LuckyLottoLastTenWinHistoryManager luckyLottoLastTenWinHistoryManager = new LuckyLottoLastTenWinHistoryManager();
    LuckyLottoWinHistoryManager luckyLottoWinHistoryManager = new LuckyLottoWinHistoryManager();
    LuckyLottoBetHistoryManager luckyLottoBetHistoryManager = new LuckyLottoBetHistoryManager();
    LuckyLottoResultManger luckyLottoResultManger = new LuckyLottoResultManger();
    LuckyLottoBetManager luckyLottoBetManager = new LuckyLottoBetManager();
    LuckLottoIsWinnerManager winnerLottoIsWinner = new LuckLottoIsWinnerManager();
    LuckyLottoWalletManager luckyLottoWalletManager = new LuckyLottoWalletManager();
    #endregion LuckyLotto

    #region Koyel
    KoyelTimer koyelTimer = new KoyelTimer();
    KoyelUIManager KoyelUIManager = new KoyelUIManager();
    KoyelWalletManager koyelWalletManager = new KoyelWalletManager();
    KoyelBetManager koyelBetManager = new KoyelBetManager();
    KoyelBetHistoryManager koyelBetHistoryManager = new KoyelBetHistoryManager();
    #endregion Koyel

    #region SpinTheWheel
    SpinTheWheelWalletManager stwWalletManager = new SpinTheWheelWalletManager();
    SpinTheWheelBetHistoryManager stwBetHistoryManager = new SpinTheWheelBetHistoryManager();
    SpinTheWheelWinHistoryManager stwWinHistoryManager = new SpinTheWheelWinHistoryManager();
    SpinTheWheelBetManager stwBetManager = new SpinTheWheelBetManager();
    SpinTheWheelIsWinnerManager stwIsWinnerManager = new SpinTheWheelIsWinnerManager();
    SpinTheWheelLastTenWinHistoryManager stwLastTenWinHistoryManager = new SpinTheWheelLastTenWinHistoryManager();
    SpinTheWheelResultManager stwResultManager = new SpinTheWheelResultManager();

    #endregion SpinTheWheel
    void Start()
    {
        _uiManager = FindFirstObjectByType<HomeUIManager>();
        walletManager = FindFirstObjectByType<WalletManager>();
        gameHistoryManager = FindFirstObjectByType<GameHistoryManager>();
    }
    public IEnumerator Login(UserModel userModel, System.Action<string> callback)
    {
        string url = "http://13.234.117.221:2556/api/v1/user/login";

        UserLoginModel userLoginModel = new UserLoginModel();

        userLoginModel.email = userModel.email;
        userLoginModel.password = userModel.password;



        string jsonData = JsonConvert.SerializeObject(userLoginModel);

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
                if (_uiManager != null)
                {
                    _uiManager.ShowAnyResponse("User Login Failed");

                }
                callback?.Invoke(request.error);
            }
            else
            {
                Debug.Log("Login Successful");
                Debug.Log(request.downloadHandler.text);
                if (_uiManager != null)
                {
                    _uiManager.ShowAnyResponse("User Login Successfully");

                }
                callback?.Invoke(request.downloadHandler.text);

                string response = request.downloadHandler.text;
                JObject json = JObject.Parse(response);
                string token = json["data"]["token"].ToString();
                string name = json["data"]["name"].ToString();
                string email = json["data"]["email"].ToString();
                UpdateAuthToken(token);
                SaveUserData saveUserData = new SaveUserData();
                saveUserData.SaveLoginData(name, email, token);
                walletManager.GetWalletBalance();

                if (_uiManager != null)
                {

                    _uiManager.LoginAndRegisterPanel.SetActive(false);
                    _uiManager.Home();
                    _uiManager.SideBarPanel.SetActive(true);

                    _uiManager.name_text.text = name;
                    _uiManager.email_text.text = email;
                }

            }
        }
    }

    public void UpdateAuthToken(string newToken)
    {
        if (newToken != "")
        {
            AuthToken = newToken;
        }
        #region LuckyLotto
        if (luckyLottoBetHistoryManager != null) { luckyLottoBetHistoryManager.SetToken(AuthToken); }
        if (luckyLottoWinHistoryManager != null) { luckyLottoWinHistoryManager.SetToken(AuthToken); }
        if (luckyLottoLastTenWinHistoryManager != null) { luckyLottoLastTenWinHistoryManager.SetToken(AuthToken); }
        if (luckyLottoResultManger != null) { luckyLottoResultManger.SetToken(AuthToken); }
        if (luckyLottoBetManager != null) { luckyLottoBetManager.SetToken(AuthToken); }
        if (winnerLottoIsWinner != null) { winnerLottoIsWinner.SetToken(AuthToken); }
        if (luckyLottoWalletManager != null) { luckyLottoWalletManager.SetToken(AuthToken); }
        #endregion LuckyLotto

        #region JhandiMunda
        if (jMBetHistoryManager != null)
        {
            jMBetHistoryManager.SetToken(AuthToken);
        }

        if (jMWinHistoryManager != null)
        {
            jMWinHistoryManager.SetToken(AuthToken);
        }

        if (jMBetManager != null)
        {
            jMBetManager.SetToken(AuthToken);
        }

        if (jMIsWinnerManager != null)
        {
            jMIsWinnerManager.SetToken(AuthToken);
        }

        if (jMLastTenHistoryManager != null)
        {
            jMLastTenHistoryManager.SetToken(AuthToken);
        }

        if (jMResultManager != null)
        {
            jMResultManager.SetToken(AuthToken);
        }

        if (jMWalletManager != null)
        {
            jMWalletManager.SetToken(AuthToken);
        }
        #endregion JhandiMunda

        #region LuckySeven
        if (betManager != null)
        {
            betManager.SetToken(AuthToken);
        }

        if (winHistoryManager != null)
        {
            winHistoryManager.SetToken(AuthToken);
        }

        if (betHistoryManager != null)
        {
            betHistoryManager.SetToken(AuthToken);
        }

        if (lastHistoryManager != null)
        {
            lastHistoryManager.SetToken(AuthToken);
        }
        if (walletManager != null)
        {
            walletManager.SetToken(AuthToken);
        }
        #endregion LuckySeven

        #region PokerSlots
        if (pokerSlotsWalletManager != null)
        {
            pokerSlotsWalletManager.SetToken(AuthToken);
        }
        if (sWheel != null)
        {
            sWheel.SetToken(AuthToken);
        }

        if (pokerSlotBetManager != null)
        {
            pokerSlotBetManager.SetToken(AuthToken);
        }

        if (slotIsWinnerManagerManager != null)
        {
            slotIsWinnerManagerManager.SetToken(AuthToken);
        }

        if (slotWinHistoryManager != null)
        {
            slotWinHistoryManager.SetToken(AuthToken);
        }

        if (pokerSlotBetHistoryManager != null)
        {
            pokerSlotBetHistoryManager.SetToken(AuthToken);
        }
        if (pokerSlotLastTenWinnersManager != null)
        {
            pokerSlotLastTenWinnersManager.SetToken(AuthToken);
        }
        #endregion PokerSlots

        #region ThunderBall
        if (thunderBallWalletManager != null)
        {
            thunderBallWalletManager.SetToken(AuthToken);
        }
        if (thunderBallTimer != null)
        {
            thunderBallTimer.SetToken(AuthToken);
        }
        if (thunderBallWinHistoryManager != null)
        {
            thunderBallWinHistoryManager.SetToken(AuthToken);
        }
        if (thunderBallBetHistoryManager != null)
        {
            thunderBallBetHistoryManager.SetToken(AuthToken);
        }

        if (thunderBallIsWinnerManager != null)
        {
            thunderBallIsWinnerManager.SetToken(AuthToken);
        }

        if (thunderBallSelectedBall != null)
        {
            thunderBallSelectedBall.SetToken(AuthToken);
        }
        if (thunderBallLastTenWinHistoryManager != null)
        {
            thunderBallLastTenWinHistoryManager.SetToken(AuthToken);
        }
        #endregion ThunderBall

        #region PowerBall
        if (powerBallWalletManager != null)
        {
            powerBallWalletManager.SetToken(AuthToken);
        }
        if (powerBallBetManager != null)
        {
            powerBallBetManager.SetToken(AuthToken);
        }
        if (powerBallWinHistoryManager != null)
        {
            powerBallWinHistoryManager.SetToken(AuthToken);
        }
        if (powerBallBetHistoryManager != null)
        {
            powerBallBetHistoryManager.SetToken(AuthToken);
        }

        if (powerBallIsWinnerManager != null)
        {
            powerBallWinHistoryManager.SetToken(AuthToken);
        }
        if (powerBallSelectedBall != null)
        {
            powerBallSelectedBall.SetToken(AuthToken);
        }
        if (powerBallLastTenWinManager != null)
        {
            powerBallLastTenWinManager.SetToken(AuthToken);
        }

        #endregion  PowerBall

        #region Roulette
        if (rouletteWalletManager != null)
        {
            rouletteWalletManager.SetToken(AuthToken);
        }
        if (superRouletteBetHistoryManager != null)
        {
            superRouletteBetHistoryManager.SetToken(AuthToken);
        }

        #endregion Roulette

        #region FataFat
        if (pattiWinHistoryManager != null)
        {
            pattiWinHistoryManager.SetToken(AuthToken);
        }

        if (singleWinHistoryManager != null)
        {
            singleWinHistoryManager.SetToken(AuthToken);
        }
        if (fATAFATWalletManager != null)
        {
            fATAFATWalletManager.SetToken(AuthToken);
        }
        if (pattiResultManager != null)
        {
            pattiResultManager.SetToken(AuthToken);
        }

        if (singleResultManager != null)
        {
            singleResultManager.SetToken(AuthToken);
        }

        if (pattiIsWinnerManager != null)
        {
            pattiIsWinnerManager.SetToken(AuthToken);
        }

        if (singleIsWinnerManager != null)
        {
            singleIsWinnerManager.SetToken(AuthToken);
        }

        if (pattiBetManager != null)
        {
            pattiBetManager.SetToken(AuthToken);
        }

        if (singleBetManager != null)
        {
            singleBetManager.SetToken(AuthToken);
        }

        if (pattiLastTenWinHistoryManager != null)
        {
            pattiLastTenWinHistoryManager.SetToken(AuthToken);
        }

        if (singleLastTenWinHistoryManager != null)
        {
            singleLastTenWinHistoryManager.SetToken(AuthToken);
        }
        #endregion FataFat

        #region Wallet
        if (walletManager != null)
        {
            walletManager.SetToken(AuthToken);
        }
        #endregion Wallet

        #region SpinTheWheel
        if (stwWalletManager != null) { stwWalletManager.SetToken(AuthToken); }
        if (stwBetHistoryManager != null) { stwBetHistoryManager.SetToken(AuthToken); }
        if (stwWinHistoryManager != null) { stwWinHistoryManager.SetToken(AuthToken); }
        if (stwBetManager != null) { stwBetManager.SetToken(AuthToken); }
        if (stwIsWinnerManager != null) { stwIsWinnerManager.SetToken(AuthToken); }
        if (stwLastTenWinHistoryManager != null) { stwLastTenWinHistoryManager.SetToken(AuthToken); }
        if (stwResultManager != null) { stwResultManager.SetToken(AuthToken); }
        #endregion SpinTheWheel

        #region Koyel
        if (koyelTimer != null)
        {
            koyelTimer.SetToken(AuthToken);
        }

        if (KoyelUIManager != null)
        {
            KoyelUIManager.SetToken(AuthToken);
        }

        if (koyelBetManager != null)
        {
            koyelBetManager.SetToken(AuthToken);
        }

        if(koyelWalletManager != null)
        {
            koyelWalletManager.SetToken(AuthToken);
        }

        if(koyelBetHistoryManager != null)
        {
            koyelBetHistoryManager.SetToken(AuthToken);
        }

        #endregion

        if (gameHistoryManager != null) { gameHistoryManager.SetToken(AuthToken); }
    }

}
[System.Serializable]
public class UserLoginModel
{
    public string name { get; set; }
    public string email { get; set; }
    public string password { get; set; }
}

[System.Serializable]
public class LoginData
{
    public string token { get; set; }
}

[System.Serializable]
public class LoginRoot
{
    public bool status { get; set; }
    public string message { get; set; }
    public LoginData data { get; set; }
}