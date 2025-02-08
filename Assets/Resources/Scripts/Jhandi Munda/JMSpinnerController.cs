using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JMSpinnerController : MonoBehaviour
{
    [Header("Dice")]
    [SerializeField] GameObject dice;

    [Header("Dice Animation Variables")]
    public float rollDuration = 3f;  // Total roll time
    public float faceChangeDuration = 0.5f;  // Duration for each face to transition

    [Header("Dice Rolling or Not")]
    public bool isRolling = false;
    public string targetColor;
    private Dictionary<int, Quaternion> faceRotations = new Dictionary<int, Quaternion>()
    {
        { 1, Quaternion.Euler(0, 0, 0) }, // Crown
        { 2, Quaternion.Euler(0, 90, 0) }, // Club
        { 3, Quaternion.Euler(-90, 0, 0) }, // Spade
        { 4, Quaternion.Euler(90, 0, 0) }, // Heart
        { 5, Quaternion.Euler(0, 270, 0)}, // Diamond
        { 6, Quaternion.Euler(180, 0, 0) } // Flag
    };

    private int currentFace = 1;  // Start with face 1
    private float elapsedTime = 0f;

    PokerSlotIsWinnerManager pokerSlotIsWinnerManager;
    JMResultManager jMResultManager;

    [Header("Audio")]
    public AudioSource rollAudio;

    void Update()
    {
        // If the dice is rolling, we cycle through facess
        if (isRolling)
        {
            elapsedTime += Time.deltaTime;

            // If enough time has passed for a face change
            if (elapsedTime >= faceChangeDuration)
            {
                elapsedTime = 0f;
                currentFace = (currentFace % 6) + 1;  // Cycle through faces 1 to 6


                // Apply the new face rotation
                dice.transform.DORotateQuaternion(faceRotations[currentFace], faceChangeDuration).SetEase(Ease.InOutQuad);
            }
        }
    }

    void Start()
    {
       
        pokerSlotIsWinnerManager = FindFirstObjectByType<PokerSlotIsWinnerManager>();
        jMResultManager = FindFirstObjectByType<JMResultManager>();

        // Ensure audio settings
        if (rollAudio != null)
        {
            rollAudio.loop = true; // Loop the sound while rolling
        }

    }

    // Call to start the roll animation
    public void StartRoll()
    {
        if (!isRolling)
        {
            isRolling = true;
            elapsedTime = 0f;

            // Start looping sound when rolling starts
            if (rollAudio != null && !rollAudio.isPlaying)
            {
                rollAudio.Play();
            }
        }
    }

    // Call to stop the roll animation
    public void StopRoll()
    {
        isRolling = false;
        StartCoroutine(AlignTheDice());
        if (rollAudio != null && rollAudio.isPlaying)
        {
            rollAudio.Stop();
        }
    }

    // Align the dice to a specific face (you can define a final face to stop on)
     IEnumerator AlignTheDice()
    {
        int targetFace;
        switch (targetColor)
        {
            case "Crown":
                targetFace = 1;
                break;
            case "Club":
                targetFace = 2;
                break;
            case "Spade":
                targetFace = 3;
                break;
            case "Heart":
                targetFace = 4;
                break;
            case "Diamond":
                targetFace = 5;
                break;
            case "Flag":
                targetFace = 6;
                break;
            default:
                targetFace = 0;
                break;
        }
        Quaternion finalRotation = faceRotations[targetFace];
        // You can decide to stop at a specific face here (optional)
        dice.transform.DORotateQuaternion(finalRotation, 0.5f).SetEase(Ease.OutQuad);
        yield return new WaitForSeconds(.5f);
        StartCoroutine(jMResultManager.ShowResult(targetColor));
    }

    // Example to go back to the main menu or another scene
    public void BackFromPokerSlot()
    {
        SceneManager.LoadScene("Home");
    }
}
