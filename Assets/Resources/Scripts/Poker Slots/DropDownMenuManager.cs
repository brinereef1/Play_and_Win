using UnityEngine;

public class DropDownMenuManager : MonoBehaviour
{
    [Header("DropDownMenu")]
    public GameObject dropDownMenu;
    [Header("DropDownMenu active or no")]
    [SerializeField] bool isActive = false;
    [Header("WinHistoryPanel")]
    [SerializeField] public GameObject WinHistoryPanel;
    [SerializeField] public GameObject LastTenWinnersPanel;
    [SerializeField] public GameObject BetHistoryPanel;
    [SerializeField] public GameObject GameRulesPanel;

    void Start()
    {
        dropDownMenu.gameObject.SetActive(false);
    }
    public void DropDownMenuButtonClicked()
    {
        if (!isActive)
        {
            dropDownMenu.gameObject.SetActive(true);
            isActive = true;
        }
        else
        {
            dropDownMenu.gameObject.SetActive(false);
            isActive = false;
        }
    }

    public void onClickCloseButton()
    {
        dropDownMenu.gameObject.SetActive(false);
        isActive = false;
    }

    public void onClickShowWinHistoryButton()
    {
        Debug.Log("onClickShowWinHistoryButton");
        LastTenWinnersPanel.gameObject.SetActive(false);
        BetHistoryPanel.gameObject.SetActive(false);
        WinHistoryPanel.gameObject.SetActive(true);
        onClickCloseButton();
    }

    public void onClickShowLastTenWinnersButton()
    {
        Debug.Log("onClickShowLastTenWinnersButton");
        LastTenWinnersPanel.gameObject.SetActive(true);
        BetHistoryPanel.gameObject.SetActive(false);
        WinHistoryPanel.gameObject.SetActive(false);
        onClickCloseButton();
    }

    public void onClickShowBetHistoryButton()
    {
        Debug.Log("onClickShowBetHistoryButton");
        LastTenWinnersPanel.gameObject.SetActive(false);
        BetHistoryPanel.gameObject.SetActive(true);
        WinHistoryPanel.gameObject.SetActive(false);
        onClickCloseButton();
    }


    public void DropDownMenuPanelsBackButtonClick()
    {
        LastTenWinnersPanel.gameObject.SetActive(false);
        BetHistoryPanel.gameObject.SetActive(false);
        WinHistoryPanel.gameObject.SetActive(false);
        GameRulesPanel.gameObject.SetActive(false);

        onClickCloseButton();
    }

    public void onClickGameRulesButton()
    {
        GameRulesPanel.gameObject.SetActive(true);
    }

}
