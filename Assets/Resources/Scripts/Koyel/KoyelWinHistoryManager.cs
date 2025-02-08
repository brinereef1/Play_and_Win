using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class KoyelWinHistoryManager : MonoBehaviour
{
    [Header("BetPrefab Parent")]
    public Transform win_prefabParent;

    [Header("betPrefab")]
    public GameObject winPrefab;
    public string AuthTok;
    //public string tempAuthToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiQWxpY2UgVXNlciIsImVtYWlsIjoidXNlcjFAZXhhbXBsZS5jb20iLCJwYXNzd29yZCI6InVzZXJwYXNzMTIzIiwidXNlcnR5cGUiOiJVc2VyIiwiaWF0IjoxNzMxNDQzNDM5fQ.5wu_oooJzt_S2HdtoewQBtnVG3mAz-UZRyWZPpcPhWE";
    private string win_history_api_url = "http://3.144.110.234:2556/api/v1/user/wiininghistory_koyel";

    SaveUserData svd = new SaveUserData();

    public GameObject winHistoryPanel;

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
        StartCoroutine(WinHistoryRequest());
    }

    public void betHistoryPanel_BackButton()
    {
        winHistoryPanel.gameObject.SetActive(false);
    }
    IEnumerator WinHistoryRequest()
    {
        Debug.Log("WinHistoryCalled1");
        AuthTok = GetToken();
        Debug.Log(AuthTok);

        using (UnityWebRequest request = UnityWebRequest.Get(win_history_api_url))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", AuthTok);
            request.SetRequestHeader("userType", "User");
            yield return request.SendWebRequest();

            string response = request.downloadHandler.text;
            Debug.Log(response);
            KoyelWinResponse winResponse = JsonConvert.DeserializeObject<KoyelWinResponse>(response);
            if (request.result == UnityWebRequest.Result.Success)
            {
                foreach (var item in winResponse.data)
                {                
                    GameObject win = Instantiate(winPrefab, win_prefabParent);
                    var Script = win.transform.GetComponent<KoyelWinHistoryDisplay>();                    
                    Script.SetWinData(item.betAmount, item.winningAmount, item.gameRoundId);
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
public class KoyelWinDatum
{
    public string gameRoundId { get; set; }
    public int betAmount { get; set; }
    public int winningAmount { get; set; }
}
[System.Serializable]
public class KoyelWinResponse
{
    public bool success { get; set; }
    public List<KoyelWinDatum> data { get; set; }
}