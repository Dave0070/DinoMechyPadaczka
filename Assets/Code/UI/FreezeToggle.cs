using System.Collections.Generic;
using UnityEngine;

public class FreezeToggle : MonoBehaviour
{
    [Tooltip("Assign the prefab instances in the scene that you want to freeze/unfreeze.")]
    public List<GameObject> selectedInstances = new List<GameObject>();

    private bool isFrozen = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (isFrozen)
            {
                UnfreezeAll();
            }
            else
            {
                FreezeAll();
            }
            isFrozen = !isFrozen;
        }
    }

    private void FreezeAll()
    {
        foreach (var instance in selectedInstances)
        {
            if (instance == null) continue;

            Rigidbody[] bodies = instance.GetComponentsInChildren<Rigidbody>(true);
            foreach (var rb in bodies)
            {
                // Freeze position and rotation by locking constraints
                rb.constraints = RigidbodyConstraints.FreezeAll;

                // Optionally zero out velocities to avoid momentum carry over
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }

    private void UnfreezeAll()
    {
        foreach (var instance in selectedInstances)
        {
            if (instance == null) continue;

            Rigidbody[] bodies = instance.GetComponentsInChildren<Rigidbody>(true);
            foreach (var rb in bodies)
            {
                // Remove all constraints to allow full physics simulation
                rb.constraints = RigidbodyConstraints.None;
                // Velocities can remain zero or be managed by physics naturally
            }
        }
    }
}
