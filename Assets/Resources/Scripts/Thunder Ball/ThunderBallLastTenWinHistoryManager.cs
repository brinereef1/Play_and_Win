using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
public class ThunderBallLastTenWinHistoryManager : MonoBehaviour
{

    [Header("WinPrefab Parent")]
    public Transform LastTenWin_prefabParent;
    [Header("WinPrefab parent 2")]
    public Transform LastTenWin_prefabParent2;
    [Header("List Of Balls")]
    public List<GameObject> ballList;
    [Header("winPrefab")]
    public GameObject LastTenWin_prefab;
    [Header("AuthenticationToken")]
    public string AuthTok;

    private string api_url = "http://13.234.117.221:2556/api/v1/user/last10thunder";

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

            TBLastTenWinResponse lastTen_winResponse = JsonConvert.DeserializeObject<TBLastTenWinResponse>(response);
            if (request.result == UnityWebRequest.Result.Success)
            {

                foreach (var item in lastTen_winResponse.games)
                {
                    // Instantiate the win history object
                    GameObject win = Instantiate(LastTenWin_prefab, LastTenWin_prefabParent);
                    var Script = win.transform.GetComponent<ThunderBallLastTenWinDisplay>();

                    // Check if chosen.Keys is null or empty
                    string chosenKey = (item.chosen?.Keys?.FirstOrDefault() != null)
                        ? item.chosen.Keys.First().ToString()
                        : "null";

                    // Set the values including the formatted IST date
                    Script.SetLastTenWinData(item.gameRoundId, chosenKey);
                }              

                foreach (var game in lastTen_winResponse.games)
                {
                    foreach (var key in game.chosen.Keys)
                    {
                        GameObject matchedColor = null;

                        // Check if the key matches any color GameObject name
                        foreach (GameObject colorObj in ballList)
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
                            GameObject colorInstance = Instantiate(matchedColor, LastTenWin_prefabParent2);
                            colorInstance.name = matchedColor.name;

                            // Access the child object and change its color
                            Transform childTransform = colorInstance.transform.GetChild(0); // Assuming the child is at index 0
                            if (childTransform != null)
                            {
                                Image childImageComponent = childTransform.GetComponent<Image>();
                                if (childImageComponent != null)
                                {
                                    // Set your desired color here
                                    childImageComponent.color = new Color(0f, 1f, 0f, 1f); // Example: Green color
                                }
                            }

                            Debug.Log($"Added {matchedColor.name} to the parent and changed the color of its child");
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
public class TBLastTenDatum
{
    public string gameRoundId { get; set; }
    public Dictionary<string, int> chosen = new Dictionary<string, int>();
}

[System.Serializable]
public class TBLastTenWinResponse
{
    public List<TBLastTenDatum> games { get; set; }
}