using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceField : MonoBehaviour
{

    public float forceAmount = 200f;
    public float timerAmount = 0.75f;

    bool isWithinField = false;
    GameObject targetObject;
    float timer;

    void Start()
    {
        timer = timerAmount;
    }

    void Update()
    {
        if (isWithinField)
        {
            timer -= Time.deltaTime;

            if (targetObject != null && timer <= 0f)
                ExertRandomForce(targetObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isWithinField = true;

            targetObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isWithinField = false;

            targetObject = null;

            ResetTimer();
        }
    }

    private Vector3 GetRandomDirection()
    {
        Vector3 direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
        direction = direction.normalized;

        return direction;
    }

    void ExertRandomForce(GameObject target)
    {
        target.GetComponent<Rigidbody>().AddForce(forceAmount * GetRandomDirection());

        ResetTimer();
    }

    void ResetTimer()
    {
        timer = timerAmount;
    }
}
