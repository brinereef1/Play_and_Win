using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting;
public class SpinTheWheelSpinnerController : MonoBehaviour
{

    public GameObject wheel;

    [Header("Spinner Varibales")]
    public float spinSpeed = 300f;   // Current speed of the spin    
    public float numberOfSegments = 12;

    public float segmentAngle;

    public string targetColor;
    public float targetAngle;

    public bool isSpinning = false;

    private string AuthTok;

    [Header("Referenced Scripts")]
    SaveUserData svd = new SaveUserData();

    [Header("Audio")]
    [SerializeField] AudioSource spinSfx;

    SpinTheWheelResultManager spinTheWheelResultManager;

    public string chosenCard = "";
    void Start()
    {
        AuthTok = svd.GetSavedAuthToken();
        spinTheWheelResultManager = FindFirstObjectByType<SpinTheWheelResultManager>();
    }


    void Update()
    {
        if (isSpinning)
        {
            SpinLogic();
        }
    }

    public void SpinLogic()
    {
        isSpinning = true;
        if (spinSfx != null && !spinSfx.isPlaying)
        {
            spinSfx.Play();
        }
        float rotationAmount = spinSpeed * Time.deltaTime; 
        wheel.transform.Rotate(0, 0, rotationAmount);
    }


    public void EndSpin()
    {
        isSpinning = false;
        if (spinSfx != null && !spinSfx.isPlaying)
        {
            spinSfx.Stop();
        }
        AlignWheelToSegment(targetColor);
    }

    public void SetToken(string token)
    {
        AuthTok = token;
    }

    public string GetToken()
    {
        return AuthTok;
    }
    // Club , Crown , Spade , Diamond , Flag , Heart
    private void AlignWheelToSegment(string card)
    {
        Debug.Log("Target Color: " + card);
        if (!string.IsNullOrEmpty(card))
        {
            switch (card)
            {
                case "White":
                    targetAngle = 0;
                    break;
                case "Black":
                    targetAngle = 116;
                    break;
                case "Red":
                    targetAngle = 90;
                    break;                
                default:
                    Debug.LogWarning("Unknown card: " + card);
                    return;
            }


            // Rotate the wheel to align with the segment center
            Debug.Log("Aligning wheel to angle: " + targetAngle);
            wheel.transform.rotation = Quaternion.Euler(0, 0, targetAngle);
            StartCoroutine(spinTheWheelResultManager.ShowResult(targetColor));
            Debug.Log("After Aligning wheel to angle: " + targetColor);
        }

    }

    public void BackFromSpinTheWheel()
    {
        SceneManager.LoadScene("Home");
    }
}