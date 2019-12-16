using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelGauge : MonoBehaviour {

    public RawImage gaugeArrow;

    ShipFuel shipFuel;

    void Start ()
    {
        shipFuel = GetComponent<ShipFuel>();

        gaugeArrow.rectTransform.rotation = Quaternion.Euler(0f, 0f, -60f);
    }
	
	void Update ()
    {
        UpdateTheGauge();
    }

    float CalculateAngle()
    {
        float shipRemainingFuel = shipFuel.GetFuelAmount();

        float angleCoeff = GetAngleCorrespondance(shipFuel.initialFuel, 120f);

        float angle = shipRemainingFuel * angleCoeff; // 0.6f for 200f

        return 60.0f - angle;
    }

    void UpdateTheGauge()
    {
        gaugeArrow.rectTransform.rotation = Quaternion.Slerp(gaugeArrow.rectTransform.rotation, Quaternion.Euler(0f, 0f, CalculateAngle()), 0.1f);
    }

    float GetAngleCorrespondance(float maxFuelAmount, float angleAmount)
    {
        return angleAmount / maxFuelAmount;
    }
}
