using UnityEngine;

public class RingSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject ringPrefab;

    [SerializeField]
    float columnSpawnInterval = 1.5f;

    [SerializeField]
    float ringSpawnInterval = 0.1f;

    [SerializeField]
    float spawnZ = 20f; // ring spawn far away

    [SerializeField]
    int minRingsPerColumn = 5;

    [SerializeField]
    int maxRingsPerColumn = 10;

    float currentColumnX;
    int ringsLeftInColumn;
    float timeUntilNextSpawn;

    void OnEnable()
    {
        // Debug.Log("ring spawn enable");
        ringsLeftInColumn = 0;
        timeUntilNextSpawn = 0f; // start a fresh column, if not in next round can see some leftover coins
    }

    void Update()
    {
        timeUntilNextSpawn -= Time.deltaTime;
        if (timeUntilNextSpawn < 0f)
        {
            if (ringsLeftInColumn > 0)
            {
                SpawnRing();
            }
            else
            {
                StartNewColumn();
            }
        }
    }

    void StartNewColumn()
    {
        currentColumnX = Random.Range(PlayArea.LeftLimitX, PlayArea.RightLimitX);
        ringsLeftInColumn = Random.Range(minRingsPerColumn, maxRingsPerColumn + 1);
        timeUntilNextSpawn = columnSpawnInterval;
    }

    void SpawnRing()
    {
        Vector3 ringPosition = new Vector3(currentColumnX, 0f, spawnZ);
        Instantiate(ringPrefab, ringPosition, Quaternion.identity);
        ringsLeftInColumn--;
        timeUntilNextSpawn = ringSpawnInterval;
    }
}
