using Newtonsoft.Json;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
public class RegistrationViewModel : MonoBehaviour
{
    HomeUIManager _uiManager;
    void Start()
    {
        _uiManager = FindFirstObjectByType<HomeUIManager>();
    }

    public IEnumerator Register(UserModel userModel, System.Action<string> callback)
    {
        string url = "http://13.234.117.221:2556/api/v1/user/userReg";  // Replace with your actual API URL

        UserRegModel userRegModel = new UserRegModel();
        userRegModel.name = userModel.name;
        userRegModel.email = userModel.email;
        userRegModel.password = userModel.password;

        string jsonData = JsonConvert.SerializeObject(userRegModel);  // JSON data ko serialize karo

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);  // Data ko bytes me convert karo
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);  // Upload handler ko set karo
            request.downloadHandler = new DownloadHandlerBuffer();  // Response ko handle karne ke liye
            request.SetRequestHeader("Content-Type", "application/json");  // Content-Type header set karo

            // Request ko send karo aur response ka wait karo
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);

                string response = request.downloadHandler.text;
                var jsonResponse = JObject.Parse(response);
                string message = (string)jsonResponse["message"];




                _uiManager.ShowAnyResponse(message);
                callback?.Invoke(request.error);
            }
            else
            {

                Debug.Log("Registration Successful");
                _uiManager.ShowAnyResponse("User Registration Successfully");
                callback?.Invoke(request.downloadHandler.text);

                _uiManager = FindFirstObjectByType<HomeUIManager>();
                _uiManager.RegisterPanel.SetActive(false);
                _uiManager.LoginPanel.SetActive(true);

            }
        }
    }



}
[System.Serializable]
public class UserRegModel
{
    public string name { get; set; }
    public string email { get; set; }
    public string password { get; set; }
}