using UnityEngine;

public class DiceRoller : MonoBehaviour
{
    public float dropHeight = 5f; // Height from which the dice drops
    public float rollDuration = 2f; // Total time for the dice to roll and drop
    public float gravity = 9.81f; // Simulated gravity

    private Vector3 startPosition;
    private Vector3 endPosition;
    private Quaternion startRotation;
    private Quaternion endRotation;
    private float startTime;
    private bool isRolling = false;

    void Start()
    {
        // Initialize positions
        startPosition = transform.position;
        endPosition = new Vector3(startPosition.x, 0.5f, startPosition.z); // Adjust Y to match plane height

        // Initialize rotations
        startRotation = transform.rotation;
        endRotation = GetRandomDiceRotation();

        // Start the roll
        StartRoll();
    }

    void Update()
    {
        if (isRolling)
        {
            Roll();
        }
    }

    void StartRoll()
    {
        // Record the start time
        startTime = Time.time;
        isRolling = true;
    }

    void Roll()
    {
        float timeSinceStarted = Time.time - startTime;
        float progress = timeSinceStarted / rollDuration;

        if (progress >= 1f)
        {
            // Roll is complete
            isRolling = false;
            transform.position = endPosition; // Snap to the final position
            transform.rotation = endRotation; // Snap to the final rotation
            return;
        }

        // Simulate falling motion with gravity
        float fallProgress = Mathf.Clamp01(progress * 2f); // Faster fall
        float yOffset = dropHeight * (1 - fallProgress);
        transform.position = new Vector3(startPosition.x, startPosition.y - yOffset, startPosition.z);

        // Simulate rotation
        float rotationProgress = Mathf.Clamp01(progress * 3f); // Faster rotation
        transform.rotation = Quaternion.Slerp(startRotation, endRotation, rotationProgress);
    }

    Quaternion GetRandomDiceRotation()
    {
        // Randomize the dice rotation to mimic a real dice roll
        float x = Random.Range(0, 360);
        float y = Random.Range(0, 360);
        float z = Random.Range(0, 360);
        return Quaternion.Euler(x, y, z);
    }
}