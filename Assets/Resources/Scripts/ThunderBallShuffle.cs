using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThunderBallShuffle : MonoBehaviour
{
    public List<GameObject> balls;
    public Button shuffleButton;
    private System.Random random = new System.Random();
    private Vector3[] originalPositions;
    [SerializeField] private float shuffleDuration = 10f; // Total shuffle duration
    private Vector3 defaultScale = new Vector3(2000, 2000, 2000); // Default scale of the balls

    void Start()
    {
        shuffleButton.onClick.AddListener(() => StartCoroutine(ShuffleBalls()));

        // Store the original positions of the balls
        originalPositions = new Vector3[balls.Count];
        for (int i = 0; i < balls.Count; i++)
        {
            originalPositions[i] = balls[i].transform.position;
        }
    }

    IEnumerator ShuffleBalls()
    {
        // Reset all balls to their original scale before starting the shuffle
        ResetBallSizes();

        float endTime = Time.time + shuffleDuration; // Set end time for the shuffle
        while (Time.time < endTime)
        {
            foreach (GameObject ball in balls)
            {
                Vector3 randomOffset = new Vector3(random.Next(-1, 2), random.Next(-1, 2), 0);
                Vector3 targetPosition = originalPositions[balls.IndexOf(ball)] + randomOffset;
                StartCoroutine(MoveBall(ball, ball.transform.position, targetPosition, 0.1f));
            }
            yield return new WaitForSeconds(0.1f); // Shuffle interval
        }

        // After shuffling, highlight the selected ball
        GameObject finalSelectedBall = balls[random.Next(balls.Count)];
        StartCoroutine(HighlightBall(finalSelectedBall));
    }

    IEnumerator MoveBall(GameObject ball, Vector3 from, Vector3 to, float duration)
    {
        float elapsed = 0;
        while (elapsed < duration)
        {
            ball.transform.position = Vector3.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        ball.transform.position = to;
    }

    IEnumerator HighlightBall(GameObject ball)
    {
        float duration = 0.5f;
        float elapsed = 0;
        Vector3 originalScale = ball.transform.localScale;
        Vector3 targetScale = originalScale * 1.5f;

        // Scale up the selected ball
        while (elapsed < duration)
        {
            Debug.Log("Ball is scaling...");
            ball.transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        ball.transform.localScale = targetScale;

        // Move the ball to the front
        elapsed = 0;
        Vector3 originalPosition = ball.transform.position;
        Vector3 targetPosition = new Vector3(originalPosition.x, originalPosition.y, originalPosition.z - 2);

        while (elapsed < duration)
        {
            ball.transform.position = Vector3.Lerp(originalPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        ball.transform.position = targetPosition;
    }

    void ResetBallSizes()
    {
        foreach (GameObject ball in balls)
        {
            if (ball.transform.localScale != defaultScale)
            {
                StartCoroutine(MoveBall(ball, ball.transform.position, ball.transform.position, 0.1f)); // Reset position if necessary
                StartCoroutine(ResetScale(ball));
            }
        }
    }

    IEnumerator ResetScale(GameObject ball)
    {
        float duration = 0.5f;
        Vector3 originalScale = ball.transform.localScale;
        Vector3 targetScale = defaultScale;

        float elapsed = 0;
        while (elapsed < duration)
        {
            ball.transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        ball.transform.localScale = targetScale;
    }
}
