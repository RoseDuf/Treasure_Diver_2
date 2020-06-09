using UnityEngine;

public class SpiderSpawner : MonoBehaviour
{
    float spawnDelay;
    [SerializeField]
    float minSpawnDelay = 5;
    [SerializeField]
    float maxSpawnDelay = 10;

    [SerializeField]
    GameObject spider;

    //their spawning coordinates
    [SerializeField]
    float coordY;
    [SerializeField]
    float minX;
    [SerializeField]
    float maxX;
    [SerializeField]
    float minZ;
    [SerializeField]
    float maxZ;

    public static float nextTimeToSpawn;
    int numberOfTimesSpawned = 0; //just an iterator
    bool delayer = true;

    public static bool twoOctosHavePassed; // to check when to change level

    private void Start()
    {
        spawnDelay = Random.Range(minSpawnDelay, maxSpawnDelay);
        nextTimeToSpawn = Time.time + spawnDelay;
        twoOctosHavePassed = false;
        Spider.objectsDestroyed = 0;
    }

    private void Update()
    {
        //when 2 spiders have been destroyed, you can set bool to true (For Levels script)
        if (Spider.objectsDestroyed == 2)
        {
            twoOctosHavePassed = true;
            numberOfTimesSpawned += 1;
        }

        //This is to make sure that the next spawn of 2 spiders has an interval between them.
        if (twoOctosHavePassed == false)
        {
            if(numberOfTimesSpawned > 2)
            {
                delayer = true;
                nextTimeToSpawn = Time.time + spawnDelay;
                numberOfTimesSpawned = 0;
            }
            Timer();
        }
    }

    void Timer()
    {
        if (nextTimeToSpawn <= Time.time) //Time.time is the number of seconds elapsed since the start of the game
        {
            if (numberOfTimesSpawned == 0) //for the first spawn
            {
                SpawnSpider();
                numberOfTimesSpawned += 1;
                spawnDelay = Random.Range(minSpawnDelay, maxSpawnDelay);
            }
            //this following if-statement is just to set the nexttimetospawn so that in the next loop the second spider will spawn at the given random time
            //otherwise the second spider will just spawn immediately
            if (Spider.objectsDestroyed == 1 && delayer == true)
            {
                nextTimeToSpawn = Time.time + spawnDelay;
                delayer = false; //just a boolean so that we don't go through this if-statement again
                return;
            }

            if (numberOfTimesSpawned == 1 && Spider.objectsDestroyed == 1) //for all future spawns
            {
                SpawnSpider();
                numberOfTimesSpawned += 1;
                spawnDelay = Random.Range(minSpawnDelay, maxSpawnDelay);
            }
        }
    }

    void SpawnSpider()
    {
        float coordX = Random.Range(minX, maxX);
        float coordZ = Random.Range(minZ, maxZ);

        Instantiate(spider, new Vector3(coordX, coordY, coordZ), spider.transform.rotation);
    }
}
