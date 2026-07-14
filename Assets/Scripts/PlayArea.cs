using UnityEngine;

// play area follows the screen aspect so wider screens allow wider movement
public static class PlayArea
{
    const float PortraitAspect = 9f / 16f; // assuming this is the standard portrait aspect
    const float PortraitHalfWidth = 6f; // for above aspect, 6 is the tuned half limit.

    public static float RightLimitX
    {
        get { return PortraitHalfWidth * (Screen.width / (float)Screen.height) / PortraitAspect; }
    }
    public static float LeftLimitX
    {
        get { return -RightLimitX; }
    }
    public static float WidthScale
    {
        get { return RightLimitX / PortraitHalfWidth; }
    }
}
