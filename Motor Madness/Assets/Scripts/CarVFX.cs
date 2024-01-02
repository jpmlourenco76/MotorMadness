using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class CarVFX : MonoBehaviour
{
    [Header("CarVFX")]

    [SerializeField] TrailRenderer TrailRef;
    public Dictionary<wheelsManager, TrailRenderer> ActiveTrails { get; private set; }
    Queue<TrailRenderer> FreeTrails = new Queue<TrailRenderer>(); //Free trail pool

    const float OffsetHitHeightForTrail = 0.05f;

    Transform ParentForEffects;         //Parent object for effects, created in the scene without a parent (Does not move).
    static Transform EffectsHolder;

    [SerializeField] ParticleSystem DefaultCollisionParticles;
    [SerializeField] List<CollissionParticles> CollisionParticlesList = new List<CollissionParticles>();
    public List<ParticleSystem> BackFireParticles = new List<ParticleSystem>();
    public List<ParticleSystem> SmokeParticles = new List<ParticleSystem>();
    public ParticleSystem RainMist;
    public List<ParticleSystem> RainTrail = new List<ParticleSystem>();

    [SerializeField] float MinTimeBetweenCollisions = 0.1f;
    float LastCollisionTime;

    public bool raining = false;



    private CarController2 carController;
    private void Awake()
    {
        carController = GetComponentInParent<CarController2>();

        TrailRef.gameObject.SetActive(false);

        ActiveTrails = new Dictionary<wheelsManager, TrailRenderer>();
        

        carController.BackFireAction += OnBackFire;
        carController.CollisionAction += PlayCollisionParticles;
        carController.CollisionStayAction += CollisionStay;


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
        EmitParams emitParams, emitParams2;
        float rndValue = UnityEngine.Random.Range(0, 1f);
        for (int i = 0; i < carController.wheels.Length; i++)
        {
            var wheel = carController.wheels[i];
            var hasSlip = wheel.HasForwardSlip || wheel.HasSideSlip;

            if((wheel.MaterialIndex == 1 || wheel.MaterialIndex == 2) && (carController.CurrentSpeed > 5f || hasSlip))
            {
                var particles = SmokeParticles[wheel.MaterialIndex];
                var point = wheel.transform.position;
                point.y = wheel.GetHit.point.y;
                var particleVelocity = -wheel.GetHit.forwardDir * wheel.GetHit.forwardSlip;
                particleVelocity += wheel.GetHit.sidewaysDir * wheel.GetHit.sidewaysSlip;
                particleVelocity += carController.playerRB.velocity;

                emitParams = new EmitParams();

                emitParams.position = point;
                emitParams.velocity = particleVelocity;
                emitParams.startSize = Mathf.Max(1f, particles.main.startSize.constant  * rndValue);
                emitParams.startLifetime = particles.main.startLifetime.constant *  rndValue;
                emitParams.startColor = particles.main.startColor.color;

                particles.Emit(emitParams, 1);
            }

            if (raining)
            {
                if(RainMist != null)
                {
                    RainMist.Emit(1);
                }
                if(i > 1)
                {
                    emitParams2 = new EmitParams();
                    emitParams2.startSize = Random.Range(0,Mathf.Lerp(-1, 1.2f, carController.playerRB.velocity.magnitude / 15f));
                    if (emitParams2.startSize < 0) emitParams2.startSize = 0f;
                   
                    foreach (var raintrail in RainTrail)
                    {
                        raintrail.Emit(emitParams2,1);
                    }
                }
                
                
            }

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

    private void CollisionStay(CarController2 carController, Collision collision)
    {
        if (carController.CurrentSpeed >= 1 && (collision.rigidbody == null || (collision.rigidbody.velocity - carController.playerRB.velocity).sqrMagnitude > 25))
        {
            PlayCollisionParticles(carController, collision);
        }
    }

    public void PlayCollisionParticles(CarController2 carController, Collision collision)
    {
        if (collision == null || Time.time - LastCollisionTime < MinTimeBetweenCollisions)
        {
            return;
        }
      
        LastCollisionTime = Time.time;
        var magnitude = collision.relativeVelocity.magnitude * Mathf.Abs( Vector3.Dot(collision.relativeVelocity.normalized, collision.contacts[0].normal));
        var particles = GetParticlesForCollision(magnitude);
        
        


        float offsetDistance = 0.1f;

        for (int i = 0; i < collision.contacts.Length; i++)
        {
            Vector3 ContactPosition = collision.contacts[i].point;
            Vector3 contactNormal = collision.contacts[i].normal;

            particles.transform.position = ContactPosition + (contactNormal * offsetDistance);

            
            particles.Play(withChildren: true);
        }
    }

    public ParticleSystem GetParticlesForCollision(float collisionMagnitude)
    {
        for (int i = 0; i < CollisionParticlesList.Count; i++)
        {
            if (collisionMagnitude >= CollisionParticlesList[i].MinMagnitudeCollision && collisionMagnitude < CollisionParticlesList[i].MaxMagnitudeCollision)
            {
                return CollisionParticlesList[i].Particles;
            }
        }

        return DefaultCollisionParticles;
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
              
   
                    SmokeParticles[wheel.MaterialIndex].transform.position = wheel.WheelView.position + (wheel.transform.up * (-wheel.Radius + 0.15f));
                    SmokeParticles[wheel.MaterialIndex].Emit(1);

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

    [System.Serializable]
    public struct CollissionParticles
    {
        public ParticleSystem Particles;
        public float MinMagnitudeCollision;
        public float MaxMagnitudeCollision;
    }

}
