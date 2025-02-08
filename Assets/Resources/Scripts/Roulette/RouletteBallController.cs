using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using Newtonsoft.Json; // Import Unity UI namespace

public class RouletteBallController : MonoBehaviour
{
    [Header("Ball Transform")]
    public Transform ball;

    [Header("Number Transform")]
    public Transform numberPlate;

    [Header("Ball Speed")]
    public float initialBallSpeed = 500f;

    [Header("TargetNumber")]
    public int targetNumber = 0;

    [Header("Spin Duration")]
    public float spinDuration = 5f;

    [Header("Elapsed Time")]
    private float elapsedTime = 0f;

    [Header("Ball Spin Speed")]
    private float ballSpeed;

    [Header("NumberPlate Spin Speed")]
    public float NumberPlateSpinSpeed;

    private bool isSpinning = false;
    private float fullSpinAngle = 720f; // Full 2 rotations before stopping

    [Header("NumberPlate Spinning")]
    public bool isNumberPlateSpinning = false;

    [Header("Ball Stop Angle")]
    public float stopAngle;

    [Header("Audio")]
    public AudioSource spinAudio;

    [Header("Angels to Stop The Ball")]
    private static Dictionary<int, float> numberToAngle = new Dictionary<int, float>
    {
        {0, -0},
        {1, -136},
        {2, -302},
        {3, -20},
        {4, -322},
        {5, -176},
        {6, -264},
        {7, -59},
        {8, -205},
        {9, -98},
        {10, -186},
        {11, -225},
        {12, -40},
        {13, -245},
        {14, -117},
        {15, -341},
        {16, -156},
        {17, -283},
        {18, -79},
        {19, -331},
        {20, -127},
        {21, -312},
        {22, -88},
        {23, -195},
        {24, -166},
        {25, -293},
        {26, -12},
        {27, -254},
        {28, -50},
        {29, -69},
        {30, -215},
        {31, -108},
        {32, -351},
        {33, -146},
        {34, -273},
        {35, -30},
        {36, -235}
    };


    [Header("Script References")]
    SuperRouletteSelectedColorWithNumber superRouletteSelectedColorWithNumber;
    void Start()
    {

        ballSpeed = initialBallSpeed;
        superRouletteSelectedColorWithNumber = FindFirstObjectByType<SuperRouletteSelectedColorWithNumber>();

        // superRouletteSelectedColorWithNumber.GetChosenNumber();

    }

    void Update()
    {

        NumberPlateSpinLogic();
        BallSpinLogic();

    }

    private void BallSpinLogic()
    {
        if (isSpinning)
        {
            ball.Rotate(Vector3.forward * ballSpeed * Time.deltaTime);
        }

    }

    private void NumberPlateSpinLogic()
    {
        if (isNumberPlateSpinning)
        {
            numberPlate.Rotate(Vector3.back * NumberPlateSpinSpeed * Time.deltaTime); // Number plate spins right-to-left
        }
    }

    // This method will be called when the spin button is pressed
    public void StartSpinTheBall()
    {
        isSpinning = true;

        // Play spinning sound
        if (spinAudio != null && !spinAudio.isPlaying)
        {
            spinAudio.Play();
        }
    }

    public void StopSpinTheBall()
    {
        StopSpinNumberPlate();
        isSpinning = false;
        SmoothStopBall();

        // Stop spinning sound
        if (spinAudio != null && spinAudio.isPlaying)
        {
            spinAudio.Stop();
        }
    }

    void SmoothStopBall()
    {
        isSpinning = false;
        Debug.Log("SmoothStopBall Called...");
        targetNumber = superRouletteSelectedColorWithNumber.getNum();
        Debug.Log(targetNumber + " target number");

        if (numberToAngle.ContainsKey(targetNumber))
        {
            float targetAngle = numberToAngle[targetNumber];

            // Instantly set the ball to the correct position
            ball.localRotation = Quaternion.Euler(-90f, 0f, targetAngle);

            Debug.Log("Ball instantly stopped at angle: " + targetAngle);

            // Show the result immediately
            StartCoroutine(superRouletteSelectedColorWithNumber.ShowResult(targetNumber.ToString()));
        }
        else
        {
            Debug.LogError("Invalid target number: " + targetNumber);
        }
    }



    IEnumerator RotateToTargetAngle(float targetAngle)
    {
        Debug.Log("RotateToTargetAngle called...");
        float currentAngle = Mathf.Repeat(ball.eulerAngles.z, 360f); // Normalize current angle to 0-360
        float totalRotationNeeded = Mathf.DeltaAngle(currentAngle, targetAngle) + 720f; // Ensure at least two full rotations
        float initialSpeed = 300f; // Initial rotation speed
        float rotationSpeed = initialSpeed; // Current rotation speed
        float decelerationStart = 90f; // Start deceleration 90 degrees before target
        float minSpeed = 30f; // Minimum speed for smooth stopping

        while (totalRotationNeeded > 0)
        {
            float remainingRotation = Mathf.Abs(totalRotationNeeded);

            // Gradually reduce speed as it approaches the target
            if (remainingRotation <= decelerationStart)
            {
                rotationSpeed = Mathf.Lerp(minSpeed, initialSpeed, remainingRotation / decelerationStart);
            }

            float step = rotationSpeed * Time.deltaTime;

            // If the remaining rotation is less than the step, stop at the exact target
            if (remainingRotation <= step)
            {
                ball.localRotation = Quaternion.Euler(-90f, 0f, targetAngle); // Snap to the exact angle
                break;
            }

            totalRotationNeeded -= step;
            currentAngle += step;

            // Apply the rotation
            ball.localRotation = Quaternion.Euler(-90f, 0f, currentAngle);

            yield return null; // Wait for the next frame
        }

        ball.localRotation = Quaternion.Euler(-90f, 0f, targetAngle); // Ensure precise stop
        Debug.Log("Ball stopped at exact angle: " + targetAngle);
        StartCoroutine(superRouletteSelectedColorWithNumber.ShowResult(targetNumber.ToString()));
    }


    public void StartSpinNumberPlate()
    {
        //Debug.Log("Start Spin");
        isNumberPlateSpinning = true;

    }

    public void StopSpinNumberPlate()
    {
        Debug.Log("Stop Spin");
        isNumberPlateSpinning = false;

        // Start the deceleration process
        StartCoroutine(StopSpinCoroutine());
    }

    private IEnumerator StopSpinCoroutine()
    {
        Quaternion targetRotation = Quaternion.Euler(270f, 0f, 0f); // Target rotation
        float rotationSpeed = 1000f; // Initial speed
        float minSpeed = 100f; // Minimum speed for smoother stop
        float stoppingTime = 2f; // Total duration for the stop
        float elapsedTime = 0f;

        while (elapsedTime < stoppingTime)
        {
            elapsedTime += Time.deltaTime;

            // Gradually reduce speed
            rotationSpeed = Mathf.Lerp(rotationSpeed, minSpeed, elapsedTime / stoppingTime);

            // Apply rotation
            numberPlate.localRotation *= Quaternion.Euler(0f, 0f, -rotationSpeed * Time.deltaTime);

            // Align to target if close enough
            if (Quaternion.Angle(numberPlate.localRotation, targetRotation) < 5f)
            {
                break;
            }

            yield return null; // Wait for the next frame
        }

        // Snap to the exact target rotation
        numberPlate.localRotation = targetRotation;
        Debug.Log("Spin Stopped at target rotation");
    }
}