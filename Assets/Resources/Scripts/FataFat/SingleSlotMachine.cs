using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SingleSlotMachine : MonoBehaviour
{
    public ScrollRect scrollRect;
    public GameObject content;
    public GameObject[] slotItems;
    public Button spinButton;
    public Button stopButton;
    public int targetNumber;
    private bool isSpinning = false;
    SingleResultManager singleResultManager;

    private readonly float[] positions = { 200f, 600f, 1000f, 1400f, 1800f, 2200f, 2600f, 3000f, 3400f, 3800f };

    public float spinDuration = 2f;
    public float scrollSpeed = 0.2f;

    void Start()
    {
        singleResultManager = FindFirstObjectByType<SingleResultManager>();
        spinButton.onClick.AddListener(StartSpinning);
        stopButton.onClick.AddListener(StopSpinning);
    }
    void Update()
    {
        if (isSpinning)
        {

            scrollRect.verticalNormalizedPosition -= scrollSpeed * Time.deltaTime;


            if (scrollRect.verticalNormalizedPosition <= 0f)
            {
                scrollRect.verticalNormalizedPosition = 1f;
            }
        }
    }

    public void StartSpinning()
    {
        isSpinning = true;
    }


    public void StopSpinning()
    {
        isSpinning = false;
        StopReel(targetNumber);
    }

    void StopReel(int selectedIndex)
    {
        if (selectedIndex < 0 || selectedIndex >= positions.Length)
        {
            Debug.LogError("Invalid slot item selected.");
            return;
        }


        float targetYPosition = positions[selectedIndex];

        Debug.Log($"Selected Index: {selectedIndex}, Target Y Position: {targetYPosition}");

        StartCoroutine(SmoothScrollToYPosition(targetYPosition));
    }

    IEnumerator SmoothScrollToYPosition(float targetYPosition)
    {
        RectTransform contentRect = content.GetComponent<RectTransform>();
        float currentY = contentRect.anchoredPosition.y;
        float elapsedTime = 0f;
        float duration = 0.5f;

        while (elapsedTime < duration)
        {

            float newY = Mathf.Lerp(currentY, targetYPosition, elapsedTime / duration);
            contentRect.anchoredPosition = new Vector2(contentRect.anchoredPosition.x, newY);

            elapsedTime += Time.deltaTime;
            yield return null;
        }


        contentRect.anchoredPosition = new Vector2(contentRect.anchoredPosition.x, targetYPosition);
        StartCoroutine(singleResultManager.ShowResult(targetNumber.ToString()));

    }
}
