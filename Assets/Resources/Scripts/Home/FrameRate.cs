using TMPro;
using UnityEngine;

public class FrameRate : MonoBehaviour
{

    public TMP_Text fpsText; // Assign a Text UI element in the inspector
    public float deltaTime = 0.0f;

    void Update()
    {
        // Calculate the frame time
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        // Calculate frames per second
        float fps = 1.0f / deltaTime;

        // Display the frame rate
        fpsText.text = string.Format("{0:0.} FPS", fps);
    }



}
