using UnityEngine;

public class SharkSpawner : MonoBehaviour
{
    //SHARK SPAWNER AND TIMER

    float spawnDelay;

    [SerializeField]
    Shark shark;

    [SerializeField]
    SpawnPoint[] spawnPoints;

    float nextTimeToSpawn;

    private void Start()
    {
        nextTimeToSpawn = 0f;
    }

    private void Update()
    {
        spawnDelayDecrease();

        //Debug.Log(nextTimeToSpawn);

        if (nextTimeToSpawn <= Time.time) //Time.time is the number of seconds elapsed since the start of the game
        {
            SpawnShark();
            nextTimeToSpawn = Time.time + spawnDelay;
        }
    }

    //the more levels we go through, the faster the sharks become
    void spawnDelayDecrease()
    {
        //spawnDelay = Mathf.Pow(1.4f, -(7-1) + 3.7f); // exponential
        spawnDelay = 10f / ((float)Levels.levels); // division
    }

    void SpawnShark()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);
        SpawnPoint spawnPoint = spawnPoints[randomIndex];

        shark.Point1 = spawnPoint.PointA;
        shark.Point2 = spawnPoint.PointB;

        Instantiate(shark, shark.Point1, Quaternion.identity);
    }
}
