using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetManager : MonoBehaviour
{
    public Button enableMovementButton; // Assign this in the Inspector

    private void Start()
    {
        // Ensure the button is assigned and add the listener
        if (enableMovementButton != null)
        {
            enableMovementButton.onClick.AddListener(EnableMovementScripts);
        }
    }

    private void EnableMovementScripts()
    {
        // Find all objects with the BlackMovement script and enable them
        BlackMovement[] movementScripts = FindObjectsOfType<BlackMovement>();

        foreach (BlackMovement movementScript in movementScripts)
        {
            movementScript.enabled = true; // Enable the script
        }
    }
}