using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using TMPro;
public class DiceAnimation : MonoBehaviour
{
    [Header("Dices")]
    [SerializeField] GameObject left_dice;
    [SerializeField] GameObject right_dice;
    [Header("Dice Animation")]
    [SerializeField] ParticleSystem leftDiceEffect; // Reference to the left dice particle effect
    [SerializeField] ParticleSystem rightDiceEffect; // Reference to the right dice particle effect
    //[SerializeField] GameObject Lucky7Panel;
    [Header("Dice Animation Variables")]
    public float rollDuration = 2f;
    public float minRotationSpeed = 360f; // Minimum speed of rotation
    public float maxRotationSpeed = 720f; // Maximum speed of rotation
    public float predictionInterval = 0.5f; // Interval for changing prediction
    [Header("Dice Rolling or Not")]
    private bool isRolling = false;
    [Header("Scripts")]
    GetTime getTime;
    public int num1, num2;
    private Dictionary<int, Quaternion> faceRotations = new Dictionary<int, Quaternion>()
    {
        { 1, Quaternion.Euler(0, 0, 0) },
        { 2, Quaternion.Euler(0, 90, 0) },
        { 3, Quaternion.Euler(-90, 0, 0) },
        { 4, Quaternion.Euler(90, 0, 0) },
        { 5, Quaternion.Euler(0, 270, 0) },
        { 6, Quaternion.Euler(180, 0, 0) }
    };

    [Header("Audio Variables")]
    [SerializeField] AudioSource diceRollAudio;
    [SerializeField] AudioSource resultAudio;

    public string[] result;

    IsWinnerManager isWinnerManager;

    public GameObject show_ResultPanel;
    public TMP_Text show_NumberText;
    LastTenHistoryManager lastTenHistoryManager = new LastTenHistoryManager();

    void Start()
    {
        isWinnerManager = FindFirstObjectByType<IsWinnerManager>();
        getTime = FindFirstObjectByType<GetTime>();
        lastTenHistoryManager = FindFirstObjectByType<LastTenHistoryManager>();

    }

    public void RollDiceButton(float serverTime)
    {
        if (!isRolling)
        {
            rollDuration = serverTime; // Set roll duration from the server time
            StartCoroutine(RollDice());
        }
    }

    IEnumerator RollDice()
    {
        isRolling = true;

        // Play particle effects
        if (leftDiceEffect != null)
        {
            leftDiceEffect.Play();
        }
        if (rightDiceEffect != null)
        {
            rightDiceEffect.Play();
        }

        float elapsedTime = 0f;
        float nextPredictionTime = predictionInterval;

        // Randomize rotation speeds
        Vector3 randomRotationSpeed1 = new Vector3(
            Random.Range(minRotationSpeed, maxRotationSpeed),
            Random.Range(minRotationSpeed, maxRotationSpeed),
            Random.Range(minRotationSpeed, maxRotationSpeed)
        );

        Vector3 randomRotationSpeed2 = new Vector3(
            Random.Range(minRotationSpeed, maxRotationSpeed),
            Random.Range(minRotationSpeed, maxRotationSpeed),
            Random.Range(minRotationSpeed, maxRotationSpeed)
        );

        Quaternion finalRotation1;
        Quaternion finalRotation2;

        while (elapsedTime < rollDuration)
        {
            elapsedTime += Time.deltaTime;

            // Change prediction result at intervals
            if (elapsedTime >= nextPredictionTime)
            {
                diceRollAudio.Play(); // Play dice roll sound
                nextPredictionTime += predictionInterval;

                // Generate new random rotations
                finalRotation1 = faceRotations[Random.Range(1, 7)];
                finalRotation2 = faceRotations[Random.Range(1, 7)];

                // Apply rotation speeds to simulate rolling
                left_dice.transform.Rotate(randomRotationSpeed1 * Time.deltaTime);
                right_dice.transform.Rotate(randomRotationSpeed2 * Time.deltaTime);

                // Snap to a new predicted result rotation smoothly
                left_dice.transform.DORotateQuaternion(finalRotation1, predictionInterval * 0.5f).SetEase(Ease.InOutQuad);
                right_dice.transform.DORotateQuaternion(finalRotation2, predictionInterval * 0.5f).SetEase(Ease.InOutQuad);
            }

            yield return null;
        }

        
    }


    public void EndRoll()
    {
        isRolling = false;

    }



    public void AlignTheDices(int num1, int num2)
    {
        Debug.Log("Aligning the dices...");
        Debug.Log("Result: " + num1 + ", " + num2);
        Quaternion finalRotation1;
        Quaternion finalRotation2;

        finalRotation1 = faceRotations[num1];
        finalRotation2 = faceRotations[num2];
        left_dice.transform.DORotateQuaternion(finalRotation1, predictionInterval * 0.5f).SetEase(Ease.InOutQuad);
        right_dice.transform.DORotateQuaternion(finalRotation2, predictionInterval * 0.5f).SetEase(Ease.InOutQuad);
        EndRoll();

        resultAudio.Play();
        // Stop particle effects
        if (leftDiceEffect != null)
        {
            leftDiceEffect.Stop();
        }
        if (rightDiceEffect != null)
        {
            rightDiceEffect.Stop();
        }

        StartCoroutine(DisplayResultAndWinner(num1, num2));


    }

    IEnumerator DisplayResultAndWinner(int num1, int num2)
    {
        int value = num1 + num2;
        yield return new WaitForSeconds(3f);
        StartCoroutine(ChosenNumber(value));
       yield return new WaitForSeconds(2f);

        if (lastTenHistoryManager != null)
        {
            lastTenHistoryManager.LastTenWinHistoryButtonClick();
        }
        else
        {
            Debug.Log("Last Ten Win History Manager is Null");
        }



        if (isWinnerManager != null)
        {
            StartCoroutine(isWinnerManager.VictorButtonClick());
        }
        else
        {
            Debug.Log("Win History Manager or IsWinnerManager is Null");
        }
    }


    public IEnumerator ChosenNumber(int finalValue){
        show_ResultPanel.gameObject.SetActive(true);
        show_NumberText.text = finalValue.ToString();
        yield return new WaitForSeconds(2f);
        show_ResultPanel.gameObject.SetActive(false);
        show_NumberText.text = "";
    }


    public void BackToHomeFromLucky7()
    {
        Debug.Log("Going to Home from Lucky7");
        SceneManager.LoadScene("Home");
    }
}
