using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public string woodlandScene = "WoodlandScene";

    private void Start()
    {
        
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    // call method when 'return' button is clicked
    private void OnButtonClick()
    {
        Debug.Log("Button clicked - switching to WoodlandScene");
        SceneManager.LoadScene(woodlandScene);
    }
}