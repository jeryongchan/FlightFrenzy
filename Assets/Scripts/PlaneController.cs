using UnityEngine;

public class PlaneController : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 10f;
    int score;

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
    }

    void OnTriggerEnter(Collider other)
    {
        RingController ring = other.GetComponent<RingController>();
        if (ring != null)
        {
            score++; // to be shown in UI
            Destroy(ring.gameObject);
        }
    }
}
