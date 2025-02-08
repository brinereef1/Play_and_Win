using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
public class PowerBallWinHistoryManager : MonoBehaviour
{
    [Header("WinPrefab Parent")]
    public Transform win_prefabParent;

    [Header("winPrefab")]
    public GameObject winPrefab;
    public string AuthTok;
    private string win_history_api_url = "http://13.234.117.221:2556/api/v1/user/userwinhistory_power";
    SaveUserData svd = new SaveUserData();

    void Start()
    {
        AuthTok = svd.GetSavedAuthToken();
        WinHistoryButtonClick();

    }

    public void SetToken(string token)
    {
        AuthTok = token;
    }

    public string GetToken()
    {
        return AuthTok;
    }

    public void WinHistoryButtonClick()
    {
        ClearWins();
        StartCoroutine(WinHistoryRequest());
    }

    IEnumerator WinHistoryRequest()
    {
        Debug.Log("WinHistoryCalled");
        AuthTok = GetToken();
        Debug.Log(AuthTok);

        using (UnityWebRequest request = UnityWebRequest.Get(win_history_api_url))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", AuthTok);
            request.SetRequestHeader("userType", "User");
            yield return request.SendWebRequest();
            string response = request.downloadHandler.text;
            PowerBallWinResponse powerBallWinResponse = JsonConvert.DeserializeObject<PowerBallWinResponse>(response);
            if (request.result == UnityWebRequest.Result.Success)
            {
                foreach (var item in powerBallWinResponse.data)
                {
                    GameObject win = Instantiate(winPrefab, win_prefabParent);
                    var Script = win.transform.GetComponent<PowerBallWinHistoryDisplay>();

                    Script.SetWinData(item.betAmount, item.winningAmount, item.gameRoundId);
                }
            }
            else
            {
                Debug.Log("Error: " + request.error);
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
public class PowerBallDatum
{
    public string gameRoundId { get; set; }
    public int betAmount { get; set; }
    public int winningAmount { get; set; }
}
[System.Serializable]
public class PowerBallWinResponse
{
    public bool success { get; set; }
    public List<PowerBallDatum> data { get; set; }
}
