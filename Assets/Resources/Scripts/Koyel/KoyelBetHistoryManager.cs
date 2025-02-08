using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class KoyelBetHistoryManager : MonoBehaviour
{
    [Header("BetPrefab Parent")]
    public Transform bet_prefabParent;

    [Header("betPrefab")]
    public GameObject betPrefab;
    string AuthTok;

    private string bet_history_api_url = "http://13.234.117.221:2556/api/v1/user/userbethistory_koyel";

    SaveUserData svd = new SaveUserData();

    public GameObject betHistoryPanel;

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

    public void betHistoryPanel_BackButton()
    {
        // hide win history panel
        betHistoryPanel.gameObject.SetActive(false);

    }
    IEnumerator BetHistoryRequest()
    {
        Debug.Log("BetHistoryCalled1");
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
            KoyelBetResponse betResponse = JsonConvert.DeserializeObject<KoyelBetResponse>(response);
            if (request.result == UnityWebRequest.Result.Success)
            {
                foreach (var item in betResponse.betHistory)
                {
                    // Instantiate the win history object
                    GameObject win = Instantiate(betPrefab, bet_prefabParent);
                    var Script = win.transform.GetComponent<KoyelBetHistoryDisplay>();

                    // Set the values including the formatted IST date
                    Script.SetBetData(item.betAmount, item.gameRoundIdgenerated);
                }
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
public class KoyelBetDatum
{
    public string gameRoundIdgenerated { get; set; }
    public int betAmount { get; set; }
}
[System.Serializable]
public class KoyelBetResponse
{
    public bool success { get; set; }
    public List<LuckySevenBetDatum> betHistory { get; set; }
}