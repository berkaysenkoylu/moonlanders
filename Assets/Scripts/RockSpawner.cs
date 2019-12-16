using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpawner : MonoBehaviour {

    public GameObject[] rocks;
    public int maxRockCount;
    public float timing;

    int rockCount;
    float timer;

	void Start ()
    {
        timer = timing;
	}
	
	void Update ()
    {
        timer -= Time.deltaTime;
        rockCount = GetCurrentRockCount();

        if(rockCount < maxRockCount && timer <= 0f)
        {
            SpawnRock();
            timer = timing;
        }
    }

    void OnEnable()
    {
        timer = timing;
    }

    void OnDisable()
    {
        // Clean up the rocks on the scene
        CleanUp();
    }

    void SpawnRock()
    {
        float yPos = Random.Range(12f, 25f);
        Vector3 newPos = transform.position;
        newPos.y = yPos;

        int index = Mathf.FloorToInt(Random.Range(0f, rocks.Length));

        GameObject newRock = Instantiate(rocks[index], newPos, Quaternion.identity);
    }

    int GetCurrentRockCount()
    {
        return GameObject.FindGameObjectsWithTag("Rock").Length;
    }

    void CleanUp()
    {
        GameObject[] remainingRocks = GameObject.FindGameObjectsWithTag("Rock");

        foreach(GameObject rock in remainingRocks)
        {
            Destroy(rock);
        }
    }
}
