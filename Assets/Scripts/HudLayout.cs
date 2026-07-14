using UnityEngine;

// for responsive design: moves the hud between portrait and landscape layouts
public class HudLayout : MonoBehaviour
{
    [SerializeField]
    RectTransform timeLeft;

    [SerializeField]
    RectTransform score;

    [SerializeField]
    Vector2 portraitTimeLeftOffset = new Vector2(0f, -80f); // from top center

    [SerializeField]
    Vector2 portraitScoreOffset = new Vector2(0f, 80f); // from bottom center

    [SerializeField]
    Vector2 landscapeTimeLeftOffset = new Vector2(-180f, -90f); // from top right

    [SerializeField]
    Vector2 landscapeScoreOffset = new Vector2(140f, -90f); // from top left

    [SerializeField]
    float landscapeScale = 0.7f; // hud shrinks in landscape to stay proportional to gameplay.

    bool wasPortrait;

    void Start()
    {
        Apply(Screen.height >= Screen.width);
    }

    void Update()
    {
        bool isPortrait = Screen.height >= Screen.width;
        if (isPortrait != wasPortrait)
        {
            Apply(isPortrait);
        }
    }

    void Apply(bool isPortrait)
    {
        wasPortrait = isPortrait;
        float scale = isPortrait ? 1f : landscapeScale;
        if (isPortrait)
        {
            PlaceAt(timeLeft, new Vector2(0.5f, 1f), portraitTimeLeftOffset, scale);
            PlaceAt(score, new Vector2(0.5f, 0f), portraitScoreOffset, scale);
        }
        else
        {
            PlaceAt(timeLeft, new Vector2(1f, 1f), landscapeTimeLeftOffset, scale);
            PlaceAt(score, new Vector2(0f, 1f), landscapeScoreOffset, scale);
        }
    }

    void PlaceAt(RectTransform element, Vector2 anchor, Vector2 offset, float scale)
    {
        element.anchorMin = anchor;
        element.anchorMax = anchor;
        element.anchoredPosition = offset;
        element.localScale = Vector3.one * scale;
    }
}
