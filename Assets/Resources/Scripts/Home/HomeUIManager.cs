using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class HomeUIManager : MonoBehaviour
{


    [Header("Login and Register")]
    [SerializeField] public GameObject LoginPanel;
    [SerializeField] public GameObject RegisterPanel;

    [Header("Panels")]
    [SerializeField] GameObject HomePanel;
    [SerializeField] public GameObject LoginAndRegisterPanel;
    [SerializeField] public GameObject WalletPanel;

    [Header("Response")]
    [SerializeField] public TMP_Text responseText;

    [Header("SideBarPanel")]
    [SerializeField] public GameObject SideBarPanel;

    [Header("UserProfile")]
    public TMP_Text name_text;
    public TMP_Text email_text;

    void Start()
    {

    }

    public void ShowAnyResponse(string responseText)
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

    public void ShowLoginPanelButtonClick()
    {
        Debug.Log("LoginPanel...");
        RegisterPanel.SetActive(false);
        LoginPanel.SetActive(true);

    }

    public void ShowRegisterPanelButtonClick()
    {
        Debug.Log("RegisterPanel...");
        LoginPanel.SetActive(false);
        RegisterPanel.SetActive(true);
    }

    #region GamesPanel


    public void JhandiMundaPlayButton()
    {
        SceneManager.LoadScene("Jhandi Munda");
    }

    public void LuckyLottoPlayButton(){
        SceneManager.LoadScene("LuckyLotto");
    }
    public void SpinTheWheelPlayButton()
    {
        Debug.Log("SpinTheWheel Starting...");
        SceneManager.LoadScene("SpinTheWheel");
    }

    public void Lucky7PlayButton()
    {
        Debug.Log("Lucky7 Starting...");
        SceneManager.LoadScene("Lucky 7");
    }

    public void KoyelPlayButton()
    {
        Debug.Log("Koyel Starting...");
        SceneManager.LoadScene("Koyel");
    }

    public void ThunderBallPlayButton()
    {
        Debug.Log("ThunderBall Starting...");
        SceneManager.LoadScene("Thunder Ball");
        // Implement your ThunderBall game logic here
    }

    public void PokerSlotsPlayButton()
    {
        Debug.Log("PokerSlots Starting...");
        SceneManager.LoadScene("Poker Slots");

        // Implement your PokerSlots game logic here
    }

    public void PowerBallPlayButton()
    {
        Debug.Log("PowerBall Starting...");
        SceneManager.LoadScene("Power Ball");
    }

    public void SuperRoulettePlayButton()
    {
        Debug.Log("SuperRoulette Starting...");
        SceneManager.LoadScene("Super Roulette");
        // Implement your SuperRoulette game logic here
    }

    public void FataFatPlayButton()
    {
        Debug.Log("FataFata Starting...");
        SceneManager.LoadScene("FataFat");
        // Implement your FataFata game logic here
    }

    #endregion GamesPanel
    public void Home()
    {
        HomePanel.SetActive(true);
        WalletPanel.SetActive(false);
    }

    public void ShowWallet()
    {
        WalletPanel.gameObject.SetActive(true);
        Debug.Log("Wallet Starting...");
    }

    public void LogOutButtonClicked()
    {
        SaveUserData saveUserData = new SaveUserData();
        saveUserData.DeleteUserLoginData();
        HomePanel.SetActive(false);
        SideBarPanel.SetActive(false);
        LoginAndRegisterPanel.SetActive(true);
        RegisterPanel.SetActive(false);
        LoginPanel.SetActive(true);
        string _logoutText = "Logout Successfully";
        ShowAnyResponse(_logoutText);
    }

    public void ExitApp()
    {
        Application.Quit();
    }

}
