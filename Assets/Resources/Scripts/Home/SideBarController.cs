using UnityEngine;
using UnityEngine.UI;

public class SideBarController : MonoBehaviour
{
    public RectTransform slideBar;
    public Image overlay;
    public float slideDuration = 0.5f;
    public float targetPositionX = 500f;

    [SerializeField] private Animator sideBarWheel;

    private Vector2 initialPosition;
    private Vector2 slideOutPosition;
    private bool isSliding = false;
    private bool isSlidingOut = false;
    private float slideStartTime;

    void Start()
    {
        if (slideBar == null || overlay == null)
        {
           // Debug.LogError("SlideBar or Overlay not assigned!");
            return;
        }

        initialPosition = slideBar.anchoredPosition;
        slideOutPosition = new Vector2(targetPositionX, initialPosition.y);
        overlay.gameObject.SetActive(false);

       // Debug.Log("Initialization complete. Initial Position: " + initialPosition + ", Target Position: " + slideOutPosition);
    }

    void Update()
    {
        if (isSliding)
        {
            float t = (Time.time - slideStartTime) / slideDuration;
            if (t >= 1.0f)
            {
                t = 1.0f;
                isSliding = false;
                overlay.gameObject.SetActive(isSlidingOut);
            }

            slideBar.anchoredPosition = isSlidingOut ? Vector2.Lerp(initialPosition, slideOutPosition, t) : Vector2.Lerp(slideOutPosition, initialPosition, t);

            //Debug.Log("Sliding: " + slideBar.anchoredPosition + ", t: " + t);
        }
    }

    public void StartSlide()
    {
        //Debug.Log("Slide Bar Started Sliding...");
        if (!isSliding && !isSlidingOut)
        {
            isSliding = true;
            isSlidingOut = true;
            slideStartTime = Time.time;
            sideBarWheel.SetTrigger("clicked");
            //Debug.Log("Start Slide Out");
        }
    }

    public void HideSlideBar()
    {
        if (!isSliding && isSlidingOut)
        {
            isSliding = true;
            isSlidingOut = false;
            slideStartTime = Time.time;
            sideBarWheel.SetTrigger("backClicked");
            // Debug.Log("Start Slide In");
        }
    }
}
