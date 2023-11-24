using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RewindManager : MonoBehaviour
{
    
    public float rewindDuration = 4f;
    public float recordInterval = 0.1f;
    private PlayerCarInput carInput;

    private Rigidbody carRigidbody;
    private List<RewindState> recordedStates = new List<RewindState>();

    private void Start()
    {
        carRigidbody = GetComponent<Rigidbody>();
        carInput = GetComponent<PlayerCarInput>();
        StartCoroutine(RecordStates());
    }

    private void Update()
    {
        if (carInput.rewind)
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        float targetTime = Time.time - rewindDuration;

        // Find the recorded state closest to the target time
        RewindState closestState = FindClosestRecordedState(targetTime);

        // Apply the recorded state to the player
        if (closestState != null)
        {
            transform.position = closestState.position;
            transform.rotation = closestState.rotation;
            carRigidbody.velocity = closestState.velocity;
        }
    }

    private IEnumerator<WaitForSeconds> RecordStates()
    {
        while (true)
        {
            recordedStates.Add(new RewindState(transform.position, transform.rotation, carRigidbody.velocity, Time.time));
            yield return new WaitForSeconds(recordInterval);
        }
    }

    private RewindState FindClosestRecordedState(float targetTime)
    {
        RewindState closestState = null;
        float closestTimeDifference = float.MaxValue;

        foreach (RewindState state in recordedStates)
        {
            float timeDifference = Mathf.Abs(state.timestamp - targetTime);

            if (timeDifference < closestTimeDifference)
            {
                closestState = state;
                closestTimeDifference = timeDifference;
            }
        }

        return closestState;
    }
}

public class RewindState
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 velocity;
    public float timestamp;

    public RewindState(Vector3 pos, Quaternion rot, Vector3 vel, float time)
    {
        position = pos;
        rotation = rot;
        velocity = vel;
        timestamp = time;
    }
}
