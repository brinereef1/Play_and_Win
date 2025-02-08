using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
public class WinHistoryManager : MonoBehaviour
{
  
    [Header("WinPrefab Parent")]
    public Transform win_prefabParent;

    [Header("winPrefab")]
    public GameObject winPrefab;
    public string AuthTok;

    private string api_url = "http://13.234.117.221:2556/api/v1/user/userwinhistiry_dice";

    SaveUserData svd = new SaveUserData();
    // 
    void Start()
    {
        AuthTok = svd.GetSavedAuthToken();
        // hide win history panel
        WinHistoryButtonClick();
    }

    public void SetToken(string token)
    {
        AuthTok = token;
        //Debug.Log("Transaction Manager AuthTok::"+AuthTok);
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
        string AuthTok = GetToken();
        Debug.Log(AuthTok);

        using (UnityWebRequest request = UnityWebRequest.Get(api_url))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", AuthTok);
            request.SetRequestHeader("userType", "User");
            yield return request.SendWebRequest();

            string response = request.downloadHandler.text;
            WinResponse winResponse = JsonConvert.DeserializeObject<WinResponse>(response);
            if (request.result == UnityWebRequest.Result.Success)
            {

                foreach (var item in winResponse.data)
                {

                    // Instantiate the win history object
                    GameObject win = Instantiate(winPrefab, win_prefabParent);
                    var Script = win.transform.GetComponent<WinHistoryDisplay>();

                    // Set the values including the formatted IST date
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
public class Datum
{
    public string gameRoundId { get; set; }
    public int betAmount { get; set; }
    public int winningAmount { get; set; }
}
[System.Serializable]
public class WinResponse
{
    public bool success { get; set; }
    public List<Datum> data { get; set; }
}