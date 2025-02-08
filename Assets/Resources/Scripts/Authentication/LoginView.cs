using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LoginView : MonoBehaviour
{
    public TMP_InputField emailField;  // Phone number ka input
    public TMP_InputField passwordField;     // Password ka input         // Login button

    private LoginViewModel loginViewModel;

    void Start()
    {
        // LoginViewModel ko reference karo
        loginViewModel = GetComponent<LoginViewModel>();

    }



    // Jab Login button click ho, yeh function chalega
    public void OnLoginButtonClicked()
    {
        // Phone number aur password fields se values lo
        string phoneNumber = emailField.text;
        string password = passwordField.text;

        // Validation check karo
        if (string.IsNullOrEmpty(phoneNumber) || string.IsNullOrEmpty(password))
        {
            Debug.LogError("Please fill all fields.");
            return;
        }

        // UserModel create karo (PhoneNumber aur Password ke saath)
        UserModel loginUser = new UserModel
        {
            email = phoneNumber,
            password = password
        };

        // LoginViewModel ke function ko call karo aur response handle karo
        StartCoroutine(loginViewModel.Login(loginUser, (response) =>
        {
            // Response handle karo (success ya error dikhane ke liye)
            Debug.Log("Login Response: " + response);
        }));
    }
}
