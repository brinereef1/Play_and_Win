using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PowerBallSpin : MonoBehaviour
{
    [Header("Game Elements")]
    public GameObject[] balls;
    public GameObject Parent_rod;
    public GameObject rod;
    public GameObject newParent;

    [Header("Force Settings")]
    public float ForceToRight = 20f;

    [Header("Rod Settings")]
    public float rodGrowDuration = 3f;
    public float rodShrinkDuration = 3f;
    public Vector3 rodFinalScale = new Vector3(1, 10, 1);
    public Vector3 ballPositionOnRod = new Vector3(0, 5, 0);
    public Vector3 ballSizeOnRod = new Vector3(1, 1, 1);
    public float finalZPosition = 0.19f;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource rodGrowSound;
    [SerializeField] private AudioSource rodShrinkSound;


    private GameObject attachedBall;
    private Vector3 originalBallPosition;
    private Transform originalBallParent;


    [SerializeField] GameObject ThunderBall_Game;
    [SerializeField] public int TargetBallNumber;

    PowerBallSelectedBall powerBallSelectedBall;
    void Start()
    {
        powerBallSelectedBall = FindFirstObjectByType<PowerBallSelectedBall>();
         //StartApplyingForce();

    }

    public void StartApplyingForce()
    {
        StartCoroutine(ApplyForceToBalls());
    }

    IEnumerator ApplyForceToBalls()
    {
        attachedBall = balls[TargetBallNumber];
        if (attachedBall != null)
        {
            AttachBallToRod(attachedBall);

            yield return new WaitForSeconds(0f);
            StartCoroutine(GrowRod());
        }
    }



    IEnumerator GrowRod()
    {
        rodGrowSound.Play();
        Vector3 initialScale = Parent_rod.transform.localScale;
        Vector3 scaleChange = rodFinalScale - initialScale;

        for (float elapsedTime = 0f; elapsedTime < rodGrowDuration; elapsedTime += Time.deltaTime)
        {
            Parent_rod.transform.localScale = initialScale + (scaleChange * (elapsedTime / rodGrowDuration));
            UpdateBallPositionOnRod();
            yield return null;
        }

        Parent_rod.transform.localScale = rodFinalScale;
        rodGrowSound.Stop();
        FinalizeBallPosition();

        yield return new WaitForSeconds(5f);

        Rigidbody rb = attachedBall.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        StartCoroutine(ShrinkRod(initialScale));
        // ResetBallPosition();
        StartCoroutine(powerBallSelectedBall.ShowResult(TargetBallNumber.ToString()));

    }

    void AttachBallToRod(GameObject ball)
    {
        originalBallPosition = ball.transform.position;
        originalBallParent = ball.transform.parent;

        ball.transform.SetParent(null);
        ball.transform.position = rod.transform.TransformPoint(ballPositionOnRod);
        ball.transform.localScale = ballSizeOnRod;
        ball.transform.SetParent(rod.transform);
        ball.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);

        Rigidbody rb = ball.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }

    void UpdateBallPositionOnRod()
    {
        if (attachedBall != null)
        {
            attachedBall.transform.localScale = ballSizeOnRod;
            float relativeZ = finalZPosition * (rodFinalScale.y / Parent_rod.transform.localScale.y);
            attachedBall.transform.localPosition = new Vector3(
                ballPositionOnRod.x,
                ballPositionOnRod.y,
                Mathf.Clamp(relativeZ, 0, finalZPosition)
            );
        }
    }

    void FinalizeBallPosition()
    {
        if (attachedBall != null)
        {
            attachedBall.transform.localPosition = new Vector3(
                ballPositionOnRod.x,
                ballPositionOnRod.y,
                finalZPosition
            );

            Rigidbody rb = attachedBall.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.AddForce(Vector3.right * ForceToRight, ForceMode.Impulse);
            }

            attachedBall.transform.SetParent(newParent.transform);
            attachedBall.transform.localScale = new Vector3(1800, 1800, 1800);
        }
    }

    IEnumerator ShrinkRod(Vector3 initialScale)
    {
        rodShrinkSound.Play();

        for (float elapsedTime = 0f; elapsedTime < rodShrinkDuration; elapsedTime += Time.deltaTime)
        {
            Parent_rod.transform.localScale = Vector3.Lerp(rodFinalScale, initialScale, elapsedTime / rodShrinkDuration);
            yield return null;
        }

        Parent_rod.transform.localScale = initialScale;
        rodShrinkSound.Stop();
        yield return new WaitForSeconds(2f);
    }

    public void ResetBallPosition()
    {
        if (attachedBall != null)
        {
            attachedBall.transform.SetParent(originalBallParent);
            attachedBall.transform.position = originalBallPosition;
            attachedBall.transform.localScale = new Vector3(1500, 1500, 1500);

            Rigidbody rb = attachedBall.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
            }

            // Debug.Log("Ball " + attachedBall.name + " reset to original position " + originalBallPosition + " with size 1000, 1000, 1000");
        }

    }

    public void BackFromPowerBall()
    {
        SceneManager.LoadScene("Home");
    }
}
