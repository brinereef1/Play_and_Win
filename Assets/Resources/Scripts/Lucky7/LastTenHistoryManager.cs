using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
public class LastTenHistoryManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] public GameObject rectAnglePanel;
    [SerializeField] public GameObject LastTenWinPanelView;

    [Header("WinPrefab Parent")]
    public Transform LastTenWin_prefabParent;
    public Transform LastTenWin_prefabParent2;

    [Header("List Of Balls")]
    public List<GameObject> cardList;

    [Header("winPrefab")]
    public GameObject LastTenWin_prefab;

    [Header("AuthenticationToken")]
    public string AuthTok;

    private string api_url = "http://13.234.117.221:2556/api/v1/user/last10dice";

    SaveUserData svd = new SaveUserData();
    // 
    void Start()
    {
        LastTenWinPanelView.gameObject.SetActive(false);
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

    public void winHistoryPanel_Click()
    {
        Debug.Log("clicked...");
        LastTenWinPanelView.gameObject.SetActive(true);  // hide win history panel
    }

    public void winHistoryPanel_BackButton()
    {
        rectAnglePanel.gameObject.SetActive(false); // hide win animation
        LastTenWinPanelView.gameObject.SetActive(false);
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
            Debug.Log("Response from last10dice: " + response);
            LastTenWinResponse lastTen_winResponse = JsonConvert.DeserializeObject<LastTenWinResponse>(response);
            if (request.result == UnityWebRequest.Result.Success)
            {

                foreach (var item in lastTen_winResponse.games)
                {
                    int sum = item.showDice.Sum();
                    Debug.Log(sum);
                    // Instantiate the win history object
                    GameObject win = Instantiate(LastTenWin_prefab, LastTenWin_prefabParent);
                    var Script = win.transform.GetComponent<LastTenHistoryDisplay>();
                    // Set the values including the formatted IST date
                    Script.SetLastTenWinData(item.gameRoundId, item.chosenCard, sum);
                }

                foreach (var game in lastTen_winResponse.games)
                {
                        string sum = game.showDice.Sum().ToString();
                   
                        //string tempKey = key.ToString();
                        GameObject matchedColor = null;

                        // Check if the key matches any color GameObject name
                        foreach (GameObject colorObj in cardList)
                        {
                           
                            if (sum.Equals(colorObj.name))
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
                            Debug.Log($"Added {matchedColor.name} to the parent");
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
public class LastTenDatum
{
    public string gameRoundId { get; set; }
    public string chosenCard { get; set; }
    public List<int> showDice { get; set; }
}

[System.Serializable]
public class LastTenWinResponse
{

    public List<LastTenDatum> games { get; set; }
}