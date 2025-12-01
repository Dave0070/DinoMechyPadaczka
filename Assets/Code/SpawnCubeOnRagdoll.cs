using UnityEngine;

public class SpawnCubeOnRagdoll : MonoBehaviour
{
    public GameObject cubePrefab; // Prefab Cube
    public Transform attachPoint; // Punkt, do którego przypisujemy Cube
    private GameObject spawnedCube; // Referencja do Cube

    void Start()
    {
        if (attachPoint == null)
        {
            attachPoint = transform; // Default to current transform if not set
        }

        if (spawnedCube == null && cubePrefab != null)
        {
            // Instancjonowanie prefabrykatu Cube
            spawnedCube = Instantiate(cubePrefab);

            // Ustawienie rodzica dla zinstancjonowanego obiektu
            spawnedCube.transform.SetParent(attachPoint, false); // Nie zmieniaj przestrzeni świata

            // Resetowanie pozycji i rotacji Cube względem attachPoint
            spawnedCube.transform.localPosition = Vector3.zero;
            spawnedCube.transform.localRotation = Quaternion.identity;

            SetupPhysics(); // Konfiguracja fizyki Cube
        }

        // Ignorowanie kolizji Cube z Ragdollem
        IgnoreCollisionsWithRagdoll();
    }

    void SetupPhysics()
    {
        // Upewniamy się, że Cube ma Rigidbody
        Rigidbody cubeRb = spawnedCube.GetComponent<Rigidbody>();
        if (cubeRb == null)
        {
            cubeRb = spawnedCube.AddComponent<Rigidbody>();
        }
        cubeRb.isKinematic = false; // Pozwalamy Cube na interakcję z fizyką
        cubeRb.useGravity = true; // Cube powinien normalnie podlegać grawitacji

        // Upewniamy się, że attachPoint (ragdoll) też ma Rigidbody
        Rigidbody attachRb = attachPoint.GetComponent<Rigidbody>();
        if (attachRb == null)
        {
            attachRb = attachPoint.gameObject.AddComponent<Rigidbody>();
        }
        attachRb.isKinematic = false; // AttachPoint też może się poruszać

        // Połączenie Cube z attachPoint fizycznie za pomocą FixedJoint
        FixedJoint joint = spawnedCube.AddComponent<FixedJoint>();
        joint.connectedBody = attachRb;
        joint.breakForce = Mathf.Infinity; // Nie chcemy, by joint się zerwał
        joint.breakTorque = Mathf.Infinity;
    }

    void IgnoreCollisionsWithRagdoll()
    {
        Collider[] ragdollColliders = GetComponentsInChildren<Collider>(); // Pobranie koliderów ragdolla
        Collider cubeCollider = spawnedCube?.GetComponent<Collider>(); // Pobranie kolidera Cube

        if (cubeCollider != null)
        {
            // Ignorowanie kolizji Cube z koliderami Ragdolla
            foreach (Collider ragdollCollider in ragdollColliders)
            {
                Physics.IgnoreCollision(ragdollCollider, cubeCollider);
            }
        }
    }
}
