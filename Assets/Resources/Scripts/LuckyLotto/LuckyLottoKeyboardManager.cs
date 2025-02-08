using TMPro;
using UnityEngine;

public class LuckyLottoKeyboardManager : MonoBehaviour
{
    public TMP_InputField[] inputFields; // Array to hold your InputFields
    public GameObject betArea;       // Reference to the betArea GameObject
    public GameObject BetCover;
    private Vector3 originalPosition; // Store the original position of betArea
    [SerializeField] private float keyboardOffset = 800f; // Adjust this value to move betArea up

    void Start()
    {
        // Save the original position of betArea
        if (betArea != null)
            originalPosition = betArea.transform.localPosition;

        // Add listeners to all InputFields
        foreach (TMP_InputField inputField in inputFields)
        {
            inputField.onSelect.AddListener((string text) => OnInputFieldActivated(inputField));
            inputField.onDeselect.AddListener((string text) => OnInputFieldDeactivated(inputField));
        }
    }

    private void OnInputFieldActivated(TMP_InputField inputField)
    {
        BetCover.SetActive(true);
        if (betArea != null)
        {
            // Move betArea up when the InputField is selected
            betArea.transform.localPosition = new Vector3(
                originalPosition.x,
                originalPosition.y + keyboardOffset,
                originalPosition.z
            );
        }
    }

    private void OnInputFieldDeactivated(TMP_InputField inputField)
    {
        BetCover.SetActive(false);
        if (betArea != null)
        {
            // Reset betArea position when the InputField is deselected
            betArea.transform.localPosition = originalPosition;
        }
    }
}
