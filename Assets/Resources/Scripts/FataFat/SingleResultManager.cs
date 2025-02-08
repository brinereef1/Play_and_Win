using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;

public class SingleResultManager : MonoBehaviour
{
    [Header("AuthToken")]
    public string AuthTok;

    [Header("LastGameInfo Api_Url")]
    string lastgameinfo_liveUrl = "http://13.234.117.221:2556/api/v1/user/lastgameinfo_fatafatsingle";

    [Header("ScriptReference")]
    SaveUserData svd = new SaveUserData();
    [Header("Result Panel")]
    public GameObject ResutlPanel;
    [Header("Number Holder")]
    public TMP_Text number_holder;

    SingleSlotMachine singleSlotMachine;
    SingleIsWinnerManager singleIsWinnerManager;
    SingleLastTenWinHistoryManager single_lastTenWinHistoryManager;

    void Start()
    {
        AuthTok = svd.GetSavedAuthToken();
        singleSlotMachine = FindFirstObjectByType<SingleSlotMachine>();
        singleIsWinnerManager = FindFirstObjectByType<SingleIsWinnerManager>();
        single_lastTenWinHistoryManager = FindFirstObjectByType<SingleLastTenWinHistoryManager>();
        // GetChosenNumber();
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
                // Debug.Log("Response from lastgameinfo: " + jsonResponse);
                SingleRoot chosen = JsonConvert.DeserializeObject<SingleRoot>(jsonResponse);
                string number = string.Join(", ", chosen.data.chosen.Keys);
                singleSlotMachine.targetNumber = int.Parse(number.ToString());
                // Debug.Log("Chosen number: " + number);
                // StartCoroutine(ShowResult(number));
            }
            else
            {
                Debug.LogError("Error in sending request: " + request.error);
            }
        }
    }

    public IEnumerator ShowResult(string number)
    {
        yield return new WaitForSeconds(2f);
        ResutlPanel.gameObject.SetActive(true);
        number_holder.text = number;
        yield return new WaitForSeconds(1f);
        ResutlPanel.gameObject.SetActive(false);
        number_holder.text = "";

        if (single_lastTenWinHistoryManager != null)
        {
            single_lastTenWinHistoryManager.LastTenWinHistoryButtonClick();
        }
        else
        {
            Debug.Log("Last Ten Win History Manager is Null");
        }
        StartCoroutine(singleIsWinnerManager.VictoryButtonClick());
    }
}
[System.Serializable]
public class SingleData
{
    public Dictionary<string, int> chosen { get; set; }
}

[System.Serializable]
public class SingleRoot
{
    public bool status { get; set; }
    public SingleData data { get; set; }
}