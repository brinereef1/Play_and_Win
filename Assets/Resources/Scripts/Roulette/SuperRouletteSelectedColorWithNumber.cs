using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
public class SuperRouletteSelectedColorWithNumber : MonoBehaviour
{
    [Header("AuthToken")]
    public string AuthTok;

    [Header("LastGameInfo Api_Url")]
    string lastgameinfo_liveUrl = "http://13.234.117.221:2556/api/v1/user/lastgameinfo_roulette";

    [Header("ScriptReference")]
    SaveUserData svd = new SaveUserData();

    [Header("Result Panel")]
    public GameObject ResutlPanel;

    [Header("Number Holder")]
    public TMP_Text number_holder;

    SuperRouletteIsWinnerManager superRouletteIsWinnerManager;
    RouletteBallController rouletteBallController;
    LastSixWinNumber lastSixWinNumber;
    SuperRouletteBetHistoryManager superRouletteBetHistoryManager;
    SuperRouletteWinningHistoryManager superRouletteWinningHistoryManager;
    SRLastTenWinnersHistoryManager srlastTenHistoryManager;

    public int num;
    void Start()
    {
        AuthTok = svd.GetSavedAuthToken();
        superRouletteIsWinnerManager = FindFirstObjectByType<SuperRouletteIsWinnerManager>();
        rouletteBallController = FindFirstObjectByType<RouletteBallController>();
        lastSixWinNumber = FindFirstObjectByType<LastSixWinNumber>();
        superRouletteBetHistoryManager = FindFirstObjectByType<SuperRouletteBetHistoryManager>();
        srlastTenHistoryManager = FindFirstObjectByType<SRLastTenWinnersHistoryManager>();

        // rouletteBallController.targetNumber = 10;
        // Debug.Log("Target Number: " + rouletteBallController.targetNumber);
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
                // Debug.Log("Response from Lastgameinfo: " + jsonResponse);
                SelectColorRoot chosen = JsonConvert.DeserializeObject<SelectColorRoot>(jsonResponse);
                string number = string.Join(", ", chosen.data.chosen.Keys);
                number = new string(number.TakeWhile(char.IsDigit).ToArray());
                Debug.Log("Chosen number: " + number);
                rouletteBallController.targetNumber = int.Parse(number.ToString());
                num = int.Parse(number);

            }
            else
            {
                Debug.Log("Error in sending request: " + request.error);
            }
        }
    }

    public int getNum()
    {
        return num;
    }

    public IEnumerator ShowResult(string number)
    {
        ResutlPanel.gameObject.SetActive(true);
        number_holder.text = number;
        yield return new WaitForSeconds(1f);
        ResutlPanel.gameObject.SetActive(false);
        number_holder.text = "";

        if (srlastTenHistoryManager != null)
        {
            srlastTenHistoryManager.LastTenWinHistoryButtonClick();
        }
        else
        {
            Debug.Log("Last Ten Win History Manager is Null");
        }

        if (lastSixWinNumber != null)
        {
            lastSixWinNumber.LastTenWinHistoryButtonClick();
        }

        if (superRouletteIsWinnerManager != null)
        {
            Debug.Log("is winner manager called");
            StartCoroutine(superRouletteIsWinnerManager.VictoryButtonClick());
        }
        else
        {
            Debug.Log("power ball script is null");
        }
        yield return new WaitForSeconds(1);
        

        if (superRouletteBetHistoryManager != null)
        {
            superRouletteBetHistoryManager.BetHistoryButtonClick();
        }

        if (superRouletteWinningHistoryManager != null)
        {
            superRouletteWinningHistoryManager.WinHistoryButtonClick();
        }
    }
}
[System.Serializable]
public class SelectColorData
{
    public Dictionary<string, double> chosen { get; set; }
}

[System.Serializable]
public class SelectColorRoot
{
    public bool status { get; set; }
    public SelectColorData data { get; set; }
}

