using UnityEngine;

public class PlaneController : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 10f;

    [SerializeField]
    float maxBankDegrees = 30f;

    [SerializeField]
    float bankSmoothing = 8f;

    float currentBankDegrees;

    public void ResetPosition()
    {
        Vector3 position = transform.position;
        position.x = 0f;
        transform.position = position;

        currentBankDegrees = 0f;
        transform.rotation = Quaternion.identity;
    }

    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        Vector3 position = transform.position;
        position.x = Mathf.Clamp(
            position.x + horizontalInput * moveSpeed * Time.deltaTime,
            PlayArea.LeftLimitX,
            PlayArea.RightLimitX
        );
        transform.position = position;

        float targetBankDegrees = -horizontalInput * maxBankDegrees; // negative z banks into the turn
        currentBankDegrees = Mathf.Lerp(currentBankDegrees, targetBankDegrees, bankSmoothing * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0f, 0f, currentBankDegrees);
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
