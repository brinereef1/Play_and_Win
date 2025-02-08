using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class JMBetHistoryManager : MonoBehaviour
{
    [Header("BetPrefab Parent")]
    public Transform win_prefabParent;

    [Header("betPrefab")]
    public GameObject winPrefab;
    public string AuthTok;

    private string bet_history_api_url = "http://13.234.117.221:2556/api/v1/user/userbethistory_jhandimunda";

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
            Debug.Log(response);
            JMBetResponse betResponse = JsonConvert.DeserializeObject<JMBetResponse>(response);
            if (request.result == UnityWebRequest.Result.Success)
            {
                foreach (var item in betResponse.betHistory)
                {
                    // Instantiate the win history object
                    GameObject win = Instantiate(winPrefab, win_prefabParent);
                    var Script = win.transform.GetComponent<JMBetHistoryDisplay>();

                    // Set the values including the formatted IST date
                    Script.SetWinData(item.betAmount, item.gameRoundIdgenerated, item.categoryName,item.betUnit);
                }
            }

        }

    }


    public void ClearWins()
    {

        if (win_prefabParent != null)
        {
            foreach (Transform child in win_prefabParent)
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
public class JMBetHistoryData
{
    public string gameRoundIdgenerated { get; set; }
    public string categoryName { get; set; }
    public int betAmount { get; set; }
    public int betUnit { get; set; }    
}
[System.Serializable]
public class JMBetResponse
{
    public bool success { get; set; }
    public List<JMBetHistoryData> betHistory { get; set; }
}