using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CarSounds : MonoBehaviour
{
    public Rigidbody carRigidbody;
    public float minSpeed = 0.0f; // Minimum speed for sound intensity.
    public float maxSpeed = 100.0f; // Maximum speed for maximum sound intensity.
    public float minPitch = 1.0f;
    public float maxPitch = 2.0f;
    public float minVolume = 0.1f;
    public float maxVolume = 1.0f;

    private AudioSource carAudioSource;
    private bool soundStarted = false;

    private void Start()
    {
        carAudioSource = GetComponent<AudioSource>();
        carAudioSource.loop = true;
        carAudioSource.playOnAwake = false;
        carAudioSource.volume = 0.0f; // Set initial volume to zero.
    }

    private void Update()
    {
        if (carRigidbody != null)
        {
            // Get the car's current speed.
            float speed = carRigidbody.velocity.magnitude;

            // Check if the car has started moving.
            if (!soundStarted && speed >= minSpeed)
            {
                carAudioSource.Play();
                soundStarted = true;
            }

            // Map speed to pitch and volume.
            float pitch = Mathf.Lerp(minPitch, maxPitch, Mathf.InverseLerp(minSpeed, maxSpeed, speed));
            float volume = Mathf.Lerp(minVolume, maxVolume, Mathf.InverseLerp(minSpeed, maxSpeed, speed));

            // Set the Audio Source properties.
            carAudioSource.pitch = pitch;
            carAudioSource.volume = volume;
        }
    }
}
