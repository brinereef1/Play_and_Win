using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Networking;
using Newtonsoft.Json;
public class PokerSlotsWalletManager : MonoBehaviour
{
    public TMP_Text total_balance_text;
    private string AuthTok;
    SaveUserData svd = new SaveUserData();

   
    private string getBalanceUrl = "http://13.234.117.221:2556/api/v1/user/usertotalwalletbalance";

    void Start()
    {
        AuthTok = svd.GetSavedAuthToken();
        GetWalletBalance();
    }
    public void GetWalletBalance()
    {
        StartCoroutine(GetBalanceCoroutine());
    }
    public void SetToken(string token)
    {
        AuthTok = token;
    }

    public string GetToken()
    {
        return AuthTok;
    }
    private IEnumerator GetBalanceCoroutine()
    {
        string AuthTok = GetToken();
        Debug.Log("Token From Wallet: " + AuthTok);

        using (UnityWebRequest request = UnityWebRequest.Get(getBalanceUrl))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", AuthTok);
            request.SetRequestHeader("userType", "User");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = request.downloadHandler.text;
                Debug.Log("Response: " + jsonResponse);

                PSWalletResponse response = JsonConvert.DeserializeObject<PSWalletResponse>(jsonResponse);
                total_balance_text.text = response.totalBalance.ToString() + "/-";
            }
            else
            {
                Debug.LogError("Error: " + request.error);
            }
        }
    }
}
[System.Serializable]
public class PSWalletResponse
{
    public double totalBalance;
}