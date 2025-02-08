using UnityEngine;

public class KeyboardManager : MonoBehaviour
{
    public RectTransform panelToMove; 
    public float moveOffset = 200f; 
    private Vector2 originalPosition;

    void Start()
    {
        originalPosition = panelToMove.anchoredPosition;
    }



    void Update()
    {
        if (TouchScreenKeyboard.visible)
        {
           
            panelToMove.anchoredPosition = new Vector2(originalPosition.x, originalPosition.y + moveOffset);
        }
        else
        {
            
        }
    }
}
