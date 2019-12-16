using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public GameObject ship;
    public float smoothTime = 2.0f;

    Vector3 targetLocation;
    Vector3 refVel;

	void Start ()
    {

	}
	
	void Update ()
    {
        CameraMovement();
    }

    void CameraMovement()
    {
        if (ship == null)
            return;

        targetLocation = new Vector3(ship.transform.position.x, ship.transform.position.y, transform.position.z);

        targetLocation.y = Mathf.Clamp(targetLocation.y, 6f, 100f);

        transform.position = Vector3.SmoothDamp(transform.position, targetLocation, ref refVel, smoothTime);
    }
}
