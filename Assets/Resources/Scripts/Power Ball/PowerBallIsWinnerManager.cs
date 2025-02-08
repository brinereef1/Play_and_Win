using System.Collections;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class PowerBallIsWinnerManager : MonoBehaviour
{
    [Header("WinPrefab Parent")]
    public Transform victory_prefabParent;
    [Header("WinPrefab")]
    public GameObject victory_prefab;

    [Header("AuthenticationToken")]
    public string AuthTok;

    [Header("ApiEndPoint")]
    private string powerballIsWinner_api_url = "http://13.234.117.221:2556/api/v1/user/iswinner_power";
    SaveUserData svd = new SaveUserData();
    PowerBallSpin powerBallSpin;

    void Start()
    {
        AuthTok = svd.GetSavedAuthToken();
        powerBallSpin = FindFirstObjectByType<PowerBallSpin>();
    }

    public void SetToken(string token)
    {
        AuthTok = token;
    }
    public string GetToken()
    {
        return AuthTok;
    }

    public IEnumerator VictoryButtonClick()
    {
        Debug.Log("victoryCalled");
        StartCoroutine(IsWinner());
        yield return new WaitForSeconds(5);
        if (victory_prefabParent.childCount > 0)
        {
            GameObject instantiatedPrefab = victory_prefabParent.GetChild(0).gameObject;
            Destroy(instantiatedPrefab);
        }
        powerBallSpin.ResetBallPosition();
    }

    IEnumerator IsWinner()
    {
        Debug.Log("Winner Called");
        string AuthTok = GetToken();
        using (UnityWebRequest request = UnityWebRequest.Get(powerballIsWinner_api_url))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", AuthTok);
            request.SetRequestHeader("userType", "User");
            yield return request.SendWebRequest();
            string response = request.downloadHandler.text;
            Debug.Log("Token from IsWinManager:" + AuthTok);
            Debug.Log("Response from IsWinManager:" + response);
            PowerBallIsWinnerRoot isWinner = JsonConvert.DeserializeObject<PowerBallIsWinnerRoot>(response);
            if (request.result == UnityWebRequest.Result.Success)
            {
                if (isWinner.success == true)
                {
                    Debug.Log(isWinner.message);
                    GameObject victory_clone = Instantiate(victory_prefab, victory_prefabParent);
                    victory_clone.transform.localPosition = Vector3.zero;

                    var Script = victory_clone.transform.GetComponent<PowerBallIsWinnerDisplay>();
                    if (Script != null)
                    {
                        Script.DisplayIsWinner(isWinner.message, isWinner.data.totalWinningAmount);
                    }
                    else
                    {
                        Debug.LogError("Script not attached.");
                    }
                }
                else
                {
                    Debug.Log(isWinner.message);
                }

            }
            else
            {
                Debug.Log("Error: " + request.result);
            }
        }
    }
}
[System.Serializable]
public class PowerBallIsWinnerRoot
{
    public bool success { get; set; }
    public string message { get; set; }
    public PokerSlotData data { get; set; }
}

[System.Serializable]
public class PowerBallData
{
    public int totalWinningAmount { get; set; }
}