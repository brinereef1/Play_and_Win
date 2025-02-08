using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections;
public class IsWinnerManager : MonoBehaviour
{
    [Header("WinPrefab Parent")]
    public Transform victory_prefabParent;

    [Header("winPrefab")]
    public GameObject victory_prefab;

    [Header("AuthenticationToken")]
    public string AuthTok;

    [Header("ApiEndPoint")]
    private string api_url = "http://13.234.117.221:2556/api/v1/user/isWinner_dice";
    SaveUserData svd = new SaveUserData();

    void Start()
    {
        AuthTok = svd.GetSavedAuthToken();

    }

    public void SetToken(string token)
    {
        AuthTok = token;
    }

    public string GetToken()
    {
        return AuthTok;
    }

    public IEnumerator VictorButtonClick()
    {
        StartCoroutine(IsWinner());
        yield return new WaitForSeconds(5);
        if ( victory_prefabParent.childCount > 0 )
        {
            Destroy(victory_prefabParent.gameObject);
        }
    }

    IEnumerator IsWinner()
    {
        string AuthTok = GetToken();
        using (UnityWebRequest request = UnityWebRequest.Get(api_url))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", AuthTok);
            request.SetRequestHeader("userType", "User");
            yield return request.SendWebRequest();

            string response = request.downloadHandler.text;

            IsWinner isWinner = JsonConvert.DeserializeObject<IsWinner>(response);
            if (request.result == UnityWebRequest.Result.Success)
            {
                if (isWinner.success == true)
                {
                    Debug.Log(isWinner.message);

                    GameObject victory_clone = Instantiate(victory_prefab, victory_prefabParent);
                    victory_clone.transform.localPosition = Vector3.zero;

                    var Script = victory_clone.transform.GetComponent<IsWinnerDisplay>();
                    if (Script != null)
                    {

                        Script.DisplayIsWinner(isWinner.message, isWinner.data.totalWinningAmount);
                    }
                    else
                    {
                        Debug.LogError("Script not attached");
                    }

                }
                else
                {
                    Debug.Log(isWinner.message);
                }
            }

        }
    }

}
[System.Serializable]
public class IsWinner
{
    public bool success { get; set; }
    public string message { get; set; }
    public Data data { get; set; }
}

[System.Serializable]
public class Data
{
    public int totalWinningAmount { get; set; }
}