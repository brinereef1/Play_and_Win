using UnityEngine;

public class RouletteBallSettle : MonoBehaviour
{
    public Transform[] numberPositions;  // Array of transforms representing each number's position
    public float settleSpeed = 5f;       // Speed at which the ball settles on a number

    private Transform targetPosition;    // The position where the ball should settle
    private bool isSettling = false;     // Whether the ball is in the process of settling

    void Update()
    {
        if (isSettling)
        {
            // Move the ball towards the target position
            transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, settleSpeed * Time.deltaTime);

            // Check if the ball has reached the target position
            if (Vector3.Distance(transform.position, targetPosition.position) < 0.01f)
            {
                isSettling = false; // Stop the settling animation
            }
        }
    }

    // Call this method to start the settling animation
    public void SettleOnNumber(int numberIndex)
    {
        if (numberIndex < 0 || numberIndex >= numberPositions.Length)
        {
            Debug.LogError("Invalid number index passed to SettleOnNumber!");
            return;
        }

        // Set the target position based on the number index
        targetPosition = numberPositions[numberIndex];
        isSettling = true;
    }
}
