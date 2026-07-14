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
        position.z -= approachSpeed * Time.deltaTime;
        transform.position = position;

        if (position.z < despawnZ)
        {
            Destroy(gameObject); // despawn uncollected rings
        }
    }
}
