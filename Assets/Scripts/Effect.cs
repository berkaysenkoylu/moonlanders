using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour {

    public float lifeTime = 1f;

    float timer;

    private void OnEnable()
    {
        
    }

    void Update ()
    {
        timer += Time.deltaTime;

        if(timer >= lifeTime)
        {
            gameObject.SetActive(false);
        }
	}

    void OnDisable()
    {
        timer = 0f;
    }
}
