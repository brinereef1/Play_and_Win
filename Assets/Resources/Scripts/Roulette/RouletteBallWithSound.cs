using UnityEngine;

public class RouletteBallWithSound : MonoBehaviour
{
    public float spinSpeed = 500f;          // Adjust the speed of the spin
    public float deceleration = 50f;        // Rate at which the wheel slows down
    public float spinDuration = 3f;         // Duration for which the wheel spins at full speed
    public AudioSource spinSound;           // Reference to the AudioSource component

    private bool isSpinning = false;
    private float currentSpeed;
    private float spinTime;

    void Update()
    {
        if (isSpinning)
        {
            // Spin the roulette wheel in the opposite direction
            transform.Rotate(0, 0, -currentSpeed * Time.deltaTime);

            // Track the spin time
            spinTime += Time.deltaTime;

            if (spinTime >= spinDuration)
            {
                // After the spin duration, start deceleration
                currentSpeed -= deceleration * Time.deltaTime;

                // Stop the spin and the sound when the speed is very low
                if (currentSpeed <= 0)
                {
                    isSpinning = false;
                    currentSpeed = 0;
                    spinSound.Stop();  // Stop the spinning sound
                }
            }
        }
    }

    // Call this method to start the spin
    public void StartSpin()
    {
        isSpinning = true;
        currentSpeed = spinSpeed;
        spinTime = 0f;  // Reset the spin time

        // Play the spinning sound
        if (spinSound != null)
        {
            spinSound.loop = true;  // Loop the sound while spinning
            spinSound.Play();
        }
    }
}
