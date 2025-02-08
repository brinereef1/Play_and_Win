using System.Collections;
using UnityEngine;
using TMPro;

public class TypewriterEffect : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textComponent;
    [SerializeField] private float startDelay;
    [SerializeField] private string theText;
    [SerializeField] private int theNum;
    [SerializeField] private bool isText;
    [SerializeField] private bool isNum;
    [SerializeField] private float animDelayTime = 0.1f;
    [SerializeField] private float restartDelayTime = 0f;

    private void OnEnable()
    {
        textComponent.text = " ";

        StartCoroutine(TypeEffect());
    }

    private IEnumerator TypeEffect()
    {
        yield return new WaitForSeconds(startDelay);

        while (true)
        {
            textComponent.text = "";
            string content = !string.IsNullOrEmpty(theText) ? theText : textComponent.text;
            content = isText ? content : isNum ? theNum.ToString() : "";

            foreach (char c in content)
            {
                textComponent.text += c;
                yield return new WaitForSeconds(animDelayTime);
            }

            if (restartDelayTime > 0)
            {
                yield return new WaitForSeconds(restartDelayTime);
            }
            else
            {
                yield break;
            }
        }
    }
}