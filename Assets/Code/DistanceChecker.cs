using UnityEngine;

public class DistanceChecker : MonoBehaviour
{
    public string targetTag; // Tag, który chcesz wykrywać
    public float detectionDistance = 5f; // Odległość, w której obiekt będzie wykrywany
    public float rotationSpeed = 2f; // Prędkość rotacji
    public GameObject partToRotate; // Obiekt, który ma zmieniać rotację
    public RagdollHandRotator ragdollHandRotator; // Referencja do skryptu RagdollHandRotator

    private Quaternion targetRotation; // Docelowa rotacja
    private bool isRotating = false; // Flaga, czy obiekt jest w trakcie rotacji

    void Update()
    {
        // Znajdź wszystkie obiekty z danym tagiem
        GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);
        bool targetInRange = false;

        foreach (GameObject target in targets)
        {
            // Oblicz odległość między tym obiektem a obiektem, do którego przypisany jest ten skrypt
            float distance = Vector3.Distance(transform.position, target.transform.position);

            // Sprawdź, czy odległość jest mniejsza lub równa zadanej odległości
            if (distance <= detectionDistance)
            {
                targetInRange = true;
                ragdollHandRotator.Activate(); // Aktywuj RagdollHandRotator
                break; // Nie musimy sprawdzać dalej, jeśli już znaleźliśmy obiekt w zasięgu
            }
        }

        // Jeżeli obiekt z tagiem nie jest w zasięgu, dezaktywuj RagdollHandRotator
        if (!targetInRange)
        {
            ragdollHandRotator.Deactivate(); // Dezaktywuj RagdollHandRotator
            if (isRotating)
            {
                targetRotation = Quaternion.Euler(partToRotate.transform.rotation.eulerAngles.x, 0f, partToRotate.transform.rotation.eulerAngles.z); // Ustaw rotację na domyślną
            }
        }

        // Płynna rotacja do docelowej rotacji
        if (isRotating)
        {
            // Płynna rotacja do docelowej rotacji
            partToRotate.transform.rotation = Quaternion.RotateTowards(partToRotate.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Sprawdź, czy osiągnięto docelową rotację
            if (Quaternion.Angle(partToRotate.transform.rotation, targetRotation) < 0.1f)
            {
                // Zatrzymaj rotację, jeżeli osiągnięto docelową rotację
                isRotating = false; // Resetuj flagę rotacji
                Debug.Log("Rotacja zakończona.");
            }
        }
    }
}