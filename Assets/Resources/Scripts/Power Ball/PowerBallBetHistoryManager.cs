using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.Networking;
public class PowerBallBetHistoryManager : MonoBehaviour
{
    [Header("BetPrefab Parent")]
    public Transform bet_prefabParent;

    [Header("betPrefab")]
    public GameObject betPrefab;
    public string AuthTok;
    private string bet_history_api_url = "http://13.234.117.221:2556/api/v1/user/userbethistory_power";
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
            PowerBallBetResponse betResponse = JsonConvert.DeserializeObject<PowerBallBetResponse>(response);
            if (request.result == UnityWebRequest.Result.Success)
            {
                foreach (var item in betResponse.betHistory)
                {
                    GameObject bet = Instantiate(betPrefab, bet_prefabParent);
                    var Script = bet.transform.GetComponent<PowerBallBetHistoryDisplay>();
                    Script.SetBetData(item.betAmount, item.gameRoundIdgenerated, item.categoryName, item.betUnit);
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
public class PowerBallBetDatum
{
    public string gameRoundIdgenerated { get; set; }
    public string categoryName {  get; set; }
    public int betAmount { get; set; }
    public int betUnit {  get; set; }
}
[System.Serializable]
public class PowerBallBetResponse
{
    public bool success { get; set; }
    public List<PowerBallBetDatum> betHistory { get; set; }
}