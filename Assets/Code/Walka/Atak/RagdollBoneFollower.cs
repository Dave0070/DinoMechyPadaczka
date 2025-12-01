using UnityEngine;
using System.Collections;

public class RagdollHandController : MonoBehaviour
{
    [Header("Kość ragdolla (musi mieć Rigidbody)")]
    public Rigidbody boneRb;

    [Header("Punkt docelowy, do którego ręka ma podążać")]
    public Transform target;

    [Header("Auto wykrywanie obiektów")]
    public string detectionTag = "Player";
    public float detectionRadius = 2.0f;

    [Header("Cykliczne działania")]
    public float followDuration = 1.5f; // czas podążania
    public float waitTime = 2.0f;       // przerwa między cyklami

    [Header("Parametry ruchu")]
    public float positionForce = 200f;
    public float rotationForce = 50f;
    public float stopDistance = 0.05f;

    private bool followEnabled = false;
    private bool routineRunning = false;

    void Update()
    {
        // Jeżeli nie trwa cykl, sprawdzaj czy w pobliżu jest obiekt z tagiem
        if (!routineRunning)
        {
            Collider[] hits = Physics.OverlapSphere(boneRb.position, detectionRadius);

            foreach (var hit in hits)
            {
                if (hit.CompareTag(detectionTag))
                {
                    StartCoroutine(FollowRoutine());
                    break;
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (!followEnabled || target == null)
            return;

        // --- Jeśli ręka osiągnęła cel ---
        float dist = Vector3.Distance(boneRb.position, target.position);
        if (dist < stopDistance)
        {
            followEnabled = false;
            return;
        }

        // --- PODĄŻANIE POZYCJĄ ---
        Vector3 direction = (target.position - boneRb.position);
        boneRb.AddForce(direction * positionForce * Time.fixedDeltaTime, ForceMode.Acceleration);

        // --- PODĄŻANIE ROTACJĄ ---
        Quaternion deltaRot = target.rotation * Quaternion.Inverse(boneRb.rotation);
        deltaRot.ToAngleAxis(out float angle, out Vector3 axis);

        if (angle > 180f) angle -= 360f;

        if (Mathf.Abs(angle) > 0.01f)
            boneRb.AddTorque(axis * angle * rotationForce * Time.fixedDeltaTime, ForceMode.Acceleration);
    }

    IEnumerator FollowRoutine()
    {
        routineRunning = true;

        // WŁĄCZ podążanie
        followEnabled = true;

        // Ręka śledzi cel przez określony czas
        yield return new WaitForSeconds(followDuration);

        // Wyłącz podążanie → ręka wraca do ragdolla
        followEnabled = false;

        // Odczekaj przerwę
        yield return new WaitForSeconds(waitTime);

        routineRunning = false;
    }

    // Podgląd zasięgu w edytorze
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
