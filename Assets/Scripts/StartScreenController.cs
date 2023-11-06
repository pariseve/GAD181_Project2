using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenController : MonoBehaviour
{
    private void OnEnable()
    {
        // Clear PlayerPrefs when the "startscreen" scene is enabled.
        PlayerPrefs.DeleteAll();
    }

    public void LoadWoodlandScene()
    {
        SceneManager.LoadScene("WoodlandScene"); // Replace "WoodlandScene" with the actual name of your scene
    }
}