using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosCheckpointManager : MonoBehaviour
{
    public List<CheckpointSystem> checkpoints;


    private void Awake()
    {
        checkpoints = new List<CheckpointSystem>(GetComponentsInChildren<CheckpointSystem>());
    }
    private void Start()
    {
        

        // Certifique-se de que h� pelo menos um checkpoint
        if (checkpoints.Count > 0)
        {
            // Percorra os checkpoints e atribua o pr�ximo checkpoint
            for (int i = 0; i < checkpoints.Count; i++)
            {
                // Se for o �ltimo checkpoint, aponte para o primeiro
                int nextIndex = (i + 1) % checkpoints.Count;
                checkpoints[i].SetupNextCheckpoint(checkpoints[nextIndex]);

                if (i != 0)
                {
                    checkpoints[i].gameObject.SetActive(false);
                }

            }
        }
    }
}