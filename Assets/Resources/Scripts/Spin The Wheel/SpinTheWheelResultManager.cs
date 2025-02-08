using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;

public class SpinTheWheelResultManager : MonoBehaviour
{
    [Header("AuthToken")]
    public string AuthTok;

    [Header("LastGameInfo Api_Url")]
    string lastgameinfo_liveUrl = "http://13.234.117.221:2556/api/v1/user/lastgameinfo_spinwheel";

    [Header("ScriptReference")]
    SaveUserData svd = new SaveUserData();

    [Header("Result Panel")]
    public GameObject ResutlPanel;

    [Header("Number Holder")]
    public TMP_Text number_holder;

    //JMIsWinnerManager jmIsWinnerManager;
    SpinTheWheelIsWinnerManager spinTheWheelIsWinnerManager;
    SpinTheWheelSpinnerController stw_spinnerController;
    SpinTheWheelLastTenWinHistoryManager spinTheWheelLastTenWinHistoryManager;

    void Start()
    {
        AuthTok = svd.GetSavedAuthToken();
        spinTheWheelIsWinnerManager = FindFirstObjectByType<SpinTheWheelIsWinnerManager>();
        stw_spinnerController = FindFirstObjectByType<SpinTheWheelSpinnerController>();
        spinTheWheelLastTenWinHistoryManager = FindFirstObjectByType<SpinTheWheelLastTenWinHistoryManager>();

        GetChosenNumber();
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
                STWRoot chosen = JsonConvert.DeserializeObject<STWRoot>(jsonResponse);
                string number = string.Join(", ", chosen.data.chosen.Keys);

                // singleSlotMachine.targetNumber = int.Parse(number.ToString());
                stw_spinnerController.targetColor = number;


                Debug.Log("Chosen number: " + number);
                //StartCoroutine(ShowResult(number));
            }
            else
            {
                Debug.LogError("Error in sending request: " + request.error);
            }
        }
    }

    public IEnumerator ShowResult(string number)
    {
        // yield return new WaitForSeconds(2f);
        ResutlPanel.gameObject.SetActive(true);
        number_holder.text = number;
        yield return new WaitForSeconds(3f);
        ResutlPanel.gameObject.SetActive(false);
        number_holder.text = "";

        if (spinTheWheelLastTenWinHistoryManager != null)
        {
            spinTheWheelLastTenWinHistoryManager.LastTenWinHistoryButtonClick();
        }
        else
        {
            Debug.Log("Last Ten Win History Manager is Null");
        }
        StartCoroutine(spinTheWheelIsWinnerManager.VictoryButtonClick());
    }
}
[System.Serializable]
public class STWResultData
{
    public Dictionary<string, int> chosen { get; set; }
}

[System.Serializable]
public class STWRoot
{
    public JMResultData data { get; set; }
}