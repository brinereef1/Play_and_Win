using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PattiBetManager : MonoBehaviour
{
    public GameObject dialogBox;
    public TMP_InputField moneyInputField;
    public GameObject buttonParent_patti;

    private List<string> betNumbers = new List<string>();
    private int totalBet = 0;
    public TMP_Text responseText;
    public List<Button> buttons_patti = new List<Button>();

    PattiTimer pattiTimer;
    SaveUserData svd = new SaveUserData();
    PattiBetHistoryManager pattiBetHistoryManager;
    private const string betApiUrl = "http://13.234.117.221:2556/api/v1/user/fatafat_userbet";
    private string AuthTok;
    FATAFATWalletManager fATAFATWalletManager;

    [Header("ShowTheBoxNumber")]
    [SerializeField] TMP_Text showTheBoxNumber_text;
    void Start()
    {
        dialogBox.SetActive(false);
        pattiTimer = FindFirstObjectByType<PattiTimer>();
        pattiBetHistoryManager = FindFirstObjectByType<PattiBetHistoryManager>();
        AuthTok = svd.GetSavedAuthToken();
        fATAFATWalletManager = FindFirstObjectByType<FATAFATWalletManager>();
        InitializeButtonsOfPattiPanel();
    }

    public void OnButtonClick(string boxNumber)
    {
        Debug.Log("BoxNo" + boxNumber);
        dialogBox.SetActive(true);
        showTheBoxNumber_text.text = boxNumber;
        betNumbers.Add(boxNumber);
    }
    public void OnCloseDialogBox()
    {
        dialogBox.SetActive(false);
    }
    void InitializeButtonsOfPattiPanel()
    {
        Button[] childButtons = buttonParent_patti.GetComponentsInChildren<Button>();
        buttons_patti.AddRange(childButtons);


        Debug.Log("Total Buttons Initialized: " + buttons_patti.Count);

        for (int i = 0; i < buttons_patti.Count; i++)
        {
            Button button = buttons_patti[i];
            button.onClick.AddListener(() => OnButtonClick(button.name));
        }


    }

    public void OnSubmitBet()
    {
        if (!string.IsNullOrEmpty(moneyInputField.text))
        {
            int betAmount = int.Parse(moneyInputField.text);
            totalBet += betAmount;
            Debug.Log(betNumbers.Count() + "Total Bet");
            if (betNumbers.Count() > 0)
            {
                string value = betNumbers.Last().ToString();
                string catId = GetCategories(value);
                Debug.Log("Categories: " + catId + " For: " + value);


                StartCoroutine(SendBetToServer(betAmount, pattiTimer.GetGameId(), pattiTimer.GetGameRoundIdGenerated(), catId));
                betNumbers.Clear();
            }

            dialogBox.SetActive(false);
            moneyInputField.text = "";
        }
        else
        {
            Debug.LogWarning("Please enter a valid amount.");
        }
    }
    public void SetToken(string token)
    {
        AuthTok = token;
    }

    public string GetToken()
    {
        return AuthTok;
    }
    IEnumerator SendBetToServer(int betAmount, string gameRoundId, string gameRoundIdgenerated, string categoryId)
    {
        Debug.Log("BetAmount = " + betAmount + " GameRoundId = " + gameRoundId + " GameRoundIdgenerated =" + gameRoundIdgenerated + "CategoryId : " + categoryId);

        if (AuthTok == null)
        {
            AuthTok = GetToken();
        }
        PattiBet bet = new PattiBet
        {
            betAmount = betAmount,
            gameRoundId = gameRoundId,
            gameRoundIdgenerated = gameRoundIdgenerated,
            categoryId = categoryId
        };

        PattiBetData betData = new PattiBetData()
        {
            bets = new List<PattiBet> { bet }
        };

        string jsonData = JsonConvert.SerializeObject(betData);
        Debug.Log("Serialized JSON Data: " + jsonData);

        using (UnityWebRequest request = new UnityWebRequest(betApiUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", AuthTok);
            request.SetRequestHeader("userType", "User");
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Bet data successfully sent to server: " + request.downloadHandler.text);
                ShowBetSentResponse("Bet Sent Successfully");
                pattiBetHistoryManager.BetHistoryButtonClick();
                fATAFATWalletManager.GetWalletBalance();
            }
            else
            {
                Debug.LogError("Error sending bet data: " + request.downloadHandler.text);

            }

        }

    }

    public void ShowBetSentResponse(string responseText)
    {
        if (responseText != null)
        {
            this.responseText.text = responseText;
            StartCoroutine(HideResponse());
        }
    }

    IEnumerator HideResponse()
    {
        yield return new WaitForSeconds(2);
        responseText.text = "";
    }

    public string GetCategories(string boxNumber)
    {
        string category = "";
        switch (boxNumber)
        {
            case "100":
                category = "6763be6586756d89958509bc";
                break;
            case "678":
                category = "6763be6586756d89958509bd";
                break;
            case "777":
                category = "6763be6586756d89958509be";
                break;
            case "560":
                category = "6763be6586756d89958509bf";
                break;
            case "470":
                category = "6763be6586756d89958509c0";
                break;
            case "380":
                category = "6763be6586756d89958509c1";
                break;
            case "290":
                category = "6763be6586756d89958509c2";
                break;
            case "119":
                category = "6763be6586756d89958509c3";
                break;
            case "137":
                category = "6763be6586756d89958509c4";
                break;
            case "236":
                category = "6763be6586756d89958509c5";
                break;
            case "146":
                category = "6763be6586756d89958509c6";
                break;
            case "669":
                category = "6763be6586756d89958509c7";
                break;
            case "579":
                category = "6763be6586756d89958509c8";
                break;
            case "399":
                category = "6763be6586756d89958509c9";
                break;
            case "588":
                category = "6763be6586756d89958509ca";
                break;
            case "489":
                category = "6763be6586756d89958509cb";
                break;
            case "245":
                category = "6763be6586756d89958509cc";
                break;
            case "155":
                category = "6763be6586756d89958509cd";
                break;
            case "227":
                category = "6763be6586756d89958509ce";
                break;
            case "344":
                category = "6763be6586756d89958509cf";
                break;
            case "355":
                category = "6763c70e86756d8995851141";
                break;
            case "128":
                category = "6763be6586756d89958509d1";
                break;
            case "000":
                category = "6763b75606d4abe82925e388";
                break;
            case "127":
                category = "6763b75606d4abe82925e389";
                break;
            case "190":
                category = "6763b75606d4abe82925e38a";
                break;
            case "280":
                category = "6763b75606d4abe82925e38b";
                break;
            case "370":
                category = "6763b75606d4abe82925e38c";
                break;
            case "460":
                category = "6763b75606d4abe82925e38d";
                break;
            case "550":
                category = "6763b75606d4abe82925e38e";
                break;
            case "235":
                category = "6763b75606d4abe82925e38f";
                break;
            case "118":
                category = "6763b75606d4abe82925e390";
                break;
            case "578":
                category = "6763b75606d4abe82925e39";
                break;
            case "145":
                category = "6763b75606d4abe82925e392";
                break;
            case "479":
                category = "6763b75606d4abe82925e393";
                break;
            case "668":
                category = "6763b75606d4abe82925e394";
                break;
            case "299":
                category = "6763b75606d4abe82925e395";
                break;
            case "334":
                category = "6763b75606d4abe82925e396";
                break;
            case "488":
                category = "6763b75606d4abe82925e397";
                break;
            case "389":
                category = "6763b75606d4abe82925e398";
                break;
            case "226":
                category = "6763b75606d4abe82925e399";
                break;
            case "569":
                category = "6763b75606d4abe82925e39a";
                break;
            case "677":
                category = "6763b75606d4abe82925e39b";
                break;
            case "136":
                category = "6763b75606d4abe82925e39c";
                break;
            case "244":
                category = "6763b75606d4abe82925e39d";
                break;
            case "900":
                category = "6763f8942436b636ceaf99f7";
                break;
            case "234":
                category = "6763f8942436b636ceaf99f8";
                break;
            case "333":
                category = "6763f8942436b636ceaf99fa";
                break;
            case "180":
                category = "6763f8942436b636ceaf99fa";
                break;
            case "360":
                category = "6763f8942436b636ceaf99fb";
                break;
            case "270":
                category = "6763f8942436b636ceaf99fc";
                break;
            case "450":
                category = "6763f8942436b636ceaf99fd";
                break;
            case "199":
                category = "6763f8942436b636ceaf99fe";
                break;
            case "117":
                category = "6763f8942436b636ceaf99ff";
                break;
            case "469":
                category = "6763f8942436b636ceaf9a00";
                break;
            case "126":
                category = "6763f8942436b636ceaf9a01";
                break;
            case "667":
                category = "6763f8942436b636ceaf9a02";
                break;
            case "478":
                category = "6763f8942436b636ceaf9a03";
                break;
            case "135":
                category = "6763f8942436b636ceaf9a04";
                break;
            case "225":
                category = "6763f8942436b636ceaf9a05";
                break;
            case "144":
                category = "6763f8942436b636ceaf9a06";
                break;
            case "379":
                category = "6763f8942436b636ceaf9a07";
                break;
            case "559":
                category = "6763b75606d4abe82925e39a";
                break;
            case "289":
                category = "6763f8942436b636ceaf9a09";
                break;
            case "388":
                category = "6763c8c486756d89958512cd";
                break;
            case "577":
                category = "6763f8942436b636ceaf9a0b";
                break;
            case "568":
                category = "6763f8942436b636ceaf9a0c";
                break;
            case "800":
                category = "6763f82b2436b636ceaf9980";
                break;
            case "567":
                category = "6763f82b2436b636ceaf9981";
                break;
            case "666":
                category = "6763f82b2436b636ceaf9982";
                break;
            case "170":
                category = "6763f82b2436b636ceaf9983";
                break;
            case "350":
                category = "6763f82b2436b636ceaf9984";
                break;
            case "260":
                category = "6763f82b2436b636ceaf9985";
                break;
            case "288":
                category = "6763f82b2436b636ceaf9986";
                break;
            case "189":
                category = "6763f82b2436b636ceaf9987";
                break;
            case "116":
                category = "6763f82b2436b636ceaf9988";
                break;
            case "233":
                category = "6763f82b2436b636ceaf9989";
                break;
            case "459":
                category = "6763f82b2436b636ceaf998a";
                break;
            case "125":
                category = "6763f82b2436b636ceaf998b";
                break;
            case "224":
                category = "6763f82b2436b636ceaf998c";
                break;
            case "477":
                category = "6763f82b2436b636ceaf998d";
                break;
            case "990":
                category = "6763f82b2436b636ceaf998e";
                break;
            case "134":
                category = "6763f82b2436b636ceaf998f";
                break;
            case "558":
                category = "6763f82b2436b636ceaf9990";
                break;
            case "369":
                category = "6763f82b2436b636ceaf9991";
                break;
            case "378":
                category = "6763f82b2436b636ceaf9992";
                break;
            case "440":
                category = "6763f82b2436b636ceaf9993";
                break;
            case "279":
                category = "6763f82b2436b636ceaf9994";
                break;
            case "468":
                category = "6763f82b2436b636ceaf9995";
                break;
            case "700":
                category = "6763cda786756d899585172a";
                break;
            case "890":
                category = "6763cda786756d899585172b";
                break;
            case "999":
                category = "6763cda786756d899585172c";
                break;
            case "160":
                category = "6763cda786756d899585172d";
                break;
            case "340":
                category = "6763cda786756d899585172e";
                break;
            case "250":
                category = "6763cda786756d899585172f";
                break;
            case "278":
                category = "6763cda786756d8995851730";
                break;
            case "179":
                category = "6763cda786756d8995851731";
                break;
            case "377":
                category = "6763cda786756d8995851732";
                break;
            case "467":
                category = "6763cda786756d8995851733";
                break;
            case "115":
                category = "6763cda786756d8995851734";
                break;
            case "124":
                category = "6763cda786756d8995851735";
                break;
            case "223":
                category = "6763cda786756d8995851736";
                break;
            case "566":
                category = "6763cda786756d8995851737";
                break;
            case "557":
                category = "6763cda786756d8995851738";
                break;
            case "368":
                category = "6763cda786756d8995851739";
                break;
            case "359":
                category = "6763cda786756d899585173a";
                break;
            case "449":
                category = "6763cda786756d899585173b";
                break;
            case "269":
                category = "6763cda786756d899585173c";
                break;
            case "133":
                category = "6763cda786756d899585173d";
                break;
            case "188":
                category = "6763cda786756d899585173e";
                break;
            case "458":
                category = "6763cda786756d899585173f";
                break;
            case "600":
                category = "6763cd0c86756d8995851698";
                break;
            case "123":
                category = "6763cd0c86756d8995851699";
                break;
            case "222":
                category = "6763cd0c86756d899585169a";
                break;
            case "150":
                category = "6763cd0c86756d899585169b";
                break;
            case "330":
                category = "6763cd0c86756d899585169c";
                break;
            case "240":
                category = "6763cd0c86756d899585169d";
                break;
            case "268":
                category = "6763cd0c86756d899585169e";
                break;
            case "169":
                category = "6763cd0c86756d899585169f";
                break;
            case "367":
                category = "6763cd0c86756d89958516a0";
                break;
            case "448":
                category = "6763cd0c86756d89958516a1";
                break;
            case "899":
                category = "6763cd0c86756d89958516a2";
                break;
            case "178":
                category = "6763cd0c86756d89958516a3";
                break;
            case "790":
                category = "6763cd0c86756d89958516a4";
                break;
            case "466":
                category = "6763cd0c86756d89958516a5";
                break;
            case "358":
                category = "6763cd0c86756d89958516a6";
                break;
            case "880":
                category = "6763cd0c86756d89958516a7";
                break;
            case "114":
                category = "6763cd0c86756d89958516a8";
                break;
            case "556":
                category = "6763cd0c86756d89958516a9";
                break;
            case "259":
                category = "6763cd0c86756d89958516aa";
                break;
            case "349":
                category = "6763cd0c86756d89958516ab";
                break;
            case "457":
                category = "6763cd0c86756d89958516ac";
                break;
            case "277":
                category = "6763cd0c86756d89958516ad";
                break;
            case "500":
                category = "6763cc4286756d89958515c5";
                break;
            case "456":
                category = "6763cc4286756d89958515c6";
                break;
            case "555":
                category = "6763cc4286756d89958515c7";
                break;
            case "140":
                category = "6763cc4286756d89958515c8";
                break;
            case "230":
                category = "6763cc4286756d89958515c9";
                break;
            case "690":
                category = "6763cc4286756d89958515ca";
                break;
            case "258":
                category = "6763cc4286756d89958515cb";
                break;
            case "159":
                category = "6763cc4286756d89958515cc";
                break;
            case "357":
                category = "6763cc4286756d89958515cd";
                break;
            case "799":
                category = "6763cc4286756d89958515ce";
                break;
            case "267":
                category = "6763cc4286756d89958515cf";
                break;
            case "780":
                category = "6763cc4286756d89958515d0";
                break;
            case "447":
                category = "6763cc4286756d89958515d1";
                break;
            case "366":
                category = "6763cc4286756d89958515d2";
                break;
            case "113":
                category = "6763cc4286756d89958515d3";
                break;
            case "122":
                category = "6763cc4286756d89958515d4";
                break;
            case "177":
                category = "6763cc4286756d89958515d5";
                break;
            case "249":
                category = "6763cc4286756d89958515d6";
                break;
            case "339":
                category = "6763cc4286756d89958515d7";
                break;
            case "889":
                category = "6763cc4286756d89958515d8";
                break;
            case "348":
                category = "6763cc4286756d89958515d9";
                break;
            case "168":
                category = "6763cc4286756d89958515da";
                break;
            case "400":
                category = "6763c8c486756d89958512bc";
                break;
            case "789":
                category = "6763c8c486756d89958512bd";
                break;
            case "888":
                category = "6763c8c486756d89958512be";
                break;
            case "590":
                category = "6763c8c486756d89958512bf";
                break;
            case "130":
                category = "6763c8c486756d89958512c0";
                break;
            case "680":
                category = "6763c8c486756d89958512c1";
                break;
            case "248":
                category = "6763c8c486756d89958512c2";
                break;
            case "149":
                category = "6763c8c486756d89958512c3";
                break;
            case "347":
                category = "6763c8c486756d89958512c4";
                break;
            case "158":
                category = "6763c8c486756d89958512c5";
                break;
            case "446":
                category = "6763c8c486756d89958512c6";
                break;
            case "699":
                category = "6763c8c486756d89958512c7";
                break;
            case "455":
                category = "6763c8c486756d89958512c8";
                break;
            case "266":
                category = "6763c8c486756d89958512c9";
                break;
            case "112":
                category = "6763c8c486756d89958512ca";
                break;
            case "356":
                category = "6763c8c486756d89958512cb";
                break;
            case "239":
                category = "6763c8c486756d89958512cc";
                break;
            case "338":
                category = "6763c8c486756d89958512cd";
                break;
            case "257":
                category = "6763c8c486756d89958512ce";
                break;
            case "220":
                category = "6763c8c486756d89958512cf";
                break;
            case "770":
                category = "6763c8c486756d89958512d0";
                break;
            case "167":
                category = "6763c8c486756d89958512d1";
                break;
            case "300":
                category = "6763c70e86756d8995851135";
                break;
            case "120":
                category = "6763c70e86756d8995851136";
                break;
            case "111":
                category = "6763c70e86756d8995851137";
                break;
            case "580":
                category = "6763c70e86756d8995851138";
                break;
            case "490":
                category = "6763c70e86756d8995851139";
                break;
            case "670":
                category = "6763c70e86756d899585113a";
                break;
            case "238":
                category = "6763c70e86756d899585113b";
                break;
            case "139":
                category = "6763c70e86756d899585113c";
                break;
            case "337":
                category = "6763c70e86756d899585113d";
                break;
            case "157":
                category = "6763c70e86756d899585113e";
                break;
            case "346":
                category = "6763c70e86756d899585113f";
                break;
            case "689":
                category = "6763c70e86756d8995851140";
                break;
            case "247":
                category = "6763c70e86756d8995851142";
                break;
            case "256":
                category = "6763c70e86756d8995851143";
                break;
            case "166":
                category = "6763c70e86756d8995851144";
                break;
            case "599":
                category = "6763c70e86756d8995851145";
                break;
            case "148":
                category = "6763c70e86756d8995851146";
                break;
            case "788":
                category = "6763c70e86756d8995851147";
                break;
            case "445":
                category = "6763c70e86756d8995851148";
                break;
            case "229":
                category = "6763c70e86756d8995851149";
                break;
            case "779":
                category = "6763c70e86756d899585114a";
                break;
            case "200":
                category = "6763c19386756d8995850c6d";
                break;
            case "345":
                category = "6763c19386756d8995850c6e";
                break;
            case "444":
                category = "6763c19386756d8995850c6f";
                break;
            case "570":
                category = "6763c19386756d8995850c70";
                break;
            case "480":
                category = "6763c19386756d8995850c71";
                break;
            case "390":
                category = "6763c19386756d8995850c72";
                break;
            case "660":
                category = "6763c19386756d8995850c73";
                break;
            case "129":
                category = "6763c19386756d8995850c74";
                break;
            case "237":
                category = "6763c19386756d8995850c75";
                break;
            case "336":
                category = "6763c19386756d8995850c76";
                break;
            case "246":
                category = "6763c19386756d8995850c77";
                break;
            case "679":
                category = "6763c19386756d8995850c78";
                break;
            case "255":
                category = "6763c19386756d8995850c79";
                break;
            case "147":
                category = "6763c19386756d8995850c7a";
                break;
            case "228":
                category = "6763c19386756d8995850c7b";
                break;
            case "499":
                category = "6763c19386756d8995850c7c";
                break;
            case "688":
                category = "6763c19386756d8995850c7d";
                break;
            case "778":
                category = "6763c19386756d8995850c7e";
                break;
            case "138":
                category = "6763c19386756d8995850c7f";
                break;
            case "156":
                category = "6763c19386756d8995850c80";
                break;
            case "110":
                category = "6763c19386756d8995850c81";
                break;
            case "589":
                category = "6763c19386756d8995850c82";
                break;
            case "335":
                category = "6763be6586756d89958509d0";
                break;
            default:
                category = "";
                break;
        }
        return category;
    }

}
[System.Serializable]
public class PattiBet
{
    public int betAmount { get; set; }
    public string gameRoundId { get; set; }
    public string gameRoundIdgenerated { get; set; }
    public string categoryId { get; set; }
}
[System.Serializable]
public class PattiBetData
{
    public List<PattiBet> bets { get; set; }
}
