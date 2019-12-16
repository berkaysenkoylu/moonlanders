using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipFuel : MonoBehaviour {

    [SerializeField]
    private float shipFuel;

    public float initialFuel;
    public Vector2 fuelConstraints; // Min and Max amount that fuel can be
    public float fuelRunOutCoeff = 5f; // How fast fuel is consumed

	void Start ()
    {
        shipFuel = initialFuel;

        // Subscribe to event
        ShipMovement.CollidedWithWalls += IncrementFuelConsumption;

        fuelConstraints = new Vector2(0f, initialFuel);
    }
	
    public void AddFuel(float amount)
    {
        shipFuel += amount;
        if (shipFuel >= fuelConstraints.y)
            shipFuel = fuelConstraints.y;
    }

    public void BurnFuel(float amount)
    {
        shipFuel -= amount;
        if (shipFuel <= fuelConstraints.x)
            shipFuel = fuelConstraints.x;
    }

    void IncrementFuelConsumption()
    {
        fuelRunOutCoeff *= 1.1f;
    }

    public float GetFuelAmount()
    {
        return shipFuel;
    }

    public void ResetFuel()
    {
        shipFuel = initialFuel;

        fuelRunOutCoeff = 5f;
    }
}
