using DG.Tweening;
using UnityEngine;

public class PlaneController : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 10f;

    [SerializeField]
    float maxBankDegrees = 30f;

    [SerializeField]
    float bankSmoothing = 8f;

    [SerializeField]
    Vector3 flyInOffset = new Vector3(14f, -10f, -12f); // stay off screen (bottom right) then fly in
    float currentBankDegrees;
    Vector3 homePosition; // basically Vector3.zero

    void Awake()
    {
        homePosition = transform.position;
        ResetPosition();
    }

    public void ResetPosition() // for fly in
    {
        transform.position = homePosition + flyInOffset;
        currentBankDegrees = 0f;
        transform.rotation = Quaternion.identity;
    }

    public void FlyIn(float duration)
    {
        transform.DOMove(homePosition, duration).SetEase(Ease.OutCubic).SetLink(gameObject);
    }

    void Update()
    {
        float horizontalInput = ReadHorizontalInput();

        Vector3 position = transform.position;
        position.x = Mathf.Clamp(
            position.x + horizontalInput * moveSpeed * PlayArea.WidthScale * Time.deltaTime, // widthscale is so in wider screen it move faster
            PlayArea.LeftLimitX,
            PlayArea.RightLimitX
        );
        transform.position = position;

        float targetBankDegrees = -horizontalInput * maxBankDegrees; // negative z banks into the turn
        currentBankDegrees = Mathf.Lerp(
            currentBankDegrees,
            targetBankDegrees,
            bankSmoothing * Time.deltaTime
        );
        transform.rotation = Quaternion.Euler(0f, 0f, currentBankDegrees);
    }

    // touch on the left or right half steers, so the same build works on mobile without a platform check
    float ReadHorizontalInput()
    {
        if (Input.touchCount > 0)
        {
            return Input.GetTouch(0).position.x < Screen.width * 0.5f ? -1f : 1f;
        }
        return Input.GetAxisRaw("Horizontal");
    }

    void OnTriggerEnter(Collider other)
    {
        RingController ring = other.GetComponent<RingController>();
        if (ring != null)
        {
            GameManager.Instance.AddScore();
            Destroy(ring.gameObject);
        }
    }
}
