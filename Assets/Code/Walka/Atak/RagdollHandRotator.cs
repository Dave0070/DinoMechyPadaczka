using UnityEngine;

public class RagdollHandRotator : MonoBehaviour
{
    [Header("Obiekt do rotacji")]
    public Transform hand; // Obiekt, który ma byæ rotowany
    private Rigidbody handRigidbody; // Rigidbody obiektu

    [Header("Docelowa rotacja")]
    public Vector3 targetRotation; // Docelowa rotacja w stopniach

    [Header("Prêdkoœæ rotacji")]
    public float rotationSpeed = 1.0f; // Prêdkoœæ rotacji

    private bool isActive = false; // Flaga aktywacji

    void Start()
    {
        // Pobierz Rigidbody z obiektu
        handRigidbody = hand.GetComponent<Rigidbody>();
        if (handRigidbody == null)
        {
            Debug.LogError("Brak komponentu Rigidbody na obiekcie rêki!");
        }
    }

    void FixedUpdate()
    {
        if (isActive) // SprawdŸ, czy skrypt jest aktywny
        {
            // Oblicz docelow¹ rotacjê w postaci Quaternion
            Quaternion targetQuaternion = Quaternion.Euler(targetRotation);

            // Oblicz ró¿nicê rotacji
            Quaternion deltaRotation = targetQuaternion * Quaternion.Inverse(hand.rotation);

            // Oblicz moment obrotowy
            Vector3 torque = new Vector3(deltaRotation.x, deltaRotation.y, deltaRotation.z) * rotationSpeed;

            // Zastosuj moment obrotowy do Rigidbody
            handRigidbody.AddTorque(torque, ForceMode.VelocityChange);
        }
    }

    // Metoda do ustawiania nowej docelowej rotacji
    public void SetTargetRotation(Vector3 newRotation)
    {
        targetRotation = newRotation;
    }

    // Metoda do aktywacji skryptu
    public void Activate()
    {
        isActive = true;
    }

    // Metoda do dezaktywacji skryptu
    public void Deactivate()
    {
        isActive = false;
    }
}