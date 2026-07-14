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
    TextMeshProUGUI timeLeftLabel;

    [SerializeField]
    TextMeshProUGUI scoreLabel;

    void Start()
    {
        playNowButton.onClick.AddListener(GameManager.Instance.EnterHowToPlay);
        continueButton.onClick.AddListener(GameManager.Instance.EnterPlaying);

        GameManager.Instance.StateChanged += OnStateChanged;
        ShowScreen(GameState.Start, false); // snap on the first frame, if not can see other screens fading out
    }

    void OnDestroy()
    {
        GameManager.Instance.StateChanged -= OnStateChanged;
    }

    void Update()
    {
        if (GameManager.Instance.State == GameState.Playing)
        {
            timeLeftLabel.text = Mathf.FloorToInt(GameManager.Instance.TimeLeft).ToString();
            scoreLabel.text = GameManager.Instance.Score.ToString();
        }
    }

    void OnStateChanged(GameState state)
    {
        ShowScreen(state, true);
    }

    void ShowScreen(GameState state, bool animateFade)
    {
        startScreen.SetVisible(state == GameState.Start, animateFade);
        howToPlayScreen.SetVisible(state == GameState.HowToPlay, animateFade);
        hudScreen.SetVisible(state == GameState.Playing, animateFade);
    }
}
