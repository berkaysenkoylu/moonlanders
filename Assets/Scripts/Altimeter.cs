using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Altimeter : MonoBehaviour {

    public float altimeterRange = 100.0f;
    public LayerMask layerMask;
    public float altimeterOffsetY = 0.2f;
    public RawImage gaugeArrow;

    float altitude = 0f;
    float startingAngleForGaugeArrow = 60f;
    float angleRange = 120f;
    float angle;

	void Start ()
    {

	}
	
	void Update ()
    {
        UpdateAltimeterGauge();
    }

    void MeasureAltitude()
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.position, Vector3.down, out hit, altimeterRange, layerMask))
        {
            altitude = Vector3.Distance(transform.position - new Vector3(0f, GetComponent<CapsuleCollider>().bounds.extents.y + altimeterOffsetY, 0f), hit.point);
            if (altitude <= 0.05f)
                altitude = 0f;
        }
        else
        {
            altitude = 100f;
        }

        //Debug.Log(altitude);
    }

    public float GetAltitude()
    {
        return altitude;
    }

    public void UpdateAltimeterGauge()
    {
        MeasureAltitude();

        angle = startingAngleForGaugeArrow - ((angleRange / altimeterRange) * altitude);

        gaugeArrow.rectTransform.rotation = Quaternion.Slerp(gaugeArrow.rectTransform.rotation, Quaternion.Euler(0f, 0f, angle), 0.1f);
    }
}
