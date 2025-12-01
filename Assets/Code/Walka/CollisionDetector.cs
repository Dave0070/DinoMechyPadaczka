using UnityEngine;
using System.Collections;

public class CollisionDetector : MonoBehaviour
{
    public string[] tagsToDetect; // Lista tagów, które będą wykrywane
    public float ignoreTime = 2f; // Czas, przez jaki dotknięcie będzie ignorowane
    private bool isIgnoringCollision = false;
    public float damageAmount = 10f; // Ilość zadawanych obrażeń

    private void OnCollisionEnter(Collision collision)
    {
        foreach (string tag in tagsToDetect)
        {
            if (collision.gameObject.CompareTag(tag))
            {
                if (!isIgnoringCollision)
                {
                    Debug.Log($"Obiekt {gameObject.name} dotknął obiektu z tagiem: {tag}");
                    HealthManager healthManager = collision.gameObject.GetComponent<HealthManager>();
                    if (healthManager != null)
                    {
                        healthManager.TakeDamage(damageAmount); // Zadaj obrażenia
                    }
                    StartCoroutine(IgnoreCollision()); // Użyj poprawnej nazwy metody
                }
                break; // Przerwij pętlę, jeśli już wykryto kolizję
            }
        }
    }

    private IEnumerator IgnoreCollision()
    {
        isIgnoringCollision = true;
        yield return new WaitForSeconds(ignoreTime);
        isIgnoringCollision = false;
    }
}
