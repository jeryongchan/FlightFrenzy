using UnityEngine;

// controls individual ring
public class RingController : MonoBehaviour
{
    [SerializeField]
    float approachSpeed = 20f;

    [SerializeField]
    float despawnZ = -35f;

    void Update()
    {
        Vector3 position = transform.position;
        position.z -= approachSpeed * GameManager.Instance.WorldSpeed * Time.deltaTime;
        transform.position = position;

        transform.rotation = Quaternion.Euler(0f, RingFlipper.AngleY, 0f);

        if (position.z < despawnZ)
        {
            Destroy(gameObject); // despawn uncollected rings
        }
    }
}
