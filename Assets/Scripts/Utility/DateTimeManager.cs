using UnityEngine;
using TMPro;
using System.Collections;

public class DateTimeManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI dateText; 
    public float typingSpeed = 0.1f;

    private void Start()
    {
        if (dateText != null)
            StartCoroutine(TypeText(System.DateTime.Now.ToString("dd/MM/yyyy")));
    }

    IEnumerator TypeText(string text)
    {
        dateText.text = "";
        foreach (char letter in text)
        {
            dateText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
