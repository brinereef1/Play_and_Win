using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class RegistrationView : MonoBehaviour
{
    public TMP_InputField usernameField;
    public TMP_InputField emailField;
    public TMP_InputField passwordField;
    public TMP_InputField confirmPasswordField;


    private RegistrationViewModel registrationViewModel;

    void Start()
    {
        // ViewModel ko reference karo
        registrationViewModel = GetComponent<RegistrationViewModel>();
    }

    // Jab Register button click ho, toh yeh function chalega
    public void OnRegisterButtonClicked()
    {
        // Form se values get karo
        string username = usernameField.text;
        
        string email = emailField.text;
        string password = passwordField.text;
        string confirmPassword = confirmPasswordField.text;

        // Input validation check kar lo
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) ||
            string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
        {
            Debug.LogError("Please fill all fields.");
            return;
        }

        if (password != confirmPassword)
        {
            Debug.LogError("Passwords do not match.");
            return;
        }

        // UserModel create karo
        UserModel newUser = new UserModel
        {
            name = username,
            email = email,
            password = password,
        };

        // RegistrationViewModel ka function call karo
        StartCoroutine(registrationViewModel.Register(newUser, (response) =>
        {
            // Response handle karo (for example, success ya error message show karo)
            Debug.Log("Registration Response: " + response);
        }));
    }
}
