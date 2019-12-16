using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour {

    [Range(-1f, 1f)]
	public float rotSpeed;
    public bool changeSpeedOverTime;
    [Range(0.0f, 1.0f)]
    public float speedMultiplier = 0.1f;

    float minimum = -1.0f;
    float maximum = 1.0f;
    float t = 0.0f;
    Animator hazardAnim;

	void Start () 
	{
        hazardAnim = GetComponent<Animator>();

        hazardAnim.SetFloat("animSpeed", rotSpeed);
    }
	
	void Update ()
    {
        if(changeSpeedOverTime)
            VarianceInSpeed();
    }

    void VarianceInSpeed()
    {
        t += speedMultiplier * Time.deltaTime;

        if (t > 1.0f)
        {
            float temp = maximum;
            maximum = minimum;
            minimum = temp;
            t = 0.0f;
        }

        rotSpeed = Mathf.Lerp(minimum, maximum, t);
        hazardAnim.SetFloat("animSpeed", rotSpeed);
    }
}
