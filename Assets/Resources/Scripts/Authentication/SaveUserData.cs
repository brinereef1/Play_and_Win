using System.Text;
using UnityEngine;

public class SaveUserData : MonoBehaviour
{
    HomeUIManager _UIManager;
    
    void Start()
    {
        _UIManager = FindFirstObjectByType<HomeUIManager>();
        // Check if the user is already logged in
        if (IsUserLoggedIn())
        {
            Debug.Log("User is already logged in.");
            // Fetch the saved token
            string savedToken = GetSavedAuthToken();
            Debug.Log("Saved Token:: " + savedToken);
            _UIManager.name_text.text = PlayerPrefs.GetString("name", "");
            _UIManager.email_text.text = PlayerPrefs.GetString("email", "");
            _UIManager.LoginAndRegisterPanel.SetActive(false);
            _UIManager.Home();

        }
        else
        {
            _UIManager.LoginAndRegisterPanel.SetActive(true);
            _UIManager.RegisterPanel.SetActive(false);
            _UIManager.LoginPanel.SetActive(true);
            Debug.Log("User is not logged in.");
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SaveLoginData(string name,string email,string token)
    {
        Debug.Log("User Token:: " + token);

        string encryptedToken = Encrypt(token);
        string s_name = name;
        string s_email = email;
        PlayerPrefs.SetString("token", encryptedToken);
        PlayerPrefs.SetString("name",s_name);
        PlayerPrefs.SetString("email",s_email);
    }

    public void DeleteUserLoginData()
    {
        ClearLoginData();
    }

    private string Encrypt(string text)
    {
        return System.Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
    }

    private string Decrypt(string encryptedText)
    {
        // Implement decryption matching the encryption algorithm
        return Encoding.UTF8.GetString(System.Convert.FromBase64String(encryptedText));
    }

    public string GetSavedAuthToken()
    {
        string encryptedToken = PlayerPrefs.GetString("token", "");
        return Decrypt(encryptedToken);
    }

    private bool IsUserLoggedIn()
    {
        return PlayerPrefs.HasKey("token"); // check if the token is exist                
    }

    private void ClearLoginData()
    {
        // PlayerPrefs.DeleteKey("name");
        // PlayerPrefs.DeleteKey("email");
        PlayerPrefs.DeleteKey("token");
        PlayerPrefs.DeleteKey("name");
        PlayerPrefs.DeleteKey("email");
        Debug.Log("Clear login data...");

        PlayerPrefs.Save(); // Save Changes
    }
}
