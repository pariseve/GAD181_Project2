using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPopup : MonoBehaviour
{
    public GameObject tutorialPopup;
    private bool tutorialShown;

    private void Start()
    {
        tutorialShown = PlayerPrefs.GetInt("TutorialShown", 0) == 1;

        if (!tutorialShown)
        {
            ShowTutorialPopup();
            PauseGame();
        }
    }

    private void Update()
    {
        if (tutorialPopup.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseTutorialPopup();
        }
    }

    public void OnExitButtonClicked()
    {
        CloseTutorialPopup();
    }

    public void ShowTutorialPopup()
    {
        tutorialPopup.SetActive(true);
    }

    public void CloseTutorialPopup()
    {
        tutorialPopup.SetActive(false);
        tutorialShown = true;
        PlayerPrefs.SetInt("TutorialShown", 1);
        UnpauseGame();
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
    }

    private void UnpauseGame()
    {
        Time.timeScale = 1f;
    }
}