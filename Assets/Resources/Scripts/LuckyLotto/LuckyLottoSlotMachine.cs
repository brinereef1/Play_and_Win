using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LuckyLottoSlotMachine : MonoBehaviour
{
    public ScrollRect[] scrollRects;
    public GameObject[] contentObjects;
    public GameObject[] slotItems;
    public Button spinButton;
    public Button stopButton;
    [SerializeField] public List<string> newList = new List<string>();
    LuckyLottoResultManger resultManger;
    private bool isSpinning = false;

    private readonly float[] positions = {
        0f, 588f, 1176f, 1764f, 2354f, 2942f, 3530f, 4118f, 4706f, 5294f,
        5882f, 6470f, 7078f, 7666f, 8254f, 8846f, 9438f, 10024f, 10614f,
        11202f, 11790f, 12378f, 12976f, 13568f, 14156f, 14744f, 15340f,
        15928f, 16516f, 17104f, 17692f, 18280f, 18880f, 19464f, 20056f,
        20644f, 21234f, 21824f, 22414f, 23004f, 23594f, 24184f, 24774f,
        25364f, 25954f, 26544f, 27134f, 27728f, 28318f, 28908f, 29498f, 30090f
    };

    public float spinDuration = 2f;
    public float scrollSpeed = 0.2f;

    private string[] selectedCards = new string[3];

    [Header("Audio")]
    public AudioSource spinAudio;
    void Start()
    {
        resultManger = FindFirstObjectByType<LuckyLottoResultManger>();
        spinButton.onClick.AddListener(StartSpinning);
        stopButton.onClick.AddListener(StopSpinning);
        // resultManger.GetChosenNumber();
        //  StartCoroutine(resultManger.ShowResult(newList));

        if (spinAudio != null)
        {
            spinAudio.loop = true; // Loop the sound while spinning
        }
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
        isSpinning = true;

        // Start playing the rolling sound
        if (spinAudio != null && !spinAudio.isPlaying)
        {
            spinAudio.Play();
        }
    }

    public void StopSpinning()
    {
        isSpinning = false;
        // resultManger.GetChosenNumber(); 

        if (spinAudio != null && spinAudio.isPlaying)
        {
            spinAudio.Stop();
        }

        if (newList.Count == 3)
        {
            for (int i = 0; i < 3; i++)
            {
                selectedCards[i] = newList[i]; // Store the selected cards
                // Debug.Log("Selected cards: " + selectedCards[i]);
            }
        }
        for (int i = 0; i < 3; i++)
        {
            int cardIndex = GetCardIndexFromName(selectedCards[i], i); // Find the matching card's index
            Debug.Log(cardIndex);
            if (cardIndex != -1) // Check if a valid index is returned
            {
                StopReel(i, cardIndex); // Stop the reel at the matched index
            }
            else
            {
                Debug.LogError($"Card '{selectedCards[i]}' not found in content objects.");
            }
        }
        StartCoroutine(resultManger.ShowResult());

    }

    void StopReel(int slotIndex, int selectedIndex)
    {
        if (selectedIndex < 0 || selectedIndex >= positions.Length)
        {
            Debug.LogError("Invalid slot item selected.");
            return;
        }

        float targetYPosition = positions[selectedIndex];

        Debug.Log($"Slot {slotIndex + 1} - Selected Index: {selectedIndex}, Target Y Position: {targetYPosition}");

        StartCoroutine(SmoothScrollToYPosition(slotIndex, targetYPosition));
    }

    IEnumerator SmoothScrollToYPosition(int slotIndex, float targetYPosition)
    {
        RectTransform contentRect = contentObjects[slotIndex].GetComponent<RectTransform>();
        float currentY = contentRect.anchoredPosition.y;
        float elapsedTime = 0f;
        float duration = 0.1f; // changes made here

        while (elapsedTime < duration)
        {
            float newY = Mathf.Lerp(currentY, targetYPosition, elapsedTime / duration);
            contentRect.anchoredPosition = new Vector2(contentRect.anchoredPosition.x, newY);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        contentRect.anchoredPosition = new Vector2(contentRect.anchoredPosition.x, targetYPosition);
     

    }

    int GetCardIndexFromName(string cardName, int slotIndex)
    {
        Debug.Log(cardName);
        foreach (var item in contentObjects)
        {
            // Get the index of the current slot (item)
            int Index = Array.IndexOf(contentObjects, item);
            // Debug.Log("Slot " + slotIndex + " - Checking child game objects:");
            // Loop through the child game objects of each content item (slot)

            if (Index == slotIndex)
            {
                int count = 0;
                foreach (Transform child in item.transform)
                {

                    if (child.gameObject.name == cardName)
                    {
                        // Debug.Log("Matched card index" + count);
                        return count;
                    }
                    count++;
                }
            }

        }
        return -1; // Return -1 if no match is found
    }
}
