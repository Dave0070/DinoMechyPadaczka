using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackMovement : MonoBehaviour
{
    [Header("Primary Target")]
    public string targetTag = "White";

    [Header("Fallback Target")]
    public string fallbackTag = "Fallback"; // <<< NOWY TAG

    public float moveSpeed = 5f;
    public float stoppingDistance = 0f;
    public float rotationSpeed = 5f;

    private Transform targetTransform;

    private void Start()
    {
        this.enabled = false;
    }

    void Update()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);

        // Jeœli nie ma g³ównego celu, sprawdzamy fallback
        if (targets.Length == 0)
        {
            targets = GameObject.FindGameObjectsWithTag(fallbackTag);

            // Jeœli dalej brak — nie ruszaj
            if (targets.Length == 0)
                return;
        }

        targetTransform = FindClosestTarget(targets);

        if (targetTransform != null)
        {
            MoveTowardsTarget(targetTransform);
        }
    }

    private Transform FindClosestTarget(GameObject[] targets)
    {
        Transform closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject target in targets)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = target.transform;
            }
        }

        return closest;
    }

    private void MoveTowardsTarget(Transform target)
    {
        float distance = Vector3.Distance(transform.position, target.position);

        if (distance > stoppingDistance)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            direction.y = 0;

            transform.position += direction * moveSpeed * Time.deltaTime;

            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);

                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.Euler(270, lookRotation.eulerAngles.y, 0),
                    Time.deltaTime * rotationSpeed
                );
            }
        }
    }

    public void ToggleMovement(bool isActive)
    {
        this.enabled = isActive;
    }
}
