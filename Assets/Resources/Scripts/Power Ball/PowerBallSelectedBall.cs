using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using Newtonsoft.Json;

public class PowerBallSelectedBall : MonoBehaviour
{
    [Header("AuthToken")]
    public string AuthTok;

    [Header("LastGameInfo Api_Url")]
     string lastgameinfo_liveUrl = "http://13.234.117.221:2556/api/v1/user/lastgameinfo_power";

    [Header("ScriptReference")]
    SaveUserData svd = new SaveUserData();

    [Header("Result Panel")]
    public GameObject ResutlPanel;

    [Header("Number Holder")]
    public TMP_Text number_holder;

    PowerBallIsWinnerManager powerBallIsWinnerManager = new PowerBallIsWinnerManager();
    PowerBallSpin powerBallSpin;
    PowerBallLastTenWinManager powerBallLastTenWinManager;

    void Start()
    {
        AuthTok = svd.GetSavedAuthToken();
        powerBallIsWinnerManager = FindFirstObjectByType<PowerBallIsWinnerManager>();
        powerBallSpin = FindFirstObjectByType<PowerBallSpin>();
        powerBallLastTenWinManager = FindFirstObjectByType<PowerBallLastTenWinManager>();

    }

    public void SetToken(string token)
    {
        AuthTok = token;
    }

    public string GetToken()
    {
        return AuthTok;
    }

    public void GetChosenNumber()
    {
        StartCoroutine(GetChosenNumberRequest());
    }

    IEnumerator GetChosenNumberRequest()
    {
        string AuthTok = GetToken();
        using (UnityWebRequest request = UnityWebRequest.Get(lastgameinfo_liveUrl))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", AuthTok);
            request.SetRequestHeader("userType", "User");
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = request.downloadHandler.text;
                // Debug.log("Response from Lastgameinfo: " + jsonResponse);
                PowerBallRoot chosen = JsonConvert.DeserializeObject<PowerBallRoot>(jsonResponse);
                string number = string.Join(", ", chosen.data.chosen.Keys);
                 Debug.Log("Chosen number: " + number);
                if (number != null)
                {
                    powerBallSpin.TargetBallNumber = int.Parse(number.ToString());
                }
                else
                {
                    Debug.Log("Chosen is null");
                }

            }
            else
            {
                Debug.Log("Error in sending request: " + request.error);
            }
        }
    }

    public IEnumerator ShowResult(string number)
    {
        ResutlPanel.gameObject.SetActive(true);
        number_holder.text = number;
        yield return new WaitForSeconds(1.0f);
        ResutlPanel.gameObject.SetActive(false);
        number_holder.text = "";

        if (powerBallLastTenWinManager != null)
        {
            powerBallLastTenWinManager.LastTenWinHistoryButtonClick();
        }
        else
        {
            Debug.Log("Last Ten Win History Manager is Null");
        }

        if (powerBallIsWinnerManager != null)
        {
            Debug.Log("is winner manager called");
            StartCoroutine(powerBallIsWinnerManager.VictoryButtonClick());
        }
        else
        {
            Debug.Log("power ball script is null");
        }
    }
}
[System.Serializable]
public class SelectedBallData
{
    public Dictionary<string, int> chosen { get; set; }
}

[System.Serializable]
public class PowerBallRoot
{
    public bool status { get; set; }
    public SelectedBallData data { get; set; }
}