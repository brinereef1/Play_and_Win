using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Collections;
using System.Linq;

public class LastSixWinNumber : MonoBehaviour
{

    [Header("Colors")]
    public GameObject[] colors;

    [Header("WinPrefab Parent")]
    public Transform lastsix_prefab_Parent;

    [Header("winPrefab")]
    public GameObject lastsix_prefab;
    [Header("AuthenticationToken")]
    public string AuthTok;

    private string api_url = "http://13.234.117.221:2556/api/v1/user/last10roulette";

    SaveUserData svd = new SaveUserData();


    void Start()
    {

        AuthTok = svd.GetSavedAuthToken();
        // hide win history panel
        LastTenWinHistoryButtonClick();

    }


    public void SetToken(string token)
    {
        AuthTok = token;
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

            string jsonData = request.downloadHandler.text;
            // Debug.Log("Response from last10poker: " + response);


            if (request.result == UnityWebRequest.Result.Success)
            {
            
                ServerResponse response = JsonConvert.DeserializeObject<ServerResponse>(jsonData);

                foreach (var game in response.games)
                {
                    foreach (var key in game.chosen.Keys)
                    {
                        GameObject matchedColor = null;

                        // Check if the key matches any color GameObject name
                        foreach (GameObject colorObj in colors)
                        {
                            if (key.Equals(colorObj.name))
                            {
                                matchedColor = colorObj;
                                break;
                            }
                        }

                        // If a match is found, instantiate the object and add it to the parent
                        if (matchedColor != null)
                        {
                            GameObject colorInstance = Instantiate(matchedColor, lastsix_prefab_Parent);
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
        foreach (Transform child in lastsix_prefab_Parent)
        {
            if (child != null)
            {
                Destroy(child.gameObject);
            }
        }
    }

}
[System.Serializable]
public class Game
{
    public string _id;
    public string gameRoundId;
    public Dictionary<string, double> chosen;
}

[System.Serializable]
public class ServerResponse
{
    public bool status;
    public string message;
    public List<Game> games;
}

[System.Serializable]
public class Chosen
{
    public Dictionary<string, double> chosen;
}