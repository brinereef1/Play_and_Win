using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
public class SpinTheWheelLastTenWinHistoryManager : MonoBehaviour
{

    [Header("WinPrefab Parent")]
    public Transform LastTenWin_prefabParent;
    [Header("WinPrefab parent 2")]
    public Transform LastTenWin_prefabParent2;
    [Header("List Of Balls")]
    public List<GameObject> cardList;
    [Header("winPrefab")]
    public GameObject LastTenWin_prefab;
    [Header("AuthenticationToken")]
    public string AuthTok;

    private string api_url = "http://13.234.117.221:2556/api/v1/user/last10spinwheel";

    SaveUserData svd = new SaveUserData();
    // 
    void Start()
    {
        AuthTok = svd.GetSavedAuthToken();
        // hide win history panel
        LastTenWinHistoryButtonClick();
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


    public void LastTenWinHistoryButtonClick()
    {
        ClearWins();
        StartCoroutine(LastTenWinHistoryRequest());
    }

    IEnumerator LastTenWinHistoryRequest()
    {
        Debug.Log("LastTenWinHistoryCalled");
        string AuthTok = GetToken();
        Debug.Log(AuthTok);

        using (UnityWebRequest request = UnityWebRequest.Get(api_url))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", AuthTok);
            request.SetRequestHeader("userType", "User");
            yield return request.SendWebRequest();

            string response = request.downloadHandler.text;
            Debug.Log("Response from last10poker: " + response);

            STWLastTenWinResponse lastTen_winResponse = JsonConvert.DeserializeObject<STWLastTenWinResponse>(response);
            if (request.result == UnityWebRequest.Result.Success)
            {
                string card = "";

                foreach (var item in lastTen_winResponse.games)
                {
                    // Instantiate the win history object
                    GameObject win = Instantiate(LastTenWin_prefab, LastTenWin_prefabParent);
                    var Script = win.transform.GetComponent<SpinTheWheelLastTenWinDisplay>();

                    // Set the values including the formatted IST date
                    if (item.chosen == null || !item.chosen.Any())
                    {
                        card = "Null";
                    }
                    else
                    {
                        card = item.chosen.Keys.First().ToString();
                    }

                    Script.SetLastTenWinData(item.gameRoundId, card);
                }

                foreach (var game in lastTen_winResponse.games)
                {
                    foreach (var key in game.chosen.Keys)
                    {
                        //string tempKey = key.ToString();
                        GameObject matchedColor = null;

                        // Check if the key matches any color GameObject name
                        foreach (GameObject colorObj in cardList)
                        {
                            //string cardName = colorObj.name.Replace(" ", "");
                            //Debug.Log("cardName: " + tempKey);
                            if (key.Equals(colorObj.name))
                            {
                                Debug.Log("cardName: " + key);

                                matchedColor = colorObj;
                                break;
                            }
                        }

                        // If a match is found, instantiate the object and add it to the parent
                        if (matchedColor != null)
                        {
                            GameObject colorInstance = Instantiate(matchedColor, LastTenWin_prefabParent2);
                            colorInstance.name = matchedColor.name;
                            Debug.Log($"Added {matchedColor.name} to the parent");
                        }
                    }

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
        foreach (Transform child in LastTenWin_prefabParent)
        {
            if (child != null)
            {
                Destroy(child.gameObject);
            }
        }

        foreach (Transform child in LastTenWin_prefabParent2)
        {
            if (child != null)
            {
                Destroy(child.gameObject);
            }
        }
    }


}
[System.Serializable]
public class STWLastTenDatum
{
    public string gameRoundId { get; set; }
    public Dictionary<string, int> chosen = new Dictionary<string, int>();
}

[System.Serializable]
public class STWLastTenWinResponse
{
    public List<STWLastTenDatum> games { get; set; }
}