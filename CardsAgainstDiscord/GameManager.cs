using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Discord;

public class GameManager : MonoBehaviour
{

    [Header("UI Items")]
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private bool isSettingsOpen;
    [SerializeField] private Text volumePercentageText;

    [SerializeField] private bool isCreatingNewGame = false;
    [SerializeField] private bool initSettings = false;
    [SerializeField] private bool initCardPacks = false;
    [SerializeField] private Button settingsBtn;
    [SerializeField] private Button cardPacksBtn;
    [SerializeField] private Button startGameBtn;

    [SerializeField] private Slider turnTimerSeconds;
    [SerializeField] private Text turnTimerCreateGame;
    [SerializeField] private float turnTimerAmount = 30f;
    [SerializeField] private Text turnTimer;

    [SerializeField] private Slider scoreToWinSlider;
    [SerializeField] private float scoreToWin = 7;
    [SerializeField] private Text scoreToWinText;
    [SerializeField] private Text scoreValue;

    [SerializeField] private int includedPackAmount = 1;
    [SerializeField] private Text includedPackText;

    [SerializeField] public bool isAdultContent = false;
    [SerializeField] private Toggle adultContentToggle;

    [SerializeField] private float playerCount;
    [SerializeField] private float maxPlayerCount;
    [SerializeField] private Text playerCountText;
    [SerializeField] private Slider playerCountSlider;

    [Header("Lists")]
    [SerializeField] private List<AnswerCard> answerCardsDeck;
    [SerializeField] private List<QuestionCard> questionCardsDeck;

    [SerializeField] private List<ActivityParty> playerList;

    public static GameManager instance;

    public void UpdateTurnTimerUI_CreateGame(string setting)
    {
        bool turnTimerCompleted = false;
        bool scoreToWinCompleted = false;
        bool playerCountCompleted = false;

        if (setting == "TurnTimer")
        {
            turnTimerCompleted = true;

            if (turnTimerSeconds.value == 0)
            {
                turnTimerCreateGame.text = "NO TIMER";
                turnTimerAmount = 0;
            }
            else
            {
                turnTimerCreateGame.text = turnTimerSeconds.value.ToString() + " SECONDS";
                turnTimerAmount = turnTimerSeconds.value;
            }
        }
        else if (setting == "ScoreToWin")
        {
            scoreToWinCompleted = true;

            if (scoreToWinSlider.value == 0)
            {
                scoreValue.text = "\u221E";
                scoreToWin = 0f;
            }
            else
            {
                scoreValue.text = scoreToWinSlider.value.ToString();
                scoreToWin = scoreToWinSlider.value;
            }
        }
        else if (setting == "PlayerCount")
        {
            playerCountCompleted = true;
            playerCountText.text = playerCountSlider.value.ToString();
            maxPlayerCount = playerCountSlider.value;
        }
        if (turnTimerCompleted && scoreToWinCompleted && playerCountCompleted)
        {
            initSettings = true;
        }
    }

    private void ErrorChecked()
    {
        if (initSettings)
        {
            settingsBtn.interactable = true;
        }
        if (initCardPacks)
        {
            cardPacksBtn.interactable = true;
        }
        if (initSettings && initCardPacks)
        {
            startGameBtn.interactable = true;
        }
    }

    public AnswerCard GetNewAnswerCard()
    {
        int remainingCards = answerCardsDeck.Count;

        int random = Random.Range(0, remainingCards);

        //answerCardsDeck.RemoveAt(random);
        //not sure where to put the line above so that the
        //card gets removed from the current deck while still transferring
        //the data of the card to the playermanager and into the players hand

        return answerCardsDeck[random];
    }



    public void NewGameBtn()
    {
        isCreatingNewGame = true;
    }

    public void StartGameBtn()
    {
        int playerCount = playerList.Count;
        for (int i = 0; i < playerCount; i++)
        {
            //deal 10 cards to each player from the answerCardsDeck List<T>
        }
        //check to confirm all players.whitecard.count is == 10 (error check)
        //generate a random number between 0 -> playerList.Count;
        //assign player who was randomly picked card czar (boolean on playermanager = true)
    }

    private IEnumerator CountdownTimer()
    {
        float counter = turnTimerAmount;
        while (counter > 0)
        {
            yield return new WaitForSeconds(1);
            counter--;
        }
    }

    private void NewRound()
    {
        //check boolean async to see if all players.submitted have submitted their cards
        //if all players have submitted their cards OR timer elapses.
        //show all white cards and black card for card czar (no timer)
        //card czar chooses a card to win
        //player who gave that card is given +1 point to score
        //check what player (number) in list was card czar
        //assign new card czar > next player on list (if last player on list then reset to 0 position)
        //update round++ (ui)
        CountdownTimer();
    }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        //playerList = new List<ActivityParty>();

        InitializeGameInformation();

        //GameSettingsOverlay.instance.InitOverlay();
    }

    private void InitializeGameInformation()
    {
        turnTimer.text = turnTimerAmount + " Seconds";
        scoreToWinText.text = scoreToWin.ToString();
        includedPackText.text = includedPackAmount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        //if (isCreatingNewGame)
        //{
        //    ErrorChecked();
        //}
    }

    public void SetAdultContent()
    {
        if (adultContentToggle.isOn)
        {
            isAdultContent = true;
        }
        else
        {
            isAdultContent = false;
        }
    }

    public void UpdateVolumePercentage()
    {
        isSettingsOpen = SettingsMenuOpen();

        if (isSettingsOpen)
        {
            volumePercentageText.text = volumeSlider.value.ToString() + "%";
        }
    }

    private bool SettingsMenuOpen()
    {
        if (GameObject.Find("SettingsPanel").activeInHierarchy)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public Text UpdateTurnTimer(int timerAmount)
    {
        turnTimerAmount = timerAmount;
        turnTimer.text = turnTimerAmount.ToString();
        return turnTimer;
    }

    public float GetTimerAmount()
    {
        return turnTimerAmount;
    }

    public float GetScoreToWin()
    {
        return scoreToWin;
    }

    public int GetIncludedPacks()
    {
        return includedPackAmount;
    }

    public bool IsAdultContentEnabled()
    {
        return isAdultContent;
    }

}
