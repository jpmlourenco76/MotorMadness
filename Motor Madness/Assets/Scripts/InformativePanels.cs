using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformativePanels : MonoBehaviour
{
    public GameManager gameManager;
    public GameData gameData;

    private bool threeSecondsDone = false;
    private float timerDuration = 3f;
    private float timer;

    private GameObject originalCanvas;

    private bool fadedOut = false;

    public CanvasGroup specialPanelOne;
    public CanvasGroup specialPanelTwo;
    public CanvasGroup specialPanelThree;

    private void Start()
    {
        threeSecondsDone = false;
        fadedOut = false;
        GameManager gameManager = GameManager.Instance;
    }

    public void SpecialPanel(int carPointer)
    {
        if (gameData.characters[0].currentLevel == 2)
        {
            originalCanvas.SetActive(true);
            if (specialPanelOne.alpha < 1)
            {
                specialPanelOne.alpha += Time.deltaTime;

                if (specialPanelOne.alpha >= 1)
                {
                    Debug.Log("Panel 1 Showing");

                    if (threeSecondsDone)
                    {
                        timer += Time.deltaTime;

                        if (timer >= timerDuration)
                        {
                            threeSecondsDone = true;
                        }
                    }

                }

            }

            if (specialPanelOne.alpha >= 0)
            {
                specialPanelOne.alpha -= Time.deltaTime;

                if (specialPanelOne.alpha == 0)
                {
                    if (threeSecondsDone && fadedOut == false)
                    {
                        threeSecondsDone = false;
                        fadedOut = true;
                        originalCanvas.SetActive(false);
                        gameManager.GoLevel(carPointer);

                    }

                }

            }
        }
        else if (gameData.characters[0].currentLevel == 4)
        {
            originalCanvas.SetActive(true);
            if (specialPanelTwo.alpha < 1)
            {
                specialPanelTwo.alpha += Time.deltaTime;

                if (specialPanelTwo.alpha >= 1)
                {
                    Debug.Log("Panel 2 Showing");

                    if (threeSecondsDone)
                    {
                        timer += Time.deltaTime;

                        if (timer >= timerDuration)
                        {
                            threeSecondsDone = true;
                        }
                    }

                }

            }

            if (specialPanelTwo.alpha >= 0)
            {
                specialPanelTwo.alpha -= Time.deltaTime;

                if (specialPanelTwo.alpha == 0)
                {
                    if (threeSecondsDone && fadedOut == false)
                    {
                        threeSecondsDone = false;
                        fadedOut = true;
                        originalCanvas.SetActive(false);
                        gameManager.GoLevel(carPointer);
                    }

                }

            }
        }
        else if (gameData.characters[0].currentLevel == 6)
        {
            originalCanvas.SetActive(true);
            if (specialPanelThree.alpha < 1)
            {
                specialPanelThree.alpha += Time.deltaTime;

                if (specialPanelThree.alpha >= 1)
                {
                    Debug.Log("Panel 3 Showing");

                    if (threeSecondsDone)
                    {
                        timer += Time.deltaTime;

                        if (timer >= timerDuration)
                        {
                            threeSecondsDone = true;
                        }
                    }

                }

            }

            if (specialPanelThree.alpha >= 0)
            {
                specialPanelThree.alpha -= Time.deltaTime;

                if (specialPanelTwo.alpha == 0)
                {
                    if (threeSecondsDone && fadedOut == false)
                    {
                        threeSecondsDone = false;
                        fadedOut = true;
                        originalCanvas.SetActive(false);
                        gameManager.GoLevel(carPointer);

                    }

                }

            }
        }
        else
        {
            gameManager.GoLevel(carPointer);
        }
    }
}
