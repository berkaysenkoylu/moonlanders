using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingWall : MonoBehaviour {

    Vector3 firstPos;
    Vector3 secondPos;
    Vector3 targetDestination;
    float offsetDist = 0.5f;
    private float wait;
    bool readyToMove = true;

    public float waitTime = 5f;

    public Vector2 yMinMax = new Vector2(-9f, 6f);
    public float wallSpeed = 1.0f;

	void Start ()
    {
        // Trajectory from Point A to Point B
        firstPos = transform.position + new Vector3(0f, yMinMax.x, 0f);
        secondPos = transform.position + new Vector3(0f, yMinMax.y, 0f);

        // Set the initial target destination
        targetDestination = firstPos;

        readyToMove = true;
        wait = Mathf.Max(0f, waitTime);
    }
	
	void Update ()
    {
        if (Vector3.Distance(transform.position, targetDestination) > offsetDist && readyToMove)
        {
            Vector3 dir = (targetDestination - transform.position).normalized;
            transform.Translate(dir * wallSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetDestination) < offsetDist)
            {
                readyToMove = false;
            }
        }
        else
        {
            if (targetDestination == firstPos)
                targetDestination = secondPos;
            else
                targetDestination = firstPos;
        }

        if (!readyToMove)
        {
            wait -= Time.deltaTime;
            if(wait <= 0f)
            {
                readyToMove = true;
                wait = waitTime;
            }
        }
    }
}
