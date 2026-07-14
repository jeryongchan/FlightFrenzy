using DG.Tweening;
using UnityEngine;

// drives one shared flip angle for every ring, so rings spawned at different times stay in sync
public class RingFlipper : MonoBehaviour
{
    public static float AngleY { get; private set; }

    [SerializeField]
    float flipInterval = 1.5f;

    [SerializeField]
    float flipDuration = 0.35f;

    void Start()
    {
        AngleY = 0f;
        DOTween
            .Sequence()
            .AppendInterval(flipInterval)
            .Append(DOTween.To(() => AngleY, angle => AngleY = angle, 180f, flipDuration))
            .SetLoops(-1, LoopType.Incremental)
            .SetLink(gameObject);
    }
}
