using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class AIInput : MonoBehaviour
{

    public float desiredSpeed;
    public float brakeinp=0;
    public float accel;
    private CarController2 AICarController;

    private Transform frontSensor;
    private Transform rightSensor;
    private Transform leftSensor;


    private float randomValue;
    private float avoidOtherCarTime;
    private float avoidOtherCarSlowdown;
    private float avoidPathOffset;

    [HideInInspector]
    public bool reverseGearOn = false;
    [HideInInspector]
    public bool persuitAiOn = false;
    private bool avoidingObstacle = false;
    private bool isBraking = false;

    public float avoidObstacleMultiplier;

    public float targetAngle;
    public float throttleInput { get; private set; }
    public float steeringInput { get; private set; }

    public float BrakeReverse { get; private set; }

    public bool handBrakeInput { get; private set; }

    public enum BrakeCondition
    {
        NeverBrake,
        TargetDirectionDifference,
        TargetDistance,
    }
    [SerializeField] BrakeCondition brakeCondition;

    public enum ProgressStyle
    {
        SmoothAlongRoute,
        PointToPoint
    }

    [SerializeField][Range(0, 1)] public float cautiousSpeedFactor = 0.05f;
    [SerializeField][Range(0, 180)] public float cautiousAngle = 50f;
    [SerializeField][Range(0, 200)] public float cautiousDistance = 100f;
    [SerializeField] public float cautiousAngularVelocityFactor = 30f;
    [SerializeField][Range(0, 0.1f)] public float steerSensitivity = 0.05f;
    [SerializeField][Range(0, 0.1f)] public float accelSensitivity = 0.04f;
    [SerializeField][Range(0, 1)] public float brakeSensitivity = 1f;
    [SerializeField][Range(0, 10)] public float lateralWander = 3f;
    [SerializeField] public float lateralWanderSpeed = 0.5f;
    [SerializeField][Range(0, 1)] public float wanderAmount = 0.1f;
    [SerializeField] public float accelWanderSpeed = 0.1f;
    [SerializeField] public bool stopWhenTargetReached;
    [SerializeField] public float reachTargetThreshold = 2;


    [SerializeField][Range(15, 50)] public float sensorsAngle;
    [SerializeField] public float avoidDistance = 10;
    [SerializeField] public float brakeDistance = 6;
    [SerializeField] public float reverseDistance = 3;


    [SerializeField] public ProgressStyle progressStyle = ProgressStyle.SmoothAlongRoute;
    [SerializeField] public WaypointPath AIcircuit;
    [SerializeField][Range(5, 50)] public float lookAheadForTarget = 5;
    [SerializeField] public float lookAheadForTargetFactor = .1f;
    [SerializeField] public float lookAheadForSpeedOffset = 10;
    [SerializeField] public float lookAheadForSpeedFactor = .2f;
    [SerializeField][Range(1, 10)] public float pointThreshold = 4;
    public Transform AItarget;
    private void Awake()
    {


        AICarController = GetComponent<CarController2>();
        frontSensor = this.transform.GetChild(0).GetChild(0).GetChild(0);
        rightSensor = this.transform.GetChild(0).GetChild(0).GetChild(1);
        leftSensor = this.transform.GetChild(0).GetChild(0).GetChild(2);

        persuitAiOn = AICarController.persuitAiOn;



        randomValue = Random.value * 100;
    }


    private void FixedUpdate()
    {
        if (AICarController.carAItarget == null || !AICarController.isEngineRunning)
        {
            if (AICarController.persuitTarget != null && AICarController.persuitAiOn)
            {
                PersuitSensors();
            }
            else
            {
                throttleInput = 0;
                steeringInput = 0;
                BrakeReverse = 0;
                handBrakeInput = true;

            }
        }
        else
        {
            if (AICarController.persuitAiOn && AICarController.persuitTarget != null)
            {
                PersuitSensors();
            }
            else if (!AICarController.persuitAiOn)
            {
                persuitAiOn = false;
            }

            ReverseGearSensors();
            if (!reverseGearOn)
            {
                AvoidSensors();
                BrakeSensors();
            }

            Vector3 fwd = transform.forward;
            if (AICarController.playerRB.velocity.magnitude > AICarController.Engine.SpeedLimit * 0.1f)
            {
                fwd = AICarController.playerRB.velocity;
            }

            desiredSpeed = AICarController.Engine.SpeedLimit;

            
            switch (brakeCondition)
            {
                case BrakeCondition.TargetDirectionDifference:
                    {
                        float approachingCornerAngle = Vector3.Angle(AICarController.carAItarget.forward, fwd);
                        float spinningAngle = AICarController.playerRB.angularVelocity.magnitude * cautiousAngularVelocityFactor;
                        float cautiousnessRequired = Mathf.InverseLerp(0, cautiousAngle, Mathf.Max(spinningAngle, approachingCornerAngle));
                        desiredSpeed = Mathf.Lerp(AICarController.Engine.SpeedLimit, AICarController.Engine.SpeedLimit * cautiousSpeedFactor, cautiousnessRequired);
                        break;
                    }

                case BrakeCondition.TargetDistance:
                    {
                        Vector3 delta = AICarController.carAItarget.position - transform.position;
                        float distanceCautiousFactor = Mathf.InverseLerp(cautiousDistance, 0, delta.magnitude);
                        float spinningAngle = AICarController.playerRB.angularVelocity.magnitude * cautiousAngularVelocityFactor;
                        float cautiousnessRequired = Mathf.Max(Mathf.InverseLerp(0, cautiousAngle, spinningAngle), distanceCautiousFactor);
                        desiredSpeed = Mathf.Lerp(AICarController.Engine.SpeedLimit, AICarController.Engine.SpeedLimit * cautiousSpeedFactor, cautiousnessRequired);
                        break;
                    }

                case BrakeCondition.NeverBrake:
                    break;
            }

          

           
            Vector3 offsetTargetPos = AICarController.carAItarget.position;

            if (Time.time < avoidOtherCarTime)
            {
                desiredSpeed *= avoidOtherCarSlowdown;
                offsetTargetPos += AICarController.carAItarget.right * avoidPathOffset;
            }
            else
            {
                offsetTargetPos += AICarController.carAItarget.right * (Mathf.PerlinNoise(Time.time * lateralWanderSpeed, randomValue) * 2 - 1) * lateralWander;
            }

        

     

            float accelBrakeSensitivity = (desiredSpeed < AICarController.CurrentSpeed)
                                              ? brakeSensitivity
                                              : accelSensitivity;


             accel = Mathf.Clamp((desiredSpeed - AICarController.CurrentSpeed) * accelBrakeSensitivity, -1, 1);
            float breakinp = 0;

       

            accel *= wanderAmount + (Mathf.PerlinNoise(Time.time * accelWanderSpeed, randomValue) * wanderAmount);

            Vector3 localTarget;

            localTarget = transform.InverseTransformPoint(offsetTargetPos);

            if (avoidingObstacle)
            {
                targetAngle = AICarController.Steer.MaxSteerAngle * (avoidObstacleMultiplier*0.2f);
            }
            else if (AICarController.persuitAiOn)
            {
                if (AICarController.persuitTarget != null)
                {
                    Transform tempTarget = AICarController.persuitTarget.GetComponentInChildren<MeshCollider>().transform;
                    Vector3 relativeVector = transform.InverseTransformPoint(tempTarget.position);
                    targetAngle = (relativeVector.x / relativeVector.magnitude) * AICarController.Steer.MaxSteerAngle;
                }
            }
            else
            {
                targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;
            }

            float steer = Mathf.Clamp(targetAngle * steerSensitivity, -1, 1) * Mathf.Sign(AICarController.CurrentSpeed);

            

            if (accel < 0) brakeinp = Mathf.Abs(accel);


            if (isBraking)
            {
                throttleInput = 0;
                steeringInput = steer;
                BrakeReverse = 1;
                handBrakeInput = false;
                
            }
            else if (reverseGearOn)
            {
                throttleInput = 0;
                steeringInput = -steer;
                BrakeReverse = 1;
                handBrakeInput = false;
                
            }
            else
            {
                if(accel > 0 )
                {
                    throttleInput = accel;
                    steeringInput = steer;
                    BrakeReverse = 0;
                    handBrakeInput = false;
                }
                else
                {
                    throttleInput = 0;
                    steeringInput = steer;
                    BrakeReverse = brakeinp;
                    handBrakeInput = false;
                }
                
                
            }

     

            if (stopWhenTargetReached && localTarget.magnitude < reachTargetThreshold)
            {
                AICarController.isEngineRunning = false;
            }

       
        }
    }


    private void OnCollisionStay(Collision col)
    {
        if (col.rigidbody != null)
        {
            var otherAI = col.rigidbody.GetComponent<AIInput>();
            if (otherAI != null)
            {
                avoidOtherCarTime = Time.time + 1;

                if (Vector3.Angle(transform.forward, otherAI.transform.position - transform.position) < 90)
                {
                    avoidOtherCarSlowdown = 0.5f;
                }
                else
                {
                    avoidOtherCarSlowdown = 1;
                }

                var otherCarLocalDelta = transform.InverseTransformPoint(otherAI.transform.position);
                float otherCarAngle = Mathf.Atan2(otherCarLocalDelta.x, otherCarLocalDelta.z);
                avoidPathOffset = lateralWander * -Mathf.Sign(otherCarAngle);
            }
        }
    }


    private void AvoidSensors()
    {
        RaycastHit hit;
        avoidingObstacle = false;

        // Right Sensor
        if (Physics.Raycast(rightSensor.position, frontSensor.transform.forward, out hit, avoidDistance))
        {
            if (!hit.collider.GetComponent<CompetitiveDrivingCheck>() && hit.collider.CompareTag("Car"))
            {
                Debug.DrawLine(rightSensor.position, hit.point, Color.yellow);
                avoidingObstacle = true;
                avoidObstacleMultiplier -= 0.5f;
            }
        }

        // Right Angle Sensor
        else if (Physics.Raycast(rightSensor.position, Quaternion.AngleAxis(sensorsAngle, rightSensor.up) * rightSensor.forward, out hit, avoidDistance) && avoidObstacleMultiplier > 0)
        {
            if (!hit.collider.GetComponent<CompetitiveDrivingCheck>() && hit.collider.CompareTag("Car"))
            {
                Debug.DrawLine(rightSensor.position, hit.point, Color.yellow);
                avoidingObstacle = true;
                avoidObstacleMultiplier -= 0.2f;
            }
        }

        // Left Sensor
        if (Physics.Raycast(leftSensor.position, frontSensor.forward, out hit, avoidDistance))
        {
            if (!hit.collider.GetComponent<CompetitiveDrivingCheck>() && hit.collider.CompareTag("Car"))
            {
                Debug.DrawLine(leftSensor.position, hit.point, Color.yellow);
                avoidingObstacle = true;
                avoidObstacleMultiplier += 0.5f;
            }
        }


        // Left Angle Sensor
        else if (Physics.Raycast(leftSensor.position, Quaternion.AngleAxis(-sensorsAngle, leftSensor.up) * leftSensor.forward, out hit, avoidDistance) && avoidObstacleMultiplier < 0)
        {
            if (!hit.collider.GetComponent<CompetitiveDrivingCheck>() && hit.collider.CompareTag("Car"))
            {
                Debug.DrawLine(leftSensor.position, hit.point, Color.yellow);
                avoidingObstacle = true;
                avoidObstacleMultiplier += 0.2f;
            }
        }

        if (avoidObstacleMultiplier == 0)
        {
            //front center sensor
            if (Physics.Raycast(frontSensor.position, frontSensor.forward, out hit, avoidDistance))
            {
                if (!hit.collider.GetComponent<CompetitiveDrivingCheck>() && hit.collider.CompareTag("Car"))
                {
                    Debug.DrawLine(frontSensor.position, hit.point, Color.yellow);
                    avoidingObstacle = true;
                    if (hit.normal.x < 0)
                    {
                        avoidObstacleMultiplier = 0.7f;
                    }
                    else
                    {
                        avoidObstacleMultiplier = -0.7f;
                    }
                }
            }
        }





        if(!Physics.Raycast(rightSensor.position, frontSensor.transform.forward, out hit, avoidDistance)&&
            !Physics.Raycast(rightSensor.position, Quaternion.AngleAxis(sensorsAngle, rightSensor.up) * rightSensor.forward, out hit, avoidDistance) &&
            !Physics.Raycast(leftSensor.position, frontSensor.forward, out hit, avoidDistance)&&
            !Physics.Raycast(leftSensor.position, Quaternion.AngleAxis(-sensorsAngle, leftSensor.up) * leftSensor.forward, out hit, avoidDistance)&&
            !Physics.Raycast(frontSensor.position, frontSensor.forward, out hit, avoidDistance)
            )
        {
            avoidObstacleMultiplier = 0;
            avoidingObstacle = false;
        }
    }

 

    private void BrakeSensors()
    {
        RaycastHit hit;
        isBraking = false;

        // Right Sensor
        if (Physics.Raycast(rightSensor.position, frontSensor.forward, out hit, brakeDistance))
        {
            if (!hit.collider.GetComponent<CompetitiveDrivingCheck>() && hit.collider.CompareTag("Car"))
            {
                Debug.DrawLine(rightSensor.position, hit.point, Color.magenta);
                isBraking = true;
            }
        }

        // Left Sensor
        if (Physics.Raycast(leftSensor.position, frontSensor.forward, out hit, brakeDistance))
        {
            if (!hit.collider.GetComponent<CompetitiveDrivingCheck>() && hit.collider.CompareTag("Car"))
            {
                Debug.DrawLine(leftSensor.position, hit.point, Color.magenta);
                isBraking = true;
            }
        }
    }

 

    private void ReverseGearSensors()
    {
        RaycastHit hit;
        reverseGearOn = false;

        // Right Sensor
        if (Physics.Raycast(rightSensor.position, frontSensor.forward, out hit, reverseDistance))
        {
            if (!hit.collider.GetComponent<CompetitiveDrivingCheck>() && hit.collider.CompareTag("Car"))
            {
                Debug.DrawLine(rightSensor.position, hit.point, Color.blue);
                reverseGearOn = true;
            }
        }

        // Left Sensor
        if (Physics.Raycast(leftSensor.position, frontSensor.forward, out hit, reverseDistance))
        {
            if (!hit.collider.GetComponent<CompetitiveDrivingCheck>() && hit.collider.CompareTag("Car"))
            {
                Debug.DrawLine(leftSensor.position, hit.point, Color.blue);
                reverseGearOn = true;
            }
        }
    }

   

    private void PersuitSensors()
    {
        RaycastHit hit;

        // Right Sensor
        if (Physics.Raycast(rightSensor.position, frontSensor.forward, out hit, AICarController.persuitDistance))
        {
            if (hit.collider == AICarController.persuitTarget.GetComponentInChildren<MeshCollider>())
            {
                Debug.DrawLine(rightSensor.position, hit.point, Color.white);
                persuitAiOn = true;
            }
        }

        // Right Angle Sensor
        if (Physics.Raycast(rightSensor.position, Quaternion.AngleAxis(sensorsAngle, rightSensor.up) * rightSensor.forward, out hit, AICarController.persuitDistance))
        {
            if (hit.collider == AICarController.persuitTarget.GetComponentInChildren<MeshCollider>())
            {
                Debug.DrawLine(rightSensor.position, hit.point, Color.white);
                persuitAiOn = true;
            }
        }

        // Left Sensor
        if (Physics.Raycast(leftSensor.position, frontSensor.forward, out hit, AICarController.persuitDistance))
        {
            if (hit.collider == AICarController.persuitTarget.GetComponentInChildren<MeshCollider>())
            {
                Debug.DrawLine(leftSensor.position, hit.point, Color.white);
                persuitAiOn = true;
            }
        }


        // Left Angle Sensor
        if (Physics.Raycast(leftSensor.position, Quaternion.AngleAxis(-sensorsAngle, leftSensor.up) * leftSensor.forward, out hit, AICarController.persuitDistance))
        {
            if (hit.collider == AICarController.persuitTarget.GetComponentInChildren<MeshCollider>())
            {
                Debug.DrawLine(leftSensor.position, hit.point, Color.white);
                persuitAiOn = true;
            }
        }

        if (Physics.Raycast(frontSensor.position, frontSensor.forward, out hit, AICarController.persuitDistance))
        {
            if (hit.collider == AICarController.persuitTarget.GetComponentInChildren<MeshCollider>())
            {
                Debug.DrawLine(leftSensor.position, hit.point, Color.white);
                persuitAiOn = true;
            }
        }
    }



    public void SetTarget(Transform target)
    {
        AICarController.carAItarget = target;
        AICarController.isEngineRunning  = true;
    }
}
