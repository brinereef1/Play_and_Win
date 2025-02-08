using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;

public class JMResultManager : MonoBehaviour
{
    [Header("AuthToken")]
    public string AuthTok;

    [Header("LastGameInfo Api_Url")]
     string lastgameinfo_liveUrl = "http://13.234.117.221:2556/api/v1/user/lastgameinfo_jhandimunda";

    [Header("ScriptReference")]
    SaveUserData svd = new SaveUserData();
    [Header("Result Panel")]
    public GameObject ResutlPanel;
    [Header("Number Holder")]
    public TMP_Text number_holder;

    JMIsWinnerManager jmIsWinnerManager;
    JMSpinnerController jmspinnerController;
    JMLastTenHistoryManager jmLASTTenHistoryManager;

    void Start()
    {
         AuthTok = svd.GetSavedAuthToken();
         jmIsWinnerManager = FindFirstObjectByType<JMIsWinnerManager>();
         jmspinnerController = FindFirstObjectByType<JMSpinnerController>();
        jmLASTTenHistoryManager = FindFirstObjectByType<JMLastTenHistoryManager>();

        //GetChosenNumber();
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
                JMRoot chosen = JsonConvert.DeserializeObject<JMRoot>(jsonResponse);
                string number = string.Join(", ", chosen.data.chosen.Keys);

                // singleSlotMachine.targetNumber = int.Parse(number.ToString());
                jmspinnerController.targetColor = number;


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
        yield return new WaitForSeconds(1f);
        ResutlPanel.gameObject.SetActive(false);
        number_holder.text = "";

        if (jmLASTTenHistoryManager != null)
        {
            jmLASTTenHistoryManager.LastTenWinHistoryButtonClick();
        }
        else
        {
            Debug.Log("Last Ten Win History Manager is Null");
        }
        StartCoroutine(jmIsWinnerManager.VictoryButtonClick());
    }
}
[System.Serializable]
public class JMResultData
{
    public Dictionary<string, int> chosen { get; set; }
}

[System.Serializable]
public class JMRoot
{
    public bool status { get; set; }
    public JMResultData data { get; set; }
}