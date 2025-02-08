using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Button button;
    private Color originalColor;
    public Color hoverColor = Color.yellow;  // Choose the color for the hover effect

    void Start()
    {
        button = GetComponent<Button>();
        originalColor = button.image.color; // Store the original color of the button
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        button.image.color = hoverColor; // Change to hover color
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        button.image.color = originalColor; // Revert to original color
    }
}
