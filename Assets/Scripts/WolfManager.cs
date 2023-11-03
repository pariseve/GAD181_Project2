using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WolfManager : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player had died");

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
