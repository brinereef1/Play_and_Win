using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject home_panel;
    public GameObject patti_panel;
    public GameObject single_panel;

    void Start()
    {

    }

    public void OnClickPattiButton()
    {
        patti_panel.SetActive(true);
        single_panel.SetActive(false);
        home_panel.SetActive(false);
    }

    public void OnClickSinglePanelButton()
    {
        single_panel.SetActive(true);
        patti_panel.SetActive(false);
        home_panel.SetActive(false);
    }

    public void OnClickBackButton()
    {
        home_panel.SetActive(true);
        patti_panel.SetActive(false);
        single_panel.SetActive(false);
    }
    public void ExitFromFataFat()
    {
        SceneManager.LoadScene("Home");
    }
}
