using DG.Tweening; // pre-imported in assignment
using UnityEngine;

// fades a canvasgroup in and out
public class ScreenFader : MonoBehaviour
{
    [SerializeField]
    float fadeDuration = 0.3f;

    CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // need to hide instead of disabling, since disabled objects cannot run fade logic
    public void SetVisible(bool visible, bool animate)
    {
        canvasGroup.blocksRaycasts = visible; // prevent clicking on hidden buttons
        float targetAlpha = visible ? 1f : 0f;
        if (animate)
        {
            canvasGroup.DOFade(targetAlpha, fadeDuration);
        }
        else
        {
            canvasGroup.alpha = targetAlpha;
        }
    }
}
