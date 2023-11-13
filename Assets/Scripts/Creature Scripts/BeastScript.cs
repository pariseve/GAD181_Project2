using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BeastScript : MonoBehaviour
{
    public int totalSacrificeValue = 0;
    public TextMeshProUGUI satisfactionText; // Reference to the TextMeshProUGUI component for displaying text

    public void ReceiveSacrifice(SacrificeObject sacrifice)
    {
        if (sacrifice != null)
        {
            Debug.Log("ReceiveSacrifice method called");
            totalSacrificeValue += sacrifice.sacrificeValue;

            // Display satisfaction text
            DisplaySatisfactionText(sacrifice.satiationAmount);

            // Add any additional logic or effects you want when receiving a sacrifice
            Debug.Log($"Beast received {sacrifice.name}. Current total sacrifice value: {totalSacrificeValue}");
        }
        else
        {
            Debug.LogWarning("Invalid sacrifice item");
        }
    }

    private void DisplaySatisfactionText(string satiationAmount)
    {
        if (satisfactionText != null)
        {
            satisfactionText.text = $"He is {satiationAmount} satisfied";
            StartCoroutine(HideSatisfactionTextAfterDelay(5f)); // Display for 5 seconds
        }
        else
        {
            Debug.LogWarning("Satisfaction text component not assigned. Please assign it in the inspector.");
        }
    }

    private IEnumerator HideSatisfactionTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (satisfactionText != null)
        {
            satisfactionText.text = ""; // Clear the text
        }
    }

}


