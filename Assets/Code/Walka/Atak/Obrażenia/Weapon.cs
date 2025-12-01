using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int damage = 10;
    public string targetTag = "enemy"; // Publiczna zmienna do ustawienia tagu celu

    private void OnCollisionEnter(Collision collision)
    {
        // Sprawdzenie, czy obiekt, który zosta³ uderzony, ma ustawiony tag
        if (collision.gameObject.CompareTag(targetTag))
        {
            Health health = collision.gameObject.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damage);
                Debug.Log("Cube A zada³ " + damage + " obra¿eñ.");
            }
        }
    }
}
