using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CarController2 : MonoBehaviour
{
    internal enum driver
    {
        AI,
        AIML,
        Human
    }
    [SerializeField] driver driveController;

    public SpawnPointManager _spawnPointManager;
    public int laps = 0;
    public bool human;
    public bool halfpointtrigger = false;
    public bool AION = false;
    public AIInput aIInput;
    private CarAgent carAgent;
    public PlayerCarInput carinput;
    [Header("Car")]

    private GameObject wheelMeshes;
    private GameObject wheelColliders;
    private GameObject[] wheelMesh = new GameObject[4];
    private wheelsManager[] wheels = new wheelsManager[4];
    public float throttleinputt, breakinpuu, steerin;
    public bool handbrakeinnn;
    public Transform _centerOfMass;
    public string carID;
    public Rigidbody playerRB { get; private set; }
    public bool IsPlayerVehicle { get; set; }
    public bool VehicleIsGrounded { get; private set; }
    public float CurrentSpeed { get; private set; }                                     //Vehicle speed, measured in "units per second".
    public int VehicleDirection { get { return CurrentSpeed < 1 ? 0 : (Mathf.Abs(VelocityAngle) < 90 ? 1 : -1); } }
    public float VelocityAngle { get; private set; }                                    //The angle of the vehicle body relative to the Velocity vector.
    public float PrevVelocityAngle { get; private set; }

    public float KPH;


    [Space(10)]


    [Header("Transmition")]
    public GearboxConfig Gearbox;
  //  public event System.Action<int> OnChangeGearAction;
    int _CurrentGear;
    public int CurrentGear              //Current gear, starting at -1 for reverse gear: -1 - reverse, 0 - neutral, 1 - 1st gear, etc.
    {
        get { return _CurrentGear; }

        set
        {
            if (_CurrentGear != value)
            {
                _CurrentGear = value;
               // OnChangeGearAction.SafeInvoke(_CurrentGear);
            }
        }
    }
    public int CurrentGearIndex { get { return CurrentGear + 1; } }     //Current gear index, starting at 0 for reverse gear: 0 - reverse, 1 - neutral, 2 - 1st gear, etc.
    public bool InChangeGear { get { return ChangeGearTimer > 0; } }

    float ChangeGearTimer = 0;
    float[] AllGearsRatio;
    wheelsManager[] DriveWheels = new wheelsManager[2];

    
    [Space(10)]


    [Header("Engine")]

    public EngineConfig Engine;
    public bool StartEngineInAwake = false;
    public float StartEngineDellay = 0.5f;
    public float CurrentMotorTorque
    {
        get
        {
            return
                 Engine.MotorTorqueFromRpmCurve.Evaluate(EngineRPM * 0.001f) *
                    (Steer.TCS > 0 ? TCSMultiplayer : 1) *
                    (Engine.SpeedLimit > 0 ? Mathf.InverseLerp(Engine.SpeedLimit, Engine.SpeedLimit * 0.5f, CurrentSpeed) : 1)
                    ;
        }
    }
    public bool isEngineRunning;
    public float EngineRPM { get; private set; }            //Current RPM.
    public float TargetRPM { get; private set; }            //TargetRPM Calculated based on the drive wheel rpm and the current gear ratio
    public float EngineLoad { get; private set; }           //Current Load

    public float MaxRPM { get { return Engine.MaxRPM; } }
    public float MinRPM { get { return Engine.MinRPM; } }

    float MaxMotorTorque;
    float CutOffTimer;
    bool InCutOff;

    public float CurrentAcceleration { get; private set; }
    public bool InHandBrake
    {
        get
        {
            switch (driveController)
            {
                case driver.AI:
                    AION = true;
                    return aIInput != null ? aIInput.handBrakeInput : false;
                case driver.AIML:
                    return carAgent != null ? carAgent.handBrakeInput : false;
                case driver.Human:
                    return carinput != null ? carinput.handBrakeInput : false;
                default: return false;
            }
        }
    }
    public float CurrentBrake { get; private set; }

    public float TCSMultiplayer { get; private set; } = 1;
    Coroutine StartEngineCoroutine;



    [Space(10)]
    [Header("Steer")]

    public SteerConfig Steer;

    protected float PrevSteerAngle;
    protected float CurrentSteerAngle;
    protected float WheelMaxSteerAngle;
    protected wheelsManager[] SteeringWheels;
    protected float HorizontalControl
    {
        get
        {
            switch (driveController)
            {
                case driver.AI:
                    return aIInput != null ? aIInput.steeringInput : 0;
                case driver.AIML:
                    return carAgent != null ? carAgent.steeringInput : 0;
                case driver.Human:
                    return carinput != null ? carinput.steeringInput : 0;
                default: return 0;
            }
        }
    }
    public bool ABSIsActive { get; private set; }
    [Space(10)]

    [Header("AI")]

    [SerializeField] public Transform carAItarget;
    
    
    private GameObject carAItargetObj;
    public bool persuitAiOn;
    public GameObject persuitTarget;
    public float persuitDistance;



    private void Awake()
    {

        _spawnPointManager = FindObjectOfType<SpawnPointManager>();


        carID = "Car_" + GetInstanceID();
        carinput = GetComponent<PlayerCarInput>();
        aIInput = GetComponent<AIInput>();
        carAgent = GetComponent<CarAgent>();
        AwakeCar();
        AwakeTransmition();
        AwakeEngine();
        AwakeSteer();
        carAItargetObj = new GameObject("WaypointsTarget");
        carAItarget = carAItargetObj.transform;


    }


    private void AwakeCar()
    {
        playerRB = GetComponent<Rigidbody>();
        _centerOfMass = transform.Find("COM");
        playerRB.centerOfMass = _centerOfMass.transform.localPosition;




        wheelMeshes = transform.Find("wheelMeshes").gameObject;
        wheelColliders = transform.Find("wheelColliders").gameObject;
       wheels[0] = wheelColliders.transform.Find("0").gameObject.GetComponent<wheelsManager>();
        wheels[1] = wheelColliders.transform.Find("1").gameObject.GetComponent<wheelsManager>();
        wheels[2] = wheelColliders.transform.Find("2").gameObject.GetComponent<wheelsManager>();
        wheels[3] = wheelColliders.transform.Find("3").gameObject.GetComponent<wheelsManager>();

        wheelMesh[0] = wheelMeshes.transform.Find("0").gameObject;
        wheelMesh[1] = wheelMeshes.transform.Find("1").gameObject;
        wheelMesh[2] = wheelMeshes.transform.Find("2").gameObject;
        wheelMesh[3] = wheelMeshes.transform.Find("3").gameObject;


    }

    void AwakeTransmition()
    {
        AllGearsRatio = new float[Gearbox.GearsRatio.Length + 2];
        AllGearsRatio[0] = -Gearbox.ReversGearRatio * Gearbox.MainRatio;
        AllGearsRatio[1] = 0;
        for (int i = 0; i < Gearbox.GearsRatio.Length; i++)
        {
            AllGearsRatio[i + 2] = Gearbox.GearsRatio[i] * Gearbox.MainRatio;
        }

        var driveWheels = new List<wheelsManager>();
        foreach (var wheel in wheels)
        {
            if (wheel.DriveWheel)
            {
                driveWheels.Add(wheel);
            }
        }

        DriveWheels = driveWheels.ToArray();
    }

    void AwakeEngine()
    {
        MaxMotorTorque = Engine.MaxMotorTorque / DriveWheels.Length;

    }

    void AwakeSteer()
    {
        var steeringWheels = new List<wheelsManager>();
        foreach (var wheel in wheels)
        {
            if(wheel.IsSteeringWheel)
            {
                steeringWheels.Add(wheel);
                if(Mathf.Abs(wheel.SteerPercent)> WheelMaxSteerAngle)
                {
                    WheelMaxSteerAngle = Mathf.Abs(wheel.SteerPercent);
                }
            }
        }
        SteeringWheels = steeringWheels.ToArray();

    }
   

    private void FixedUpdate()
    {
        throttleinputt = CurrentAcceleration;
        breakinpuu = CurrentBrake;
        handbrakeinnn = InHandBrake;
        steerin = HorizontalControl;

        UpdateCar();
        UpdateEngine();
        UpdateTransmission();
        UpdateBrake();
        UpdateSteer();
        ApplyWheelPositions();
    }
    public void UpdateCar()
    {
        CurrentSpeed = playerRB.velocity.magnitude;
        KPH = CurrentSpeed * 3.6f;
        PrevVelocityAngle = VelocityAngle;

        Vector3 horizontalLocalVelocity = transform.InverseTransformDirection(playerRB.velocity);
        horizontalLocalVelocity.y = 0;
        if (horizontalLocalVelocity.sqrMagnitude > 0.01f)
        {
            VelocityAngle = -Vector3.SignedAngle(horizontalLocalVelocity.normalized, Vector3.forward, Vector3.up);
        }
        else
        {
            VelocityAngle = 0;
        }

        VehicleIsGrounded = false;

        for (int i = 0; i < wheels.Length; i++)
        {
            VehicleIsGrounded |= wheels[i].IsGrounded;
        }
    }
    public void UpdateEngine()
    {
        if (!isEngineRunning)
        {
            return;
        }
        switch (driveController)
        {
            case driver.AI:
                if (aIInput == null)
                {
                    CurrentAcceleration = 0;
                    CurrentBrake = 0;
                }
                else if (!Gearbox.AutomaticGearBox || CurrentGear >= 0)
                {
                    CurrentAcceleration = aIInput.throttleInput;
                    CurrentBrake = aIInput.BrakeReverse;
                }
                else if (CurrentGear < 0)
                {
                    CurrentAcceleration = aIInput.BrakeReverse;
                    CurrentBrake = aIInput.throttleInput;
                }
                break;
            case driver.AIML:
                if (carAgent == null)
                {
                    CurrentAcceleration = 0;
                    CurrentBrake = 0;
                }
                else if (!Gearbox.AutomaticGearBox || CurrentGear >= 0)
                {
                    CurrentAcceleration = carAgent.throttleInput;
                    CurrentBrake = carAgent.BrakeReverse;
                }
                else if (CurrentGear < 0)
                {
                    CurrentAcceleration = carAgent.BrakeReverse;
                    CurrentBrake = carAgent.throttleInput;
                }
                break;


            case driver.Human:
                
                if (carinput == null)
                {
                    CurrentAcceleration = 0;
                    CurrentBrake = 0;
                }
                else if (!Gearbox.AutomaticGearBox || CurrentGear >= 0)
                {
                    CurrentAcceleration = carinput.throttleInput;
                    CurrentBrake = carinput.BrakeReverse;
                }
                else if (CurrentGear < 0)
                {
                    CurrentAcceleration = carinput.BrakeReverse;
                    CurrentBrake = carinput.throttleInput;
                }
                break;

        }

        //TCS Logic
        if (Steer.TCS > 0 && CurrentAcceleration > 0.1f)
        {
            float avgForwardFriction = 0;
            for (int i = 0; i < DriveWheels.Length; i++)
            {
                avgForwardFriction += DriveWheels[i].ForwardSlipNormalized;
            }
            avgForwardFriction /= DriveWheels.Length;

            TCSMultiplayer = Mathf.InverseLerp(2f, 1.5f, avgForwardFriction + Steer.TCS * 0.5f);
        }
        else
        {
            TCSMultiplayer = 1;
        }

        if (InCutOff)
        {
            if (CutOffTimer > 0)
            {
                CutOffTimer -= Time.fixedDeltaTime;
                EngineRPM = Mathf.Lerp(EngineRPM, Engine.TargetCutOffRPM, Engine.RPMEngineToRPMWheelsFast * Time.fixedDeltaTime);
            }
            else
            {
                EngineRPM = Engine.TargetCutOffRPM;
                InCutOff = false;
            }
        }

        float avgRPM = 0;
        int enabledWheelsCount = 0;
        for (int i = 0; i < DriveWheels.Length; i++)
        {
            if (DriveWheels[i].enabled)
            {
                avgRPM += DriveWheels[i].RPM;
                enabledWheelsCount++;
            }
        }

        if (enabledWheelsCount > 0)
        {
            avgRPM /= enabledWheelsCount;
        }
        else
        {
            avgRPM = Engine.MinRPM;
        }

        EngineLoad = 0;

        if (!InCutOff)
        {
            //Calculation of the current engine rpm.
            if (!Gearbox.HasRGear && CurrentGear == -1)
            {
                TargetRPM = 0;
            }
            else
            {
                TargetRPM = (avgRPM * CurrentGear) <= 0 && !InHandBrake ? ((EngineRPM + 1000) * CurrentAcceleration) : (Mathf.Abs(avgRPM) * Mathf.Abs(AllGearsRatio[CurrentGearIndex]));
            }

            TargetRPM = Mathf.Clamp (TargetRPM, MinRPM, MaxRPM);
            var changeRPMSpeed = Mathf.Abs(CurrentAcceleration) > 0.1f && TargetRPM > EngineRPM ? Engine.RPMEngineToRPMWheelsFast : Engine.RPMEngineToRPMWheelsSlow;

            //Calculation of the current engine load.
            EngineLoad = Mathf.Clamp((TargetRPM - EngineRPM), -300, 300) / 300 * CurrentAcceleration;

            EngineRPM = Mathf.Lerp(EngineRPM, TargetRPM, changeRPMSpeed * Time.fixedDeltaTime);
        }

        //Check CutOff.
        if (EngineRPM >= Engine.CutOffRPM)
        {
      
            InCutOff = true;
            CutOffTimer = Engine.CutOffTime;
        }

    }
    public void UpdateTransmission()
    {

        if (!Mathf.Approximately(CurrentAcceleration, 0) && (Gearbox.HasRGear || CurrentGear >= 0))
        {
            var motorTorque = CurrentAcceleration * (CurrentMotorTorque* (MaxMotorTorque * AllGearsRatio[CurrentGearIndex]));

            if (InChangeGear)
            {
                motorTorque = 0;
            }

            //Calculation of target rpm for driving wheels.
            var targetWheelsRPM = AllGearsRatio[CurrentGearIndex] == 0 ? 0 : EngineRPM / AllGearsRatio[CurrentGearIndex];
            var offset =  Mathf.Abs(400 / AllGearsRatio[CurrentGearIndex]);

            for (int i = 0; i < DriveWheels.Length; i++)
            {
                var wheel = DriveWheels[i];
                var wheelTorque = motorTorque;

                //The torque transmitted to the wheels depends on the difference between the target RPM and the current RPM. 
                //If the current RPM is greater than the target RPM, the wheel will brake. 
                //If the current RPM is less than the target RPM, the wheel will accelerate.

                if (targetWheelsRPM != 0 && Mathf.Sign(targetWheelsRPM * wheel.RPM) > 0)
                {
                    var multiplier = Mathf.Abs(wheel.RPM) / (Mathf.Abs(targetWheelsRPM) + offset);
                    if (multiplier >= 1f)
                    {
                        wheelTorque *= (1 - multiplier);
                    }
                }

                //Apply of torque to the wheel.
                wheel.SetMotorTorque(wheelTorque);
            }
        }
        else
        {
            for (int i = 0; i < DriveWheels.Length; i++)
            {
                DriveWheels[i].SetMotorTorque(0);
            }
        }

        if (InChangeGear)
        {
            ChangeGearTimer -= Time.fixedDeltaTime;
        }


        //Automatic gearbox logic. 
        if (!InChangeGear && Gearbox.AutomaticGearBox)
        {

            bool forwardIsSlip = false;
            bool anyWheelIsGrounded = false;
            float avgSign = 0;
            for (int i = 0; i < DriveWheels.Length; i++)
            {
                forwardIsSlip |= DriveWheels[i].ForwardSlipNormalized > 0.9f;
                anyWheelIsGrounded |= DriveWheels[i].IsGrounded;
                avgSign += DriveWheels[i].RPM;
            }

            avgSign = Mathf.Sign(avgSign);

            if (anyWheelIsGrounded && !forwardIsSlip && EngineRPM > Engine.RPMToNextGear && CurrentGear >= 0 && CurrentGear < (AllGearsRatio.Length - 2))
            {
                NextGear();
            }
            else if (CurrentGear > 0 && (EngineRPM + 10 <= MinRPM || CurrentGear != 1) &&
                Engine.RPMToNextGear > EngineRPM / AllGearsRatio[CurrentGearIndex] * AllGearsRatio[CurrentGearIndex - 1] + Engine.RPMToPrevGearDiff)
            {
                PrevGear();
            }

            //Switching logic from neutral gear.
            if (CurrentGear == 0 && CurrentBrake > 0)
            {
                CurrentGear = -1;
            }
            else if (CurrentGear == 0 && CurrentAcceleration > 0)
            {
                CurrentGear = 1;
            }
            else if ((avgSign > 0 && CurrentGear < 0 || VehicleDirection == 0) && Mathf.Approximately(CurrentAcceleration, 0))
            {
                CurrentGear = 0;
            }
        }
    }
    public void UpdateSteer()
    {
        var needHelp = Mathf.Abs(VelocityAngle) > 0.001f && Mathf.Abs(VelocityAngle) < Steer.MaxVelocityAngleForHelp && CurrentSpeed > Steer.MinSpeedForHelp && CurrentGear > 0;
        float helpAngle = 0;
        var angularVelocity = playerRB.angularVelocity;

        if (needHelp)
        {
            for (int i = 0; i < SteeringWheels.Length; i++)
            {
                if (wheels[i].IsGrounded)
                {
                    HelpAngularVelocity();
                    break;
                }
            }

            helpAngle = Mathf.Clamp(VelocityAngle * Steer.HelpDriftIntensity, -Steer.MaxSteerAngle, Steer.MaxSteerAngle);
        }
        else if (CurrentSpeed < Steer.MinSpeedForHelp && CurrentAcceleration > 0 && CurrentBrake > 0)
        {
            angularVelocity.y += Steer.HandBrakeAngularHelpCurve.Evaluate(Mathf.Abs(angularVelocity.y)) * HorizontalControl * 5 * Time.fixedDeltaTime;
            playerRB.angularVelocity = angularVelocity;
        }

        float helpWhenChangeAngle = VehicleDirection == 1 ? (VelocityAngle - PrevVelocityAngle) * (Steer.MaxSteerAngle / 90) : 0;

        var steerMultiplayer = Steer.EnableSteerLimit && VehicleDirection > 0 ? Steer.SteerLimitCurve.Evaluate(CurrentSpeed) : 1;

        float targetSteerAngle = HorizontalControl * Steer.MaxSteerAngle * steerMultiplayer;

        //Wheel turn limitation.
        var targetAngle = Mathf.Clamp(helpAngle + targetSteerAngle, -Steer.MaxSteerAngle, Steer.MaxSteerAngle);

        //Calculation of the steering speed. The steering wheel should turn faster towards the velocity angle.
        //More details (With images) are described in the documentation.
        float steerAngleChangeSpeed;

        float currentAngleDiff = (VelocityAngle - Mathf.Abs(CurrentSteerAngle));

        if (!needHelp || PrevSteerAngle > CurrentSteerAngle && CurrentSteerAngle > VelocityAngle || PrevSteerAngle < CurrentSteerAngle && CurrentSteerAngle < VelocityAngle)
        {
            steerAngleChangeSpeed = Steer.SteerChangeSpeedToVelocity.Evaluate(currentAngleDiff);
        }
        else
        {
            steerAngleChangeSpeed = Steer.SteerChangeSpeedFromVelocity.Evaluate(currentAngleDiff);
        }

        PrevSteerAngle = CurrentSteerAngle;
        CurrentSteerAngle = Mathf.MoveTowards(CurrentSteerAngle, targetAngle, steerAngleChangeSpeed * steerMultiplayer * Time.fixedDeltaTime);
        CurrentSteerAngle = Mathf.Clamp((CurrentSteerAngle + helpWhenChangeAngle),-Steer.MaxSteerAngle, Steer.MaxSteerAngle);

        //Apply a turn to the front wheels.
        for (int i = 0; i < SteeringWheels.Length; i++)
        {
            SteeringWheels[i].SetSteerAngle(CurrentSteerAngle);
        }

    }
    public void UpdateBrake()
    {
        ABSIsActive = false;
        //HandBrake
        if (InHandBrake)
        {
            for (int i = 0; i < wheels.Length; i++)
            {
                wheels[i].SetHandBrake(true);
            }
        }
        //Brake and acceleration pressed at the same time for burnout.
        else if (CurrentAcceleration > 0 && CurrentBrake > 0 && CurrentSpeed < 5)
        {
            for (int i = 0; i < wheels.Length; i++)
            {
                wheels[i].SetBrakeTorque(wheels[i].DriveWheel ? 0 : CurrentBrake);
            }
        }
        //Just braking.
        else
        {
            if (Steer.ABS > 0 && CurrentBrake > 0)
            {
                //ABS Logic.
                float maxSlipForEnableAbs = 2.8f - Steer.ABS * 1.2f;
                for (int i = 0; i < wheels.Length; i++)
                {
                    if (wheels[i].ForwardSlipNormalized > maxSlipForEnableAbs)
                    {
                        wheels[i].SetBrakeTorque(0);
                        ABSIsActive |= true;
                    }
                    else
                    {
                        wheels[i].SetBrakeTorque(CurrentBrake);
                    }
                }
            }
            else
            {
                //Without ABS Logic.
                for (int i = 0; i < wheels.Length; i++)
                {
                    wheels[i].SetBrakeTorque(CurrentBrake);
                }
            }
        }
    }
    public void HelpAngularVelocity()
    {
        var angularVelocity = playerRB.angularVelocity;

        float angularHelp = 0;
        float angularHelpMultiplier = HorizontalControl * Mathf.Clamp01(CurrentSpeed / Steer.MaxSpeedForMaxAngularHelp) * Time.fixedDeltaTime;


        if (HorizontalControl * VelocityAngle >= 0 && Mathf.Abs(VelocityAngle) < 150)
        {
            //Drift resistance help.
            angularHelp = Steer.DriftResistanceCurve.Evaluate(VelocityAngle < 0 ? playerRB.angularVelocity.y : -playerRB.angularVelocity.y) * Mathf.Clamp01(Mathf.Abs(VelocityAngle) / 60) * angularHelpMultiplier;
        }

        if (InHandBrake)
        {
            //Handbrake resistance help.
            angularHelp += Steer.HandBrakeAngularHelpCurve.Evaluate(angularVelocity.y * -Mathf.Sign(angularVelocity.y)) * angularHelpMultiplier;
        }

        if (Steer.DriftLimitAngle > 0)
        {
            if ((-VelocityAngle * playerRB.angularVelocity.y) > 0)
            {
                float groundedWheels = 0;
                for (int i = 0; i < wheels.Length; i++)
                {
                    if (wheels[i].IsGrounded)
                    {
                        groundedWheels++;
                    }
                }
                float limitMultiplier = Mathf.InverseLerp(Steer.DriftLimitAngle, Steer.DriftLimitAngle * 0.5f, Mathf.Abs(VelocityAngle));
                angularVelocity.y = Mathf.Lerp(angularVelocity.y, angularVelocity.y * limitMultiplier, (groundedWheels / wheels.Length) * Time.fixedDeltaTime * 10);
            }
        }

        angularVelocity.y += angularHelp;
        playerRB.angularVelocity = angularVelocity;
    }
    public void NextGear()
    {
        if (!InChangeGear && CurrentGear < (AllGearsRatio.Length - 2))
        {
            CurrentGear++;
            ChangeGearTimer = Gearbox.ChangeUpGearTime;
           // PlayBackfireWithProbability();
        }
    }
    public void PrevGear()
    {
        if (!InChangeGear && CurrentGear >= 0)
        {
            CurrentGear--;
            ChangeGearTimer = Gearbox.ChangeDownGearTime;
        }
    }
    public void StartEngine()
    {
        if (StartEngineCoroutine == null)
        {
            StartEngineCoroutine = StartCoroutine(DoStartEngine());
        }
    }
    IEnumerator DoStartEngine()
    {
        //OnStartEngineAction.SafeInvoke(StartEngineDellay);

        float timer = 0;
        EngineRPM = 0;
        while (timer < StartEngineDellay)
        {
            yield return null;
            timer += Time.deltaTime;
            EngineRPM = Mathf.Lerp(0, MinRPM, Mathf.Pow(Mathf.InverseLerp(0, StartEngineDellay, timer), 2));
        }

        EngineRPM = MinRPM;

        StartEngineCoroutine = null;
    }

    public void ApplyWheelPositions()
    {
        Vector3 wheelPosition = Vector3.zero;
        Quaternion wheelRotation = Quaternion.identity;

        for (int i = 0; i < 4; i++)
        {
            wheels[i].WheelCollider.GetWorldPose(out wheelPosition, out wheelRotation);
            wheelMesh[i].transform.position = wheelPosition;
            wheelMesh[i].transform.rotation = wheelRotation;
        }
    }

    public void Respawn()
    {
        Vector3 pos = _spawnPointManager.SelectRandomSpawnpoint();
        transform.rotation = Quaternion.LookRotation(Vector3.forward);

        transform.position = pos - new Vector3(0, 0.4f, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HPT"))
        {
            halfpointtrigger = true;
        }

        if (other.CompareTag("FINISHLINE") && halfpointtrigger == true)
        {
            laps++;
            halfpointtrigger = false;


            switch (driveController)
            {
                case driver.Human:
                    human = true;
                    break;
            }
        }
    }



    [System.Serializable]
    public class GearboxConfig
    {
        public float ChangeUpGearTime = 0.3f;                   //Delay after upshift.
        public float ChangeDownGearTime = 0.2f;                 //Delay after downshift.

        [Header("Automatic gearbox")]
        public bool AutomaticGearBox = true;

        [Header("Ratio")]
        public float[] GearsRatio;                              //Gear ratio. The values ​​are best take from the technical data of real transmissions.
        public float MainRatio;
        public bool HasRGear = true;
        public float ReversGearRatio;
    }

    [System.Serializable]
    public class EngineConfig
    {
        [Header("Power")]
        public float MaxMotorTorque = 150;                  //Maximum torque, reached at 1 value(y) of MotorTorqueFromRpmCurve.
        public AnimationCurve MotorTorqueFromRpmCurve;
        public float MaxRPM = 7000;
        public float MinRPM = 700;
        public float RPMEngineToRPMWheelsFast = 15;         //Rpm change with increasing speed.
        public float RPMEngineToRPMWheelsSlow = 4;          //Rpm change with decreasing speed.
        public float SpeedLimit = 0;
        public float finalDrive = 3.7f;

        [Header("Cut off")]
        public float CutOffRPM = 6800;                      //The rpm at which the cut-off is triggered.
        public float TargetCutOffRPM = 6400;                //The value to which the rpm fall.
        public float CutOffTime = 0.1f;                     //The time for which the rpm fall during the cut-off.

        //Automatic gear shifting is in EngineConfig because the maximum number of rpm can be different for the motors, and the gearbox can be the same.
        [Header("Automatic change gear")]
        public float RPMToNextGear = 6500;
        public float RPMToPrevGearDiff = 500;
    }
    [System.Serializable]
    public class SteerConfig
    {
        [Header("Steer settings")]
        public float MaxSteerAngle = 25;
        public bool EnableSteerLimit = true;                    //Enables limiting wheel turning based on car speed.
        public AnimationCurve SteerLimitCurve;                  //Limiting wheel turning if the EnableSteerLimit flag is enabled
        public AnimationCurve SteerChangeSpeedToVelocity;       //The speed of turn of the wheel in the direction of the velocity of the car.
        public AnimationCurve SteerChangeSpeedFromVelocity;     //The speed of turn of the wheel from the direction of the velocity of the car.

        [Header("Steer assistance")]
        public float MaxVelocityAngleForHelp = 120;             //The maximum degree of angle of the car relative to the velocity, at which the steering assistance will be provided.
        public float MinSpeedForHelp = 1.5f;

        [Space(10)]
        [Range(0, 1)] public float HelpDriftIntensity = 0.8f;   //The intensity of the automatic steering while drifting.

        [Header("Angular help")]
        public AnimationCurve HandBrakeAngularHelpCurve;        //The power of assistance that turns the car with the hand brake.
        public AnimationCurve DriftResistanceCurve;
        public float MaxSpeedForMaxAngularHelp = 20;
        public float DriftLimitAngle = 0;

        [Header("Electronic assistants")]
        [Range(0, 1)]
        public float ABS;               //ABS to prevent wheel lock when braking.
                                        //[Range(0, 1)]
                                        //public float ESP;             TODO add ESP logic
        [Range(0, 1)]
        public float TCS;               //TCS to prevent wheel slip when accelerating.
    }

}


