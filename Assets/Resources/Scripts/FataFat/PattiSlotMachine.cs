using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PattiSlotMachine : MonoBehaviour
{
    public ScrollRect[] scrollRects;
    public GameObject[] contentObjects;
    public GameObject[] slotItems;
    public Button spinButton;
    public Button stopButton;
    public string targetNumber;
    private bool isSpinning = false;

    private readonly float[] positions = { 0f, 200f, 405f, 610f, 815f, 1020f, 1225f, 1430f, 1640f, 1840f };

    public float spinDuration = 2f;
    public float scrollSpeed = 0.2f;

    private int[] targetNumbers = new int[3];
    PattiResultManager pattiResultManager;

    void Start()
    {
        pattiResultManager = FindFirstObjectByType<PattiResultManager>();
        spinButton.onClick.AddListener(StartSpinning);
        stopButton.onClick.AddListener(StopSpinning);

    }

    void Update()
    {
        if (isSpinning)
        {

            for (int i = 0; i < scrollRects.Length; i++)
            {
                scrollRects[i].verticalNormalizedPosition -= scrollSpeed * Time.deltaTime;


                if (scrollRects[i].verticalNormalizedPosition <= 0f)
                {
                    scrollRects[i].verticalNormalizedPosition = 1f;
                }
            }
        }
    }

    public void StartSpinning()
    {

        string input = targetNumber.ToString(); ;
        if (input.Length == 3)
        {
            for (int i = 0; i < 3; i++)
            {
                targetNumbers[i] = int.Parse(input[i].ToString());
            }
            isSpinning = true;
        }
        else
        {
            Debug.LogError("Please enter a valid 3-digit number.");
        }
    }

    public void StopSpinning()
    {
        isSpinning = false;

        for (int i = 0; i < 3; i++)
        {
            StopReel(i, targetNumbers[i]);
        }
        StartCoroutine(pattiResultManager.ShowResult(targetNumber.ToString()));

    }

    void StopReel(int slotIndex, int selectedNumber)
    {
        if (selectedNumber < 0 || selectedNumber >= positions.Length)
        {
            Debug.LogError("Invalid slot item selected.");
            return;
        }


        float targetYPosition = positions[selectedNumber];

        Debug.Log($"Slot {slotIndex + 1} - Selected Number: {selectedNumber}, Target Y Position: {targetYPosition}");


        StartCoroutine(SmoothScrollToYPosition(slotIndex, targetYPosition));
    }

    IEnumerator SmoothScrollToYPosition(int slotIndex, float targetYPosition)
    {
        RectTransform contentRect = contentObjects[slotIndex].GetComponent<RectTransform>();
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
        yield return new WaitForSeconds(3f);

    }
}
