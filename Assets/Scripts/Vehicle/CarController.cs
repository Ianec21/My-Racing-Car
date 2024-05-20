using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private float speed;
    private Rigidbody rb;
    public WheelObjects wheelObjects;
    public WheelColliders wheelColliders;

    [Header("Inputs")]
    public float gasInput;
    public float brakeInput;
    public float steeringInput;

    [Header("Values")]
    public float motorPower;
    public float brakePower;
    public float steeringAngle;

    [Header("Others")]
    public AnimationCurve steeringCurve;
    private float slipAngle;
    private PlayerController player;
    public IgnitionScript ignitionScript;

    [Header("Effects")]
    public ParticleSystem smokeParticleSystem;
    private ParticleSystem smokeParticle;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();

        smokeParticle = GameObject.Instantiate(smokeParticleSystem);
        smokeParticle.transform.parent = GameObject.Find("Exhaust").transform;
        smokeParticle.transform.localPosition = new Vector3(0.486f, 0.564f, -3.831f);
        smokeParticle.transform.localRotation = Quaternion.Euler(179.842f, 0f, 0f);
        smokeParticle.enableEmission = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (ignitionScript.ignitionState)
        {
            smokeParticle.enableEmission = true;
        }
        else smokeParticle.enableEmission = false;

        if (player.isInCar() && ignitionScript.ignitionState)
        {
            HandleInput();
            ApplyMotorPower();
            ApplySteering();
            HandleBrake();
        }

        //Wheel Position based on collider
        SetWheelPosition(wheelColliders.FrontLeft, wheelObjects.FrontLeft);
        SetWheelPosition(wheelColliders.FrontRight, wheelObjects.FrontRight);
        SetWheelPosition(wheelColliders.RearRight, wheelObjects.RearRight);
        SetWheelPosition(wheelColliders.RearLeft, wheelObjects.RearLeft);
    }

    void FixedUpdate()
    {
        speed = rb.velocity.magnitude;
    }

    public void HandleInput()
    {
        gasInput = Input.GetAxis("Vertical");
        steeringInput = Input.GetAxis("Horizontal");

        slipAngle = Vector3.Angle(transform.forward, rb.velocity - transform.forward);
        if(slipAngle < 120f)
        {
            if (gasInput < 0)
            {
                brakeInput = Mathf.Abs(gasInput);
                gasInput = 0;
            }
            else brakeInput = 0;
        }
        else brakeInput = 0;
    }

    public void HandleBrake()
    {
        wheelColliders.FrontLeft.brakeTorque = brakeInput * brakePower * 0.7f;
        wheelColliders.FrontRight.brakeTorque = brakeInput * brakePower * 0.7f;

        wheelColliders.RearLeft.brakeTorque = brakeInput * brakePower * 0.3f;
        wheelColliders.RearRight.brakeTorque = brakeInput * brakePower * 0.3f;
    }

    public void ApplySteering()
    {
        float steer = steeringInput * steeringCurve.Evaluate(speed);

        wheelColliders.FrontLeft.steerAngle = steer;
        wheelColliders.FrontRight.steerAngle = steer;
    }

    public void ApplyMotorPower()
    {
        wheelColliders.RearLeft.motorTorque = motorPower * gasInput;
        wheelColliders.RearRight.motorTorque = motorPower * gasInput;
    }

    public void SetWheelPosition(WheelCollider collider, Transform wheelTransform)
    {
        Quaternion quat;
        Vector3 pos;

        collider.GetWorldPose(out pos, out quat);
        wheelTransform.position = pos;
        wheelTransform.rotation = quat;
    }

    public float getSpeed()
    {
        return speed;
    }

    [System.Serializable]
    public class WheelObjects
    {
        public Transform FrontLeft;
        public Transform FrontRight;
        public Transform RearRight;
        public Transform RearLeft;
    }

    [System.Serializable]
    public class WheelColliders
    {
        public WheelCollider FrontLeft;
        public WheelCollider FrontRight;
        public WheelCollider RearRight;
        public WheelCollider RearLeft;
    }
}
