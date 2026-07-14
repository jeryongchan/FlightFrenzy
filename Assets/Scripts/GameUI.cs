using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField]
    ScreenFader startScreen;

    [SerializeField]
    ScreenFader howToPlayScreen;

    [SerializeField]
    ScreenFader hudScreen;

    [SerializeField]
    Button playNowButton;

    [SerializeField]
    Button continueButton;

    [SerializeField]
    AudioSource buttonClickSound;

    [SerializeField]
    TextMeshProUGUI timeLeftLabel;

    [SerializeField]
    TextMeshProUGUI scoreLabel;

    [SerializeField]
    float punchScale = 0.3f;

    [SerializeField]
    float punchDuration = 0.25f;

    [SerializeField]
    int countdownSecondsLeft = 3; // matches the countdown sound, so the timer punches on each beep

    [SerializeField]
    Color countdownTimeLeftColor = Color.red;

    Color normalTimeLeftColor;
    int displayedScore = -1;
    int displayedSecondsLeft = -1;

    void Start()
    {
        normalTimeLeftColor = timeLeftLabel.color;

        playNowButton.onClick.AddListener(GameManager.Instance.EnterHowToPlay);
        continueButton.onClick.AddListener(GameManager.Instance.EnterFlyIn);
        playNowButton.onClick.AddListener(buttonClickSound.Play);
        continueButton.onClick.AddListener(buttonClickSound.Play);

        GameManager.Instance.StateChanged += OnStateChanged;
        ShowScreen(GameState.Start, false); // snap on the first frame, if not can see other screens fading out
    }

    void OnDestroy()
    {
        GameManager.Instance.StateChanged -= OnStateChanged;
    }

    void Update()
    {
        GameState state = GameManager.Instance.State;
        if (state != GameState.FlyIn && state != GameState.Playing) // need include flyin if not might show stale value from last round
        {
            return;
        }

        int score = GameManager.Instance.Score;
        if (score != displayedScore)
        {
            Punch(scoreLabel);
            displayedScore = score;
            scoreLabel.text = score.ToString();
        }

        int secondsLeft = Mathf.FloorToInt(GameManager.Instance.TimeLeft);
        if (secondsLeft != displayedSecondsLeft)
        {
            if (secondsLeft <= countdownSecondsLeft)
            {
                timeLeftLabel.color = countdownTimeLeftColor;
                Punch(timeLeftLabel);
            }
            displayedSecondsLeft = secondsLeft;
            timeLeftLabel.text = secondsLeft.ToString();
        }
    }

    void Punch(TextMeshProUGUI label)
    {
        label.transform.DOComplete(); // snap any running punch back, so fast consecutiv pickups wont accumulate scale
        label.transform
            .DOPunchScale(Vector3.one * punchScale, punchDuration)
            .SetLink(label.gameObject);
    }

    void OnStateChanged(GameState state)
    {
        if (state == GameState.FlyIn)
        {
            displayedScore = -1;
            displayedSecondsLeft = -1;
            timeLeftLabel.color = normalTimeLeftColor;
        }
        ShowScreen(state, true);
    }

    void ShowScreen(GameState state, bool animateFade)
    {
        startScreen.SetVisible(state == GameState.Start, animateFade);
        howToPlayScreen.SetVisible(state == GameState.HowToPlay, animateFade);
        hudScreen.SetVisible(state == GameState.FlyIn || state == GameState.Playing, animateFade);
    }
}
