using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.Windows;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CarController2))]
public class PlayerCarInput : MonoBehaviour
{
    internal enum driver
    {
        AI,
        Human
    }    

    [SerializeField]driver driveController;

    public float HorizontalChangeSpeed = 10;
    public bool RotateCameraWithMousePressed;

    private CarInput input;
    CarLights CarLights;
    public float throttleInput { get; private set; }
    public float steeringInput { get; private set; }

    public float BrakeReverse { get; private set; }

    public bool handBrakeInput { get; private set; }

    public bool ManualCameraRotation { get; private set; }

    public Vector2 ViewDelta { get; private set; }
    public bool ViewDeltaFromGamepad { get; private set; }

    public bool cameraswitch, rewind, cameraRear;

    private float GearUpInput = 0;
   
    private float GearDownInput = 0;

    float TargetHorizontal;




    public CarController2 carController;
  
    void Awake()
    {
        input = new CarInput();
      
        
        carController = GetComponent<CarController2>();
        CarLights = GetComponent<CarLights>();
    }
    private void OnEnable()
    {
        switch (driveController)
        {
            case driver.AI:
                break;
            case driver.Human:
                input.Enable();
                input.Gameplay.Acceleration.performed += ApplyThrottle;
                input.Gameplay.Acceleration.canceled += ReleaseThrottle;
                input.Gameplay.BrakeReverse.performed += ApplyBrakeReverse;
                input.Gameplay.BrakeReverse.canceled += ReleaseBrakeReverse;
                input.Gameplay.SteeringAngle.performed += ApplySteering;
                input.Gameplay.SteeringAngle.canceled += ReleaseSteering;
                input.Gameplay.Handbrake.performed += ApplyHandbrake;
                input.Gameplay.Handbrake.canceled += ReleaseHandbrake;
                input.Gameplay.ViewDelta.performed += ApplyViewDelta;
                input.Gameplay.ViewDelta.canceled += ReleaseViewDelta;
                input.Gameplay.CameraSwitch.performed += ChangeCamera;
                input.Gameplay.CameraSwitch.canceled += RealeaseCamera;
                input.Gameplay.Respawn.performed += ApplyRespawn;
                input.Gameplay.Respawn.canceled += ReleaseRespawn;
                input.Gameplay.RearCam.performed += RearCamera;
                input.Gameplay.RearCam.canceled += RealeaseRearCamera;
                input.Gameplay.Lights.performed += TurnMainLightsOn;



                input.Gameplay.GearUp.performed += ApplyGearUp;
                input.Gameplay.GearUp.canceled += ReleaseGearUp;
                input.Gameplay.GearDown.performed += ApplyGearDown;
                input.Gameplay.GearDown.canceled += ReleaseGearDown;
                
                break;
        }
      

    }

    private void Start()
    {


    }
    private void Update()
    {
    }


    private void FixedUpdate()
    {
        switch (driveController)
        {
            case driver.AI: 
                AIDrive();
                break;
            case driver.Human:
                HumanDrive();
                CheckInputDevices();
                break;
        }
  
    }

    private void AIDrive()
    {

        
    }

    void CheckInputDevices()
    {
        var devices = InputSystem.devices;

        // Iterate through all connected devices
        foreach (var device in devices)
        {
            if (device is Keyboard)
            {
                // Set your int to 10 when a keyboard is detected
                HorizontalChangeSpeed = 5;
            }
            else if (device is Gamepad || device is Joystick)
            {
                // Set your int to 0 when a controller is detected
                HorizontalChangeSpeed = 5;
            }
        }
    }

    private void HumanDrive() 
    {
        steeringInput = Mathf.MoveTowards(steeringInput, TargetHorizontal, Time.deltaTime * HorizontalChangeSpeed);
        if (Mouse.current != null)
        {
            ManualCameraRotation = RotateCameraWithMousePressed && !ViewDeltaFromGamepad ? Mouse.current.leftButton.isPressed : ViewDelta.sqrMagnitude > 0.05f;
        }


      //  carController.SetInput(throttleInput, steeringInput, handBrakeInput);
        //steeringCurve.Evaluate(carController.speed)
    }

    private void OnDisable()
    {
        switch (driveController)
        {
            case driver.AI:
                break;
            case driver.Human:
                input.Disable();
                break;
        }
       
        

    }

    private void ApplyThrottle(InputAction.CallbackContext value)
    {
        throttleInput = value.ReadValue<float>();
    }
    private void ReleaseThrottle(InputAction.CallbackContext value)
    {
        throttleInput = 0;
    }
    private void ApplySteering(InputAction.CallbackContext value)
    {
        TargetHorizontal = value.ReadValue<float>();
    }
    private void ReleaseSteering(InputAction.CallbackContext value)
    {
        TargetHorizontal = 0;
    }

    private void ApplyBrakeReverse(InputAction.CallbackContext value)
    {
        BrakeReverse = value.ReadValue<float>();
    }
    private void ReleaseBrakeReverse(InputAction.CallbackContext value)
    {
        BrakeReverse = 0;
    }
    private void ApplyHandbrake(InputAction.CallbackContext value)
    {
        handBrakeInput = true;
    }
    private void ReleaseHandbrake(InputAction.CallbackContext value)
    {
        handBrakeInput = false;
    }

    private void ChangeCamera(InputAction.CallbackContext value)
    {
        cameraswitch = true;
    }
    private void RealeaseCamera(InputAction.CallbackContext value)
    {
        cameraswitch = false;
    }

    private void RearCamera(InputAction.CallbackContext value)
    {
        cameraRear = true;
    }
    private void RealeaseRearCamera(InputAction.CallbackContext value)
    {
        cameraRear = false;
    }

    
    private void TurnMainLightsOn(InputAction.CallbackContext value)
    {
        CarLights.SwitchMainLights();
    }


    private void ApplyRespawn(InputAction.CallbackContext value)
    {
        rewind = true;
    }
    private void ReleaseRespawn(InputAction.CallbackContext value)
    {
        rewind = false;
    }
    private void ApplyViewDelta(InputAction.CallbackContext value)
    {
        ViewDelta = value.ReadValue<Vector2>();
        ViewDeltaFromGamepad = value.control.device is Gamepad;
    }
    private void ReleaseViewDelta(InputAction.CallbackContext value)
    {
        ViewDelta = Vector2.zero;
    }
    private void ApplyGearUp(InputAction.CallbackContext value)
    {

        carController.NextGear();
  
    }
    private void ReleaseGearUp(InputAction.CallbackContext value)
    {

       
    }
    private void ApplyGearDown(InputAction.CallbackContext value)
    {
        
           carController.PrevGear();
     
    }
    private void ReleaseGearDown(InputAction.CallbackContext value)
    {
     
            
     
    }


}