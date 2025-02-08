using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class LuckyLottoBetHistoryManager : MonoBehaviour
{
    [Header("BetPrefab Parent")]
    public Transform bet_prefabParent;

    [Header("betPrefab")]
    public GameObject betPrefab;
    public string AuthTok;
    private string bet_history_api_url = "http://13.234.117.221:2556/api/v1/user/bethistory";
    SaveUserData svd = new SaveUserData();

    void Start()
    {
        AuthTok = svd.GetSavedAuthToken().ToString();
        BetHistoryButtonClick();
    }
    public void SetToken(string token)
    {
        AuthTok = token;
    }

    public string GetToken()
    {
        return AuthTok;
    }

    public void BetHistoryButtonClick()
    {
        ClearWins();
        StartCoroutine(BetHistoryRequest());
    }
    IEnumerator BetHistoryRequest()
    {
        Debug.Log("BetHistoryCalled");
        AuthTok = GetToken();
        Debug.Log(AuthTok);

        using (UnityWebRequest request = UnityWebRequest.Get(bet_history_api_url))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", AuthTok);
            request.SetRequestHeader("userType", "User");
            yield return request.SendWebRequest();
            string response = request.downloadHandler.text;
            Debug.Log("bet Response::" + response);
            LuckyLotoBetResponse betResponse = JsonConvert.DeserializeObject<LuckyLotoBetResponse>(response);
            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("bet Response::" + betResponse);
                foreach (var item in betResponse.betHistory)
                {                    
                        GameObject bet = Instantiate(betPrefab, bet_prefabParent);
                        var Script = bet.transform.GetComponent<LuckyLottoBetHistoryDisplay>();

                        // Use the `user.betAmount` and `user.winningAmount`
                        Script.SetBetData(item.betAmount, item.gameRoundIdgenerated, item.categoryName,item.betUnit);
                    
                }
            }
            else
            {
                Debug.LogError("Error in downloading: " + request.error);
            }
        }
    }

    

    public void ClearWins()
    {

        if (bet_prefabParent != null)
        {
            foreach (Transform child in bet_prefabParent)
            {
                if (child != null)
                {
                    Destroy(child.gameObject);
                }
            }
        }
    }
}
[System.Serializable]
public class LLBetHistoryData
{
    public string gameRoundIdgenerated { get; set; }
    public string categoryName { get; set; }
    public int betAmount { get; set; }
    public int betUnit {  get; set; }
}

[System.Serializable]
public class LuckyLotoBetResponse
{    
    public List<LLBetHistoryData> betHistory { get; set; }
}