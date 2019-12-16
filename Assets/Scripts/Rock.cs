using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour {

    float speed = 10f;
    float rotationSpeed = 5f;
    Vector3 destination;
    Vector3 selfRotation;


    void Start ()
    {
        float randYPos = Random.Range(10f, 20f);
        destination = transform.position + new Vector3(-160f, randYPos, 0f);

        selfRotation = new Vector3(Random.Range(0, 40f), Random.Range(0, 40f), Random.Range(0, 40f));
    }
	
	void Update ()
    {
		if(Vector3.Distance(transform.position, destination) >= 1.0f)
        {
            MoveTowardsDestination();
        }
        else
        {
            DestroySelf();
        }
	}

    void MoveTowardsDestination()
    {
        Vector3 dir = (destination - transform.position).normalized;

        transform.Translate(dir * speed * Time.deltaTime, Space.World);
        transform.Rotate(selfRotation * rotationSpeed * Time.deltaTime);
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
