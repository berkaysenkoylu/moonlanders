using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShipFuel), typeof(Rigidbody))]
public class ShipMovement : MonoBehaviour {

    public float force = 20.0f;
    public float rotSpeed = 15f;
    public float wallForce = 100f; // How much force when hit on a wall
    public float maxShipHealth;
    public GameObject shipCollisionEffect;
    public GameObject shipExplosionEffect;
    public GameObject shipPropulsionEffect;

    Rigidbody rb;
    ShipFuel shipFuel;
    float angle = 30f;
    [SerializeField]
    bool isGrounded = false;
    [SerializeField]
    float shipHealth;
    float volume;
    AudioSource jetEngineSound;
    Vector2 minMaxVolume = new Vector2(0.1f, 0.6f);
    Vector3 propulsionInitialPosition;
    Vector3 fullThrottlePosition;
    Vector3 refVelocity;

    // Event handling
    public delegate void CollisionConsequenceHandler();
    public static event CollisionConsequenceHandler CollidedWithWalls;
    public static event CollisionConsequenceHandler CollidedWithGround;
    public static event CollisionConsequenceHandler LevelFinished;
    public static event CollisionConsequenceHandler ShipDamagedBeyondRepair;

    void Start ()
    {
        // Get the rigidbody component's reference
        rb = GetComponent<Rigidbody>();

        // Get the fuel component's reference
        shipFuel = GetComponent<ShipFuel>();

        // Get the Audio Source
        jetEngineSound = GetComponent<AudioSource>();

        // Set the ship health
        shipHealth = maxShipHealth;

        propulsionInitialPosition = shipPropulsionEffect.transform.localPosition;
        fullThrottlePosition = new Vector3(0f, -1.3f, 0f);

        volume = minMaxVolume.x;

        jetEngineSound.volume = volume;
    }

    void OnEnable()
    {
        volume = 0.2f;

        if(jetEngineSound != null)
            jetEngineSound.volume = volume;
    }

    void OnDisable()
    {
        if (jetEngineSound != null)
            jetEngineSound.volume = 0f;
    }

    void FixedUpdate ()
    {
        // As long as there is enough fuel, one can control the ship
        if(shipFuel.GetFuelAmount() > 0f)
            Ship_Movement();

        // Check for ship health here
        CheckForShipHealth();

        if (shipFuel.GetFuelAmount() <= 0f)
        {
            shipPropulsionEffect.SetActive(false);
        }
        else
        {
            shipPropulsionEffect.SetActive(true);
        }
    }

    // Move the ship using rigidbody's force exertion
    void Ship_Movement()
    {
        if (Input.GetKey(KeyCode.W))
        {
            // Ship Movement
            rb.AddForce(transform.up * force);
            shipFuel.BurnFuel(shipFuel.fuelRunOutCoeff * Time.deltaTime);

            // Ship propulsion effect
            shipPropulsionEffect.transform.localPosition = Vector3.SmoothDamp(shipPropulsionEffect.transform.localPosition, fullThrottlePosition, ref refVelocity, 0.1f);

            volume = Mathf.Lerp(volume, minMaxVolume.y, 0.5f * Time.deltaTime);
        }
        else
        {
            // Ship propulsion effect
            shipPropulsionEffect.transform.localPosition = Vector3.SmoothDamp(shipPropulsionEffect.transform.localPosition, propulsionInitialPosition, ref refVelocity, 0.1f);

            volume = Mathf.Lerp(volume, minMaxVolume.x, 2f * Time.deltaTime);
        }

        jetEngineSound.volume = volume;

        RaycastHit hit;

        if(Physics.Raycast(transform.position, Vector3.down, out hit, 1.7f))
        {
            // hit.collider.gameObject.tag == "Ground" || hit.collider.gameObject.tag == "Obstacle"
            if (hit.collider.gameObject.layer == 9)
            {
                isGrounded = true;

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(hit.normal), rotSpeed * Time.deltaTime);
            }  
        }
        else
        {
            isGrounded = false;
        }

        if (!isGrounded)
        {
            // If not grounded, allow rotations, otherwise don't
            float horizontal = Input.GetAxis("Horizontal");
            //transform.rotation = Quaternion.Slerp(Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 0, -horizontal * angle), rotSpeed * Time.deltaTime);
            
            if (horizontal == 0f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 0), rotSpeed * Time.deltaTime);
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, -horizontal * angle), rotSpeed * Time.deltaTime);
            }
            
        }
    }

    void CheckForShipHealth()
    {
        if(shipHealth <= 0f)
        {
            shipExplosionEffect.transform.position = transform.position;
            shipExplosionEffect.SetActive(true);
            ShipDamagedBeyondRepair();
        }
    }

    public float GetShipHealth()
    {
        return shipHealth;
    }

    public void RestartShip()
    {
        shipHealth = maxShipHealth;

        //shipFuel.ResetFuel();
    }

    void DamageShip(Collision collision)
    {
        shipHealth -= collision.relativeVelocity.magnitude;
        shipHealth = Mathf.Clamp(shipHealth, 0f, maxShipHealth);
    }

    // Pick up fuel cells
    private void OnTriggerEnter(Collider other)
    {
        // Level finish
        if(other.gameObject.tag == "FinishPoint")
        {
            RaycastHit hit;

            // To make sure we are on top of the platform...
            if(Physics.Raycast(transform.position, Vector3.down, out hit, 1.7f))
            {
                if(hit.collider.gameObject.tag == "FinishPoint")
                {
                    // Emit the event
                    LevelFinished();
                }
            }
        }

        if(other.gameObject.tag == "Fuel")
        {
            Debug.Log("Fuel has been picked up!");
            GetComponent<ShipFuel>().AddFuel(50f);
            Destroy(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Obstacle")
        {
            if (CollidedWithWalls != null)
            {
                if (collision.contacts[0].normal != Vector3.up)
                    rb.AddForce(collision.contacts[0].normal * wallForce);

                shipCollisionEffect.SetActive(true);
                shipCollisionEffect.transform.position = collision.contacts[0].point;

                float angle;
                if (collision.contacts[0].normal.x >= 0)
                    angle = -45f;
                else
                    angle = 45f;

                Vector3 newDir = new Vector3();
                
                newDir.x = (collision.contacts[0].normal.x * Mathf.Cos(angle) + collision.contacts[0].normal.y * Mathf.Sin(angle));
                newDir.y = (collision.contacts[0].normal.x * -Mathf.Sin(angle) + collision.contacts[0].normal.y * Mathf.Cos(angle));
                newDir.z = collision.contacts[0].normal.z;

                shipCollisionEffect.transform.forward = newDir; //*

                CollidedWithWalls();
            }

            DamageShip(collision);
        }

        if(collision.gameObject.tag == "Rock")
        {
            rb.AddForce(collision.contacts[0].normal * collision.relativeVelocity.magnitude * 10f); //?

            DamageShip(collision);  // Damage the ship
            CollidedWithWalls();    // To increase the fuel consumption
        }

        if(collision.gameObject.tag == "Ground")
        {
            if(CollidedWithGround != null)
            {
                CollidedWithGround();
            }

            shipCollisionEffect.SetActive(true);
            shipCollisionEffect.transform.position = collision.contacts[0].point;

            if (rb.velocity.x >= 0)
            {
                shipCollisionEffect.transform.forward = Vector3.left;
            }
            else
            {
                shipCollisionEffect.transform.forward = Vector3.right;
            }

            DamageShip(collision);
        }
    }
}
