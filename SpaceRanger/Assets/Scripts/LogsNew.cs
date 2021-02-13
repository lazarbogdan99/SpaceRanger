using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogsNew : MonoBehaviour
{

    public float spawnDelay = 1.2f;
    public GameObject log;
    public Transform[] LogSpawnPoints;

    float nextTimeToSpawn = 0f;

    // Update is called once per frame
    void Update()
    {
        if (nextTimeToSpawn <= Time.time)
        {
            Spawnlogs();
            nextTimeToSpawn = Time.time + spawnDelay;
        }
    }

    void Spawnlogs()
    {
        int randomIndex = Random.Range(0, LogSpawnPoints.Length);
        Transform LogSpawnPoint = LogSpawnPoints[randomIndex];

        Instantiate(log, LogSpawnPoint.position, LogSpawnPoint.rotation);
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        // Frog? Then make it a Child
        if (coll.tag == "Player")
            coll.transform.parent = transform;
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag == "Player")
            coll.transform.parent = null;
    }
}
