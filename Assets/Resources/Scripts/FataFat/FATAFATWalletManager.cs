using UnityEngine;
using TMPro;
using Newtonsoft.Json;
using UnityEngine.Networking;
using System.Collections;
public class FATAFATWalletManager : MonoBehaviour
{
    public TMP_Text patti_total_balance_text;
    public TMP_Text single_total_balance_text;

    private string AuthTok;
    SaveUserData svd = new SaveUserData();

    // Server endpoints
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

                FATAFATWalletResponse response = JsonConvert.DeserializeObject<FATAFATWalletResponse>(jsonResponse);
                patti_total_balance_text.text = response.totalBalance.ToString() + "/-";
                single_total_balance_text.text = response.totalBalance.ToString() + "/-";
            }
            else
            {
                Debug.LogError("Error: " + request.error);
            }
        }
    }
}
[System.Serializable]
public class FATAFATWalletResponse
{
    public double totalBalance;
}