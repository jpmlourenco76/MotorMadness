using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Video;

public class videocontroller : MonoBehaviour
{
    public RawImage rawImage;
    public List <VideoPlayer>  videoPlayer;
    private GameManager gameManager;
    private bool allowSkip = false;
    private bool skipvideo;
    private bool foundvideo = false;
    private VideoPlayer currentVideoPlayer;
    public bool videoCompleted = false;
    private void Awake()
    {
        videoPlayer = new List<VideoPlayer> (GetComponentsInChildren<VideoPlayer>());
        
       
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        if (gameManager == null)
        {
            Debug.LogError("GameManager is not assigned in the GameManager.Instance.");
            return;
        }
    }
    private void OnEnable()
    {
        var skipAction = new CarInput().Menu.SkipVideo;
        skipAction.performed += Skipvideo;
        skipAction.canceled += NoSkipvideo;
        skipAction.Enable();

    }

  
    
    public void videoStart()
    {
        StartCoroutine(PlayVideoSingle());
    }

    public  IEnumerator PlayVideo()
    {
        foundvideo = false;
        videoCompleted = false;
        skipvideo = false;
        allowSkip = false;

        float delayBeforeSkip = 2.0f;
        float timer = 0.0f;


        bool conditionmet = false; 
        rawImage.enabled = true;
        for (int i = 0; i < videoPlayer.Count; i++)
        {
            
            if (gameManager.gameData.characters[0].characterName.Contains(videoPlayer[i].gameObject.name))
            {
                currentVideoPlayer = videoPlayer[i];
                currentVideoPlayer.Prepare();

                WaitForSeconds waitForSeconds = new WaitForSeconds(1);

                while (!currentVideoPlayer.isPrepared)
                {
                    yield return waitForSeconds;
                    break;
                }

                rawImage.texture = currentVideoPlayer.texture;
                rawImage.color = Color.white;
                foundvideo = true;
                currentVideoPlayer.Play();

                

                allowSkip = true;




                while (currentVideoPlayer.isPlaying && !skipvideo)
                {
                    yield return null; 
                    
                }
                videoCompleted = true;
                if (skipvideo)
                {
                    currentVideoPlayer.Stop();
                    rawImage.enabled = false;
                }
                conditionmet = true;
                break;
            }

        }
     
        
    }


    public IEnumerator PlayVideoSingle()
    {
        foundvideo = false;
        videoCompleted = false;
        skipvideo = false;
        allowSkip = false;
        float delayBeforeSkip = 2.0f;
        float timer = 0.0f;
        rawImage.enabled = true;

        if (!foundvideo)
        {
            
            currentVideoPlayer = videoPlayer[0];
            currentVideoPlayer.Prepare();

            WaitForSeconds waitForSeconds = new WaitForSeconds(1);

            while (!currentVideoPlayer.isPrepared)
            {
                yield return waitForSeconds;
                break;
            }

            rawImage.texture = currentVideoPlayer.texture;
            rawImage.color = Color.white;
            currentVideoPlayer.Play();


            while (timer < delayBeforeSkip)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            allowSkip = true;

            while (currentVideoPlayer.isPlaying && !skipvideo)
            {
                yield return null;


            }
            videoCompleted = true;
            if (skipvideo)
            {
                currentVideoPlayer.Stop();
                rawImage.enabled = false;
            }

        }
    }

        private void Skipvideo(InputAction.CallbackContext value)
    {
        if (allowSkip)
        {
            skipvideo = true;
        }
    }
    private void NoSkipvideo(InputAction.CallbackContext value)
    {
        skipvideo = false;
    }
}
