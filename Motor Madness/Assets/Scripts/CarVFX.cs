using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarVFX : MonoBehaviour
{
    [Header("CarVFX")]

    [SerializeField] TrailRenderer TrailRef;
    public Dictionary<wheelsManager, TrailRenderer> ActiveTrails { get; private set; }
    Queue<TrailRenderer> FreeTrails = new Queue<TrailRenderer>(); //Free trail pool

    const float OffsetHitHeightForTrail = 0.05f;

    Transform ParentForEffects;         //Parent object for effects, created in the scene without a parent (Does not move).
    static Transform EffectsHolder;

    public List<ParticleSystem> ExhaustParticles = new List<ParticleSystem>();
    public List<ParticleSystem> BackFireParticles = new List<ParticleSystem>();
    public List<ParticleSystem> SmokeParticles = new List<ParticleSystem>();


    private CarController2 carController;
    private void Awake()
    {
        carController = GetComponentInParent<CarController2>();

        TrailRef.gameObject.SetActive(false);

        ActiveTrails = new Dictionary<wheelsManager, TrailRenderer>();
        

        carController.BackFireAction += OnBackFire;



    }

    private void Start()
    {
        foreach (var wheel in carController.wheels)
        {
            if (wheel != null)
            {
                ActiveTrails.Add(wheel, null);
            }

        }
    }

    private void Update()
    {
        for (int i = 0; i < carController.wheels.Length; i++)
        {
            var wheel = carController.wheels[i];
            

            var hasSlip = wheel.HasForwardSlip || wheel.HasSideSlip;


            UpdateTrail(wheel, !wheel.StopEmitFX && wheel.IsGrounded && hasSlip);
        }
           
    }



    void OnBackFire ()
    {
        foreach (var particles in BackFireParticles)
        {
            particles.Emit (1);
        }
    }

    

    public void UpdateTrail(wheelsManager wheel, bool hasSlip)
    {
        var trail = ActiveTrails[wheel];

        if (hasSlip)
        {
            if (trail == null)
            {
                //Get free or create trail.

                trail = GetTrail(wheel.WheelView.position + (wheel.transform.up * (-wheel.Radius + OffsetHitHeightForTrail)));
                trail.transform.SetParent(wheel.transform);
                ActiveTrails[wheel] = trail;
            }
            else
            {
                //Move the trail to the desired position
                trail.transform.position = wheel.WheelView.position + (wheel.transform.up * (-wheel.Radius + OffsetHitHeightForTrail));
                foreach (var particles in SmokeParticles)
                {
                    particles.transform.position = wheel.WheelView.position + (wheel.transform.up * (-wheel.Radius + 0.15f));
                    particles.Emit(1);
                }

            }
        }
        else if (ActiveTrails[wheel] != null)
        {
            //Set trail as free.
            SetTrailAsFree(trail);
            trail = null;
            ActiveTrails[wheel] = trail;
        }
    }

    public TrailRenderer GetTrail(Vector3 startPos)
    {
        TrailRenderer trail = null;
        if (FreeTrails.Count > 0)
        {
            trail = FreeTrails.Dequeue();
        }
        else
        {
            trail = Instantiate(TrailRef, ParentForEffects);
        }

        trail.transform.position = startPos;
        trail.gameObject.SetActive(true);
        trail.Clear();

        return trail;
    }

    public void SetTrailAsFree(TrailRenderer trail)
    {
        StartCoroutine(WaitVisibleTrail(trail));
    }

    /// <summary>
    /// The trail is considered busy until it disappeared.
    /// </summary>
    private IEnumerator WaitVisibleTrail(TrailRenderer trail)
    {
        trail.transform.SetParent(ParentForEffects);
        yield return new WaitForSeconds(trail.time);
        trail.Clear();
        trail.gameObject.SetActive(false);
        FreeTrails.Enqueue(trail);
    }

   

}
