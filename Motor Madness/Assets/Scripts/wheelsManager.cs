using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;


[RequireComponent(typeof(WheelCollider))]

public class wheelsManager : MonoBehaviour
{

    [Range(-1f, 1f)]
    public float SteerPercent;                  //Percentage of wheel turns, 1 - maximum possible turn CarController.Steer.MaxSteerAngle, -1 negative wheel turn (For example, to turn the rear wheels).
    public bool DriveWheel;
    public float MaxBrakeTorque;
    public bool HandBrakeWheel;
    public Transform WheelView;                 //The transform of which takes the position and rotation of the wheel.
    public Transform WheelHub;                  //The transform which occupies rotation only along the Y axis of the wheel.
    public float MaxVisualDamageAngle = 5f;     //The maximum offset angle for children to visualize damage.
    
 

    public float RPM { get { return WheelCollider.rpm; } }
    public float CurrentMaxSlip { get { return Mathf.Max(CurrentForwardSlip, CurrentSidewaysSlip); } }
    public float CurrentForwardSlip { get; private set; }
    public float CurrentSidewaysSlip { get; private set; }
    public float SlipNormalized { get; private set; }
    public float ForwardSlipNormalized { get; private set; }
    public float SidewaysSlipNormalized { get; private set; }
    public bool HasForwardSlip { get { return 0.3f + CurrentForwardSlip > WheelCollider.forwardFriction.asymptoteSlip ; } }
    public bool HasSideSlip { get { return 0.3f + CurrentSidewaysSlip > WheelCollider.sidewaysFriction.asymptoteSlip ; } }
    public WheelHit GetHit { get { return Hit; } }
    public Vector3 HitPoint { get; private set; }
    public bool IsGrounded { get { return  WheelCollider.isGrounded; } } //!IsDead &&
    public bool StopEmitFX { get; set; }
    public float Radius { get { return WheelCollider.radius; } }
    public Vector3 LocalPositionOnAwake { get; private set; }       //For CarState

 
    float BrakeSpeed = 2;
    float CurrentBrakeTorque;


    [System.NonSerialized]
    public Vector3 Position;
    [System.NonSerialized]
    public Quaternion Rotation;

    public bool IsSteeringWheel { get { return !Mathf.Approximately(0, SteerPercent); } }
    protected Vector3 InitialPos;

    public WheelCollider WheelCollider { get; protected set; }
    protected WheelHit Hit;
    protected CarController2 _carController;
    protected float CurrentRotateAngle;
    [Range(0, 1)]
    public float AntiRollBar;



    public float SuspensionPos { get; private set; } = 0;
    public float PrevSuspensionPos { get; private set; } = 0;
    public wheelsManager AntiRollWheel;
    private float stiffnessMultiplier = 2f;
    [HideInInspector] public float stiffnessupgrade= 0;


    private void Awake()
    {
        _carController = GetComponentInParent<CarController2>();
        WheelCollider = GetComponent<WheelCollider>();
        WheelCollider.ConfigureVehicleSubsteps(40, 100, 20);

    }



    private void FixedUpdate()
    {
       

        if (WheelCollider.GetGroundHit(out Hit))
        {
            //Calculation of the current friction.
            CurrentForwardSlip =  (CurrentForwardSlip + Mathf.Abs(Hit.forwardSlip)) / 2;
            CurrentSidewaysSlip = (CurrentSidewaysSlip + Mathf.Abs(Hit.sidewaysSlip)) / 2;

            HitPoint = Hit.point;

            ForwardSlipNormalized = CurrentForwardSlip / WheelCollider.forwardFriction.extremumSlip;
            SidewaysSlipNormalized = CurrentSidewaysSlip / WheelCollider.sidewaysFriction.extremumSlip;

            SlipNormalized = Mathf.Max(ForwardSlipNormalized, SidewaysSlipNormalized);
        }
        else
        {
            CurrentForwardSlip = 0;
            CurrentSidewaysSlip = 0;
            ForwardSlipNormalized = 0;
            SidewaysSlipNormalized = 0;
            SlipNormalized = 0;
        }

        AdjustStiffness();
        ApplyStiffness();
        ApplyBrake();
        ApplyAntiRollForce();
    }


    void AdjustStiffness()
    {
        WheelHit hit;
       if( WheelCollider.GetGroundHit(out hit))
        {
            PhysicMaterial surfaceMaterial = hit.collider.material;

            WheelFrictionCurve forwardFriction = WheelCollider.forwardFriction;
            WheelFrictionCurve sidewaysFriction = WheelCollider.sidewaysFriction;


            if (!_carController.AION)
            {
                forwardFriction.stiffness = (surfaceMaterial.dynamicFriction * stiffnessMultiplier) + stiffnessupgrade;
                sidewaysFriction.stiffness = (surfaceMaterial.dynamicFriction * stiffnessMultiplier) + stiffnessupgrade;
            }
            else {
                forwardFriction.stiffness = (surfaceMaterial.dynamicFriction * stiffnessMultiplier)+0.4f;
                sidewaysFriction.stiffness = (surfaceMaterial.dynamicFriction * stiffnessMultiplier)+0.4f;
            }

            WheelCollider.forwardFriction = forwardFriction;
            WheelCollider.sidewaysFriction = sidewaysFriction;

         

        }


    }

    void ApplyStiffness()
    {
        WheelFrictionCurve forwardFriction = WheelCollider.forwardFriction;
        float stiffness = forwardFriction.stiffness;
        var friction = WheelCollider.forwardFriction;
        friction.stiffness = stiffness;
        WheelCollider.forwardFriction = friction;

        friction = WheelCollider.sidewaysFriction;
        friction.stiffness = stiffness * Mathf.Lerp(0.3f, 1, Mathf.InverseLerp(2, 1, ForwardSlipNormalized));
        WheelCollider.sidewaysFriction = friction;
    }

    void ApplyBrake()
    {
        if (CurrentBrakeTorque > WheelCollider.brakeTorque)
        {
            WheelCollider.brakeTorque = Mathf.Lerp(WheelCollider.brakeTorque, CurrentBrakeTorque, BrakeSpeed * Time.fixedDeltaTime);
        }
        else
        {
            WheelCollider.brakeTorque = CurrentBrakeTorque;
        }
    }
    void ApplyAntiRollForce()
    {
        if (IsGrounded && AntiRollWheel)
        {
            float susDiff = (SuspensionPos - AntiRollWheel.SuspensionPos) * AntiRollBar * 3;
            WheelCollider.attachedRigidbody.AddForceAtPosition(transform.up * WheelCollider.attachedRigidbody.mass * susDiff, transform.position);
        }
    }

    public void SetMotorTorque(float motorTorque, bool forceSetTroque = false)
    {
        if (DriveWheel || forceSetTroque)
        {
            WheelCollider.motorTorque = motorTorque;
        }
    }

    public void SetSteerAngle(float steerAngle)
    {
        if (IsSteeringWheel)
        {
            WheelCollider.steerAngle = steerAngle * SteerPercent;
        }
    }

    public void SetHandBrake(bool handBrake)
    {
        if (HandBrakeWheel && handBrake)
        {
            CurrentBrakeTorque = MaxBrakeTorque;
        }
        else
        {
            CurrentBrakeTorque = 0;
        }
    }

    public void SetBrakeTorque(float brakeTorque)
    {
        CurrentBrakeTorque = brakeTorque * MaxBrakeTorque;
    }
}
