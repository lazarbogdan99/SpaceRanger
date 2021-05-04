using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialLogSpawner : MonoBehaviour
{
    private List<Transform> logSpawnPoints;
    public List<GameObject> spawnedLogs;

    public GameObject SpecialLog;

    // Time to spawn another Log
    public float spawnRate = 2.0f;
    public float timer;

    private GameObject Right, Left;

    // Used to control how many logs can there be at same time
    public int maxLogs = 1;

    void Start()
    {
        logSpawnPoints = new List<Transform>();
        spawnedLogs = new List<GameObject>();
        // I look for every spawn point with the tag SpecialLogSpawn
        // And add it to our List ( A list is a dynamic array )
        foreach (var obj in GameObject.FindGameObjectsWithTag("SpecialLogSpawn"))
        {
            logSpawnPoints.Add(obj.transform);
        }

        foreach (var obj in GameObject.FindGameObjectsWithTag("SpecialLogSpawnSamll"))
        {
            logSpawnPoints.Add(obj.transform);
        }

        Left = GameObject.Find("LeftBorder");
        Right = GameObject.Find("RightBorder");
    }

    // Update is called once per frame
    void Update()
    {

        // If certain time passed and I didn't spawn too many logs, spawn one
        // And add it to the list
        if (timer >= spawnRate && spawnedLogs.Count < maxLogs)
        {
            timer = 0;
            var randomIndex = Random.Range(0, logSpawnPoints.Count);
            var obj = Instantiate(SpecialLog, logSpawnPoints[randomIndex].position, Quaternion.identity);

            // I get the distance to the left and right boundaries
            var distToLeft = Vector2.Distance(logSpawnPoints[randomIndex].position, Left.transform.position);
            var distToRight = Vector2.Distance(logSpawnPoints[randomIndex].position, Right.transform.position);

            // If the point is closer to the left boundary then I make the log go to the right
            if (distToLeft < distToRight)
            {
                obj.GetComponent<Logs>().ToTheRight = true;
            }

            spawnedLogs.Add(obj);
        }
        else
            timer += Time.deltaTime;
    }
}