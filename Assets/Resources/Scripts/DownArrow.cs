using UnityEngine;
using System.Collections;

public class NeedleAnimator : MonoBehaviour
{
    public float startAngle = -30f;  // Starting angle of the needle
    public float endAngle = 30f;     // Ending angle of the needle
    public float animationDuration = 2f; // Duration of the back-and-forth animation

    private RectTransform rectTransform;
   // private bool isAnimating = false;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        StartCoroutine(AnimateNeedle());
    }

    private IEnumerator AnimateNeedle()
    {
        while (true)
        {
            yield return StartCoroutine(RotateNeedle(startAngle, endAngle));
            yield return StartCoroutine(RotateNeedle(endAngle, startAngle));
        }
    }

    private IEnumerator RotateNeedle(float fromAngle, float toAngle)
    {
        //isAnimating = true;
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            float angle = Mathf.Lerp(fromAngle, toAngle, elapsedTime / animationDuration);
            rectTransform.localRotation = Quaternion.Euler(0f, 0f, angle);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rectTransform.localRotation = Quaternion.Euler(0f, 0f, toAngle);
        //isAnimating = false;
    }
}
