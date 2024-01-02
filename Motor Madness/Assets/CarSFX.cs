using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarSFX : MonoBehaviour
{
    public CarController2 carController;

    private List <wheelsManager> Wheels = new List<wheelsManager>();


    [Header("Ground effects")]
    [SerializeField] AudioSource WheelsEffectSourceRef;                                //Wheel source reference, for playing slip sounds.
    [SerializeField] List<GroundSound> GroundSounds = new List<GroundSound>();


    [Header("Collisions")]
    [SerializeField] float MinTimeBetweenCollisions = 0.1f;
    [SerializeField] float DefaultMagnitudeDivider = 20;                                //default divider to calculate collision volume.
    [SerializeField] AudioClip DefaultCollisionClip;                                    //Clip playable if the desired one was not found.      
    [SerializeField] List<ColissionEvent> CollisionEvents = new List<ColissionEvent>();

    [Header("Frictions")]
    [SerializeField] AudioSource FrictionEffectSourceRef;
    [SerializeField] float PlayFrictionTime = 0.5f;
    [SerializeField] AudioClip DefaultFrictionClip;                                     //Clip playable if the desired one was not found.                        
    [SerializeField] List<ColissionEvent> FrictionEvents = new List<ColissionEvent>();


    [Header("Other settings")]
    public AudioSource OtherEffectsSource;                                             //Source for playing other sound effects.

   

    [Header("Engine")]
    [SerializeField] AudioSource EngineSourceRef;
    [SerializeField] AudioClip StartEngineClip;
    [SerializeField] AudioClip StopEngineClip;
    [SerializeField] AudioClip LowEngineClip;
    [SerializeField] AudioClip MediumEngineClip;
    [SerializeField] AudioClip HighEngineClip;

    [SerializeField] float MinEnginePitch = 0.5f;
    [SerializeField] float MaxEnginePitch = 1.5f;

    [SerializeField] List<AudioClip> BackFireClips;

    [Header("Wind Sound")]
    [SerializeField] AudioSource SpeedWindSource;
    [SerializeField] float WindSoundStartSpeed = 20;
    [SerializeField] float WindSoundMaxSpeed = 100;
    [SerializeField] float WindSoundStartPitch = 0.4f;
    [SerializeField] float WindSoundMaxPitch = 1.5f;

    AudioClip CurrentFrictionClip;
    float LastCollisionTime;
    float LastBlowOffTime;
    float[] EngineSourcesRanges = new float[1] { 1f };
    List<AudioSource> EngineSources = new List<AudioSource>();
    Dictionary<int, GroundSound> WheelSounds = new Dictionary<int, GroundSound>();
    Dictionary<AudioClip, FrictionSoundData> FrictionSounds = new Dictionary<AudioClip, FrictionSoundData>();

    private bool onEngine = false;
    private void Awake()
    {
        carController = GetComponentInParent<CarController2>();



    }
    private void Start()
    {
        foreach (var wheel in carController.wheels) { 
            if(wheel != null)
            {
                Wheels.Add(wheel);
            }
        }

        if (OtherEffectsSource && OtherEffectsSource.gameObject.activeInHierarchy)
        {
           carController.CollisionAction += PlayCollisionSound;
        }

        if (WheelsEffectSourceRef != null && WheelsEffectSourceRef.gameObject.activeInHierarchy)
        {
            WheelsEffectSourceRef.volume = 0;
            if (WheelsEffectSourceRef.clip != null)
            {
                foreach (var groundSound in GroundSounds)
                {
                    if (groundSound.IdleGroundClip == WheelsEffectSourceRef.clip)
                    {
                        groundSound.Source = WheelsEffectSourceRef;
                        WheelSounds.Add(carController.wheels[0].MaterialIndex, groundSound);
                        break;
                    }
                }
            }
            else
            {
                WheelsEffectSourceRef.Stop();
            }

            UpdateWheels();
        }
        if (FrictionEffectSourceRef != null && FrictionEffectSourceRef.gameObject.activeInHierarchy)
        {
            FrictionSounds.Add(FrictionEffectSourceRef.clip, new FrictionSoundData() { Source = FrictionEffectSourceRef, LastFrictionTime = Time.time });
            FrictionEffectSourceRef.Stop();

            UpdateFrictions();
            carController.CollisionStayAction += PlayCollisionStayAction;
        }
        carController.BackFireAction += OnBackFire;

        if (EngineSourceRef && EngineSourceRef.gameObject.activeInHierarchy)
        {

            //Create engine sounds list.
            List<AudioClip> engineClips = new List<AudioClip>();
            if (LowEngineClip != null)
            {
                engineClips.Add(LowEngineClip);
            }
            if (MediumEngineClip != null)
            {
                engineClips.Add(MediumEngineClip);
            }
            if (HighEngineClip != null)
            {
                engineClips.Add(HighEngineClip);
            }

            if (engineClips.Count == 2)
            {
                //If the engine has 2 sounds, then they will switch at 30% rpm.
                EngineSourcesRanges = new float[2] { 0.3f, 1f };
            }
            else if (engineClips.Count == 3)
            {
                //If the engine has 3 sounds, then they will switch at 30% and 60% rpm.
                EngineSourcesRanges = new float[3] { 0.3f, 0.6f, 1f };
            }

            //Init Engine sounds.
            if (engineClips != null && engineClips.Count > 0)
            {
                AudioSource engineSource;

                for (int i = 0; i < engineClips.Count; i++)
                {
                    if (EngineSourceRef.clip == engineClips[i])
                    {
                        engineSource = EngineSourceRef;
                        EngineSourceRef.transform.SetSiblingIndex(EngineSourceRef.transform.parent.childCount);
                    }
                    else
                    {
                        engineSource = Instantiate(EngineSourceRef, EngineSourceRef.transform.parent);
                        engineSource.clip = engineClips[i];
                        engineSource.Play();
                    }

                    engineSource.name = string.Format("Engine source ({0})", i);
                    EngineSources.Add(engineSource);
                }

                if (!EngineSources.Contains(EngineSourceRef))
                {
                    Destroy(EngineSourceRef);
                }
            }

            if (!carController.isEngineRunning)
            {
                if (EngineSources != null && EngineSources.Count > 0)
                {
                    EngineSources.ForEach(s => s.pitch = 0);
                }
                else
                {
                    EngineSourceRef.pitch = 0;
                }
            }

            UpdateEngine();
        }

        if (SpeedWindSource && SpeedWindSource.gameObject.activeInHierarchy)
        {
            UpdateWindEffect();
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateWheels();
        UpdateFrictions();
        UpdateEngine();
        UpdateWindEffect();
    }

    public void StartEngine(float startDellay)
    {
        if (StartEngineClip != null)
        {
            OtherEffectsSource.PlayOneShot(StartEngineClip);
        }
        StartCoroutine(ActivateBoolAfterDelay(startDellay));
    }
    IEnumerator ActivateBoolAfterDelay(float delay)
    {
      
        yield return new WaitForSeconds(delay);

        onEngine = true;

    }

    void UpdateWheels()
    {
        foreach(var wheel in carController.wheels)
        {
            GroundSound sound = null;
            
            if (!WheelSounds.TryGetValue(wheel.MaterialIndex, out sound))
            {
                var source = WheelsEffectSourceRef.gameObject.AddComponent<AudioSource>();
                source.playOnAwake = WheelsEffectSourceRef.playOnAwake;
                source.spatialBlend = WheelsEffectSourceRef.spatialBlend;

                for (int i = 0; i < GroundSounds.Count; i++)
                {
                    if (i == wheel.MaterialIndex)
                    {
                        sound = GroundSounds[i];
                        break;
                    }
                }

                if (sound == null)
                {
                    sound = GroundSounds[0];
                }

                source.Stop();
                source.volume = 0;
                sound.Source = source;
                WheelSounds.Add(wheel.MaterialIndex, sound);
            }
            sound.WheelsCount++;

            //Find the maximum slip for each sound.
            if (wheel.SlipNormalized > sound.Slip)
            {
                sound.Slip = wheel.SlipNormalized;
            }
        }

        var speedNormalized = Mathf.Clamp01(carController.CurrentSpeed / 30);


        foreach (var sound in WheelSounds)
        {
            AudioClip clip;
            float targetVolume;

            if (sound.Value.Slip >= 0.9f)
            {
                clip = sound.Value.SlipGroundClip;
                targetVolume = Mathf.Clamp01(sound.Value.Slip - 0.5f);
            }
            else
            {
                clip = sound.Value.IdleGroundClip;
                targetVolume = Mathf.Clamp01(carController.CurrentSpeed / 30);
            }

            if (sound.Value.Source.clip != clip && clip != null)
            {
                sound.Value.Source.clip = clip;
            }

            if (sound.Value.WheelsCount == 0 || speedNormalized == 0 || clip == null)
            {
                targetVolume = 0;
            }

            //Passing parameters to sources.
            sound.Value.Source.volume = Mathf.Lerp(sound.Value.Source.volume, targetVolume, 10 * Time.deltaTime);
            sound.Value.Source.pitch = Mathf.Lerp(0.7f, 1.2f, sound.Value.Source.volume);

            sound.Value.Slip = 0;
            sound.Value.WheelsCount = 0;

            if (Mathf.Approximately(0, sound.Value.Source.volume) && sound.Value.Source.isPlaying)
            {
                sound.Value.Source.Stop();
            }
            else if (!Mathf.Approximately(0, sound.Value.Source.volume) && !sound.Value.Source.isPlaying)
            {
                sound.Value.Source.Play();
            }
        }

    }

    void UpdateFrictions()
    {
        FrictionSoundData soundData;
        var speedNormalized = Mathf.Clamp01(carController.CurrentSpeed / 30);

        foreach (var sound in FrictionSounds)
        {
            soundData = sound.Value;
            if (soundData.Source.isPlaying)
            {
                var time = Time.time - soundData.LastFrictionTime;

                if (time > PlayFrictionTime)
                {
                    sound.Value.Source.pitch = 0;
                    sound.Value.Source.volume = 0;
                    soundData.Source.Stop();
                }
                else
                {
                    sound.Value.Source.pitch = Mathf.Lerp(0.4f, 1.2f, speedNormalized);
                    soundData.Source.volume = speedNormalized * (1 - (time / soundData.LastFrictionTime));
                }
            }
        }
    }


    public void PlayCollisionStayAction(CarController2 vehicle, Collision collision)
    {
        if (carController.CurrentSpeed >= 1 && (collision.rigidbody == null || (collision.rigidbody.velocity - vehicle.playerRB.velocity).sqrMagnitude > 25))
        {
            PlayFrictionSound(collision, collision.relativeVelocity.magnitude);
        }
    }
    public void PlayCollisionSound(CarController2 vehicle, Collision collision)
    {
        
        var collisionLayer = collision.gameObject.layer;

        if (Time.time - LastCollisionTime < MinTimeBetweenCollisions)
        {
            return;
        }

        LastCollisionTime = Time.time;
        float collisionMagnitude = 0;
        if (collision.rigidbody)
        {
            collisionMagnitude = (carController.playerRB.velocity - collision.rigidbody.velocity).magnitude;
        }
        else
        {
            collisionMagnitude = collision.relativeVelocity.magnitude;
        }
        float magnitudeDivider;

        var audioClip = GetClipForCollision(collisionLayer, collisionMagnitude, out magnitudeDivider);

        var volume = Mathf.Clamp01(collisionMagnitude );

        Debug.Log(audioClip + " " + volume);

        OtherEffectsSource.PlayOneShot(audioClip, volume);
    }

    void PlayFrictionSound(Collision collision, float magnitude)
    {
        if (carController.CurrentSpeed >= 1)
        {
            CurrentFrictionClip = GetClipForFriction(collision.collider.gameObject.layer, magnitude);

            FrictionSoundData soundData;
            if (!FrictionSounds.TryGetValue(CurrentFrictionClip, out soundData))
            {
                var source = FrictionEffectSourceRef.gameObject.AddComponent<AudioSource>();
                source.clip = CurrentFrictionClip;

                soundData = new FrictionSoundData() { Source = source };
                FrictionSounds.Add(CurrentFrictionClip, soundData);
            }

            if (!soundData.Source.isPlaying)
            {
                soundData.Source.Play();
            }

            soundData.LastFrictionTime = Time.time;
        }
    }
    AudioClip GetClipForCollision(int layer, float collisionMagnitude, out float magnitudeDivider)
    {
        for (int i = 0; i < CollisionEvents.Count; i++)
        {
            if ( collisionMagnitude >= CollisionEvents[i].MinMagnitudeCollision && collisionMagnitude < CollisionEvents[i].MaxMagnitudeCollision)
            {
                if (CollisionEvents[i].MaxMagnitudeCollision == float.PositiveInfinity)
                {
                    magnitudeDivider = DefaultMagnitudeDivider;
                }
                else
                {
                    magnitudeDivider = CollisionEvents[i].MaxMagnitudeCollision;
                }

                return CollisionEvents[i].AudioClip;
            }
        }

        magnitudeDivider = DefaultMagnitudeDivider;
        return DefaultCollisionClip;
    }

    /// <summary>
    /// Search for the desired event based on the friction magnitude and the collision layer.
    /// </summary>
    /// <param name="layer">Collision layer.</param>
    /// <param name="collisionMagnitude">Collision magnitude.</param>
    AudioClip GetClipForFriction(int layer, float collisionMagnitude)
    {
        for (int i = 0; i < FrictionEvents.Count; i++)
        {
            if (collisionMagnitude >= FrictionEvents[i].MinMagnitudeCollision && collisionMagnitude < FrictionEvents[i].MaxMagnitudeCollision)
            {
                return FrictionEvents[i].AudioClip;
            }
        }

        return DefaultFrictionClip;
    }


    void UpdateEngine()
    {
        if (carController.isEngineRunning && onEngine)
        {
            if (EngineSources.Count == 0 && EngineSourceRef && EngineSourceRef.gameObject.activeInHierarchy)
            {
                EngineSourceRef.pitch = Mathf.Lerp(MinEnginePitch, MaxEnginePitch, (carController.EngineRPM - carController.MinRPM) / (carController.MaxRPM - carController.MinRPM));
            }
            else if (EngineSources.Count > 1)
            {
                float rpmNorm =Mathf.Clamp01 ((carController.EngineRPM - carController.MinRPM) / (carController.MaxRPM - carController.MinRPM));
                float pith = Mathf.Lerp(MinEnginePitch, MaxEnginePitch, rpmNorm);

                for (int i = 0; i < EngineSources.Count; i++)
                {
                    EngineSources[i].pitch = pith;

                    if (i > 0 && rpmNorm < EngineSourcesRanges[i - 1])
                    {
                        EngineSources[i].volume = Mathf.InverseLerp(0.2f, 0, EngineSourcesRanges[i - 1] - rpmNorm);
                    }
                    else if (rpmNorm > EngineSourcesRanges[i])
                    {
                        EngineSources[i].volume = Mathf.InverseLerp(0.3f, 0, rpmNorm - EngineSourcesRanges[i]);
                    }
                    else
                    {
                        EngineSources[i].volume = 1;
                    }

                    if (Mathf.Approximately(EngineSources[i].volume, 0) && EngineSources[i].isPlaying)
                    {
                        EngineSources[i].Stop();
                    }

                    if (EngineSources[i].volume > 0 && !EngineSources[i].isPlaying)
                    {
                        EngineSources[i].Play();
                    }
                }
            }
        }
        else if (!carController.isEngineRunning && onEngine)
        {
            float pith = Mathf.Lerp(0, MinEnginePitch, Mathf.Clamp01(carController.EngineRPM / carController.MinRPM));
            if (EngineSources.Count == 0 && EngineSourceRef && EngineSourceRef.gameObject.activeInHierarchy)
            {
                EngineSourceRef.pitch = Mathf.MoveTowards(EngineSourceRef.pitch, pith, Time.deltaTime);
            }
            else if (EngineSources.Count > 1)
            {
                EngineSources[0].pitch = Mathf.MoveTowards(EngineSources[0].pitch, pith, Time.deltaTime);
                for (int i = 1; i < EngineSources.Count; i++)
                {
                    EngineSources[i].pitch = pith;
                    if (EngineSources[i].isPlaying)
                    {
                        EngineSources[i].Stop();
                    }
                }
            }
        }
    }

    void UpdateWindEffect()
    {
        if (carController.human)
        {
            var curentSpeedNorm = Mathf.InverseLerp(WindSoundStartSpeed, WindSoundMaxSpeed, carController.CurrentSpeed);
            if (curentSpeedNorm > 0 && !SpeedWindSource.isPlaying)
            {
                SpeedWindSource.Play();
            }
            SpeedWindSource.volume = curentSpeedNorm;
            SpeedWindSource.pitch = Mathf.Lerp(WindSoundStartPitch, WindSoundMaxPitch, curentSpeedNorm);
        }
        else if (SpeedWindSource.isPlaying)
        {
            SpeedWindSource.Stop();
        }
    }
    void OnBackFire()
    {
        if (BackFireClips != null && BackFireClips.Count > 0)
        {
            OtherEffectsSource.PlayOneShot(BackFireClips[Random.Range(0, BackFireClips.Count - 1)]);
        }
    }


    [System.Serializable]
    public struct ColissionEvent
    {
        public AudioClip AudioClip;
        public LayerMask CollisionMask;
        public float MinMagnitudeCollision;
        public float MaxMagnitudeCollision;
    }


    public class FrictionSoundData
    {
        public AudioSource Source;
        public float LastFrictionTime;
    }


    [System.Serializable]
    public class GroundSound
    {
        public AudioClip IdleGroundClip;
        public AudioClip SlipGroundClip;

        public AudioSource Source { get; set; }
        public float Slip { get; set; }
        public int WheelsCount { get; set; }
    }
}
