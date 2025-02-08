using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting;
public class SpinnerWheel : MonoBehaviour
{

    public GameObject wheel;

    [Header("Spinner Varibales")]
    public float spinSpeed = 300f;   // Current speed of the spin
    public float originalNeedleRotation;
    public float numberOfSegments = 12;
    public float segmentAngle;
    public string targetColor;
    public float targetAngle;
    public RectTransform rectTransform;
    public bool isSpinning = false;
    [SerializeField] AudioSource spinSound;
    [SerializeField] GameObject PokerSlots_Game;
    private string AuthTok;

    [Header("Referenced Scripts")]
    SaveUserData svd = new SaveUserData();
    PokerSlotIsWinnerManager pokerSlotIsWinnerManager;

    [Header("Result Panel")]
    [SerializeField] GameObject showResultPanel;
    [Header("Cards")]
    [SerializeField] Sprite[] cards;
    [Header("CardHolder")]
    [SerializeField] Image cardHolder;


    public string chosenCard = "";

    PokerSlotLastTenWinnersManager pokerslotLastTenWinnersManager;

    void Start()
    {
        AuthTok = svd.GetSavedAuthToken();
        pokerSlotIsWinnerManager = FindFirstObjectByType<PokerSlotIsWinnerManager>();
        pokerslotLastTenWinnersManager = FindFirstObjectByType<PokerSlotLastTenWinnersManager>();

        // GetChosenCard();
        // EndSpin();
    }


    private void Update()
    {
        if (isSpinning)
        {
            SpinLogic();
        }
    }

    public void SpinLogic()
    {
        float rotationAmount = spinSpeed * Time.deltaTime; //
        wheel.transform.Rotate(0, 0, rotationAmount);
    }


    public void EndSpin()
    {
        isSpinning = false;
        GetChosenCard();
    }

    public void GetChosenCard()
    {
        StartCoroutine(SendRequestToGetChosenCard()); //
    }

    public void SetToken(string token)
    {
        AuthTok = token;
    }

    public string GetToken()
    {
        return AuthTok;
    }

    IEnumerator SendRequestToGetChosenCard()
    {

        string AuthTok = GetToken();
        string get_chosen_card_live_url = "http://13.234.117.221:2556/api/v1/user/lastgameinfo_poker";

        using (UnityWebRequest request = UnityWebRequest.Get(get_chosen_card_live_url))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", AuthTok);
            request.SetRequestHeader("userType", "User");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("AuthToken: " + AuthTok);
                string jsonResponse = request.downloadHandler.text;
                // Debug.Log("Response: " + jsonResponse);

                // Parse the JSON string
                JObject jsonObject = JObject.Parse(jsonResponse);

                // Access the "chosen" object
                JObject chosenObject = (JObject)jsonObject["data"]["chosen"];
                Debug.Log(chosenObject.ToString());


                // Iterate through the keys and values in the chosenObject
                foreach (var property in chosenObject.Properties())
                {
                    string key = property.Name;
                    int value = (int)property.Value;

                    Debug.Log("Key: " + key + ", Value: " + value);

                    chosenCard = key.ToString();
                    Debug.Log("Before Aligning wheel to angle: " + chosenCard);
                    AlignWheelToSegment(chosenCard);

                }

            }
        }
    }


    IEnumerator ShowResult(string chosenCard)
    {
        Debug.Log("ShowResult");
        MatchTheCard(chosenCard);
        showResultPanel.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        showResultPanel.gameObject.SetActive(false);

        if (pokerslotLastTenWinnersManager != null)
        {
            pokerslotLastTenWinnersManager.LastTenWinHistoryButtonClick();
        }
        else
        {
            Debug.Log("Last Ten Win History Manager is Null");
        }

        if (pokerSlotIsWinnerManager != null)
        {
            StartCoroutine(pokerSlotIsWinnerManager.VictoryButtonClick());
        }
        else
        {
            Debug.Log("Win History Manager or IsWinnerManager is Null");
        }
    }




    public void MatchTheCard(string chosenCard)
    {
        foreach (var card in cards)
        {
            if (card.name == chosenCard)
            {
                Debug.Log("Matched Card: " + card.name);
                Image img = cardHolder.GetComponent<Image>();
                img.sprite = card;
                break;
            }
        }
    }

    private void AlignWheelToSegment(string card)
    {
        Debug.Log("Target Color: " + card);
        if (!string.IsNullOrEmpty(card))
        {
            switch (card)
            {
                case "DiamondK":
                    targetAngle = 0;
                    break;
                case "CubeJ":
                    targetAngle = 30;
                    break;
                case "CubeK":
                    targetAngle = 60;
                    break;
                case "CubeQ":
                    targetAngle = 90;
                    break;
                case "HeartJ":
                    targetAngle = 120;
                    break;
                case "HeartQ":
                    targetAngle = 150;
                    break;
                case "HeartK":
                    targetAngle = 180;
                    break;
                case "SpadeJ":
                    targetAngle = 210;
                    break;
                case "SpadeK":
                    targetAngle = 240;
                    break;
                case "SpadeQ":
                    targetAngle = 270;
                    break;
                case "DiamondJ":
                    targetAngle = 300;
                    break;
                case "DiamondQ":
                    targetAngle = 330;
                    break;
                default:
                    Debug.LogWarning("Unknown card: " + card);
                    return;
            }


            // Rotate the wheel to align with the segment center
            Debug.Log("Aligning wheel to angle: " + targetAngle);
            wheel.transform.rotation = Quaternion.Euler(0, 0, targetAngle);
            StartCoroutine(ShowResult(chosenCard));

            chosenCard = "";
            Debug.Log("After Aligning wheel to angle: " + chosenCard);
        }

    }

    public void BackFromPokerSlot()
    {
        SceneManager.LoadScene("Home");

    }
}