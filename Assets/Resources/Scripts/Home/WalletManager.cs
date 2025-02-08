using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Collections;
using System.Text;

public class WalletManager : MonoBehaviour
{
    public TMP_Text total_balance_text;
    public TMP_Text total_balance_text_home;
    public TMP_InputField withdrawl_amount_text;
    public TMP_InputField add_amount_text;

    private string AuthTok;
    SaveUserData svd = new SaveUserData();

    // Server endpoints
    private string getBalanceUrl = "http://13.234.117.221:2556/api/v1/user/usertotalwalletbalance";
    private string addMoneyUrl = "http://13.234.117.221:2556/api/v1/user/addmoney";
   
    
    void Start()
    {
        AuthTok = svd.GetSavedAuthToken();
        GetWalletBalance();
    }

    public void AddMoneyButton()
    {
        if (string.IsNullOrEmpty(add_amount_text.text))
        {
            Debug.LogWarning("Please enter an amount to add.");
            return;
        }

        double amountToAdd = double.Parse(add_amount_text.text);
        StartCoroutine(AddMoneyCoroutine(amountToAdd));
    }

    public void WithdrawlButton()
    {
        Debug.Log("Withdrawl Button");
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

    // Coroutine to send GET request and update the balance text
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

                WalletResponse response = JsonConvert.DeserializeObject<WalletResponse>(jsonResponse);
                total_balance_text.text = response.totalBalance.ToString() + "/-";
                total_balance_text_home.text = response.totalBalance.ToString() + "/-";
            }
            else
            {
                Debug.Log("" + request.downloadHandler.text);
                total_balance_text.text = "";
                total_balance_text_home.text = "";
            }
        }
    }

    // Coroutine to send POST request for adding money
    private IEnumerator AddMoneyCoroutine(double amount)
    {
        string AuthTok = GetToken();
        Debug.Log("Token From Wallet (Add Money): " + AuthTok);

        // Create the data object for the POST request
        AddMoneyRequest requestData = new AddMoneyRequest
        {
            requestedAmount = amount,
        };

        // Convert the data object to JSON
        string jsonData = JsonConvert.SerializeObject(requestData);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);

        // Create the UnityWebRequest for POST with the correct URL
        using (UnityWebRequest request = new UnityWebRequest(addMoneyUrl, UnityWebRequest.kHttpVerbPOST))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", AuthTok);
            request.SetRequestHeader("userType", "User");

            // Send the request and wait for the response
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = request.downloadHandler.text;
                Debug.Log("Add Money Response: " + jsonResponse);

                // Optionally, update the balance after adding money
                GetWalletBalance();
            }
            else
            {
                Debug.LogError("Error (Add Money): " + request.error);
            }
        }
    }

}

// Helper class for parsing GET response
[System.Serializable]
public class WalletResponse
{
    public double totalBalance;
}

// Helper class for POST request data
[System.Serializable]
public class AddMoneyRequest
{
    public double requestedAmount;

}
