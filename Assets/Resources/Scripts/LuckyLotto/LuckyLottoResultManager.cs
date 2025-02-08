using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using System.Linq;

public class LuckyLottoResultManger : MonoBehaviour
{
    [Header("AuthToken")]
    public string AuthTok;

    [Header("LastGameInfo Api_Url")]
    string lastgameinfo_liveUrl = "http://13.234.117.221:2556/api/v1/user/lastgameinfo";

    [Header("ScriptReference")]
    SaveUserData svd = new SaveUserData();
    [Header("Result Panel")]
    public GameObject ResutlPanel;
    [Header("Number Holder")]
    public TMP_Text number_holder;
    [SerializeField] public string result = "";


    LuckyLottoSlotMachine slotMachine;
    LuckLottoIsWinnerManager isWinnerManager;
    LuckyLottoLastTenWinHistoryManager luckyLottoLastTenWinHistoryManager;

    void Start()
    {
        AuthTok = svd.GetSavedAuthToken();
        slotMachine = FindFirstObjectByType<LuckyLottoSlotMachine>();
        isWinnerManager = FindAnyObjectByType<LuckLottoIsWinnerManager>();
        luckyLottoLastTenWinHistoryManager = FindFirstObjectByType<LuckyLottoLastTenWinHistoryManager>();

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
        slotMachine.newList.Clear();
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
                Debug.Log("Json response: " + jsonResponse);


                LuckyLottoRoot chosen = JsonConvert.DeserializeObject<LuckyLottoRoot>(jsonResponse);
                foreach (var data in chosen.data.showCards)
                {

                    string temp = data.suit + data.color + data.value;
                    slotMachine.newList.Add(temp.ToLower());
                }

                result = chosen.data.chosen.Keys.First().ToString();

            }
            else
            {
                Debug.LogError("Error in sending request: " + request.error);
            }
        }
    }

    public IEnumerator ShowResult()
    {
        string number = result.ToString();

        yield return new WaitForSeconds(2f);
        ResutlPanel.gameObject.SetActive(true);
        number_holder.text = number;
        yield return new WaitForSeconds(1f);
        ResutlPanel.gameObject.SetActive(false);
        number_holder.text = "";

        if (luckyLottoLastTenWinHistoryManager != null)
        {
            luckyLottoLastTenWinHistoryManager.LastTenWinHistoryButtonClick();
        }
        else
        {
            Debug.Log("Last Ten Win History Manager is Null");
        }
        StartCoroutine(isWinnerManager.VictoryButtonClick());
    }
}



[System.Serializable]
public class LuckyLottoData
{
    public Dictionary<string, int> chosen { get; set; }
    public List<LuckyLottoShowCard> showCards { get; set; }

}
[System.Serializable]
public class LuckyLottoRoot
{
    public bool status { get; set; }
    public LuckyLottoData data { get; set; }
}
[System.Serializable]
public class LuckyLottoShowCard
{
    public string value { get; set; }
    public string suit { get; set; }
    public string color { get; set; }
}