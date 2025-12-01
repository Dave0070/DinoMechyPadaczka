using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public float maxHealth = 100f; // Maksymalne zdrowie
    public float currentHealth; // Aktualne zdrowie (zmienione na public)
    public Slider healthSlider; // Referencja do suwaka

    private void Awake()
    {
        // Upewnij si�, �e slider jest przypisany, je�eli nie, znajd� go w hierarchii
        if (healthSlider == null)
        {
            healthSlider = GetComponentInChildren<Slider>();
        }
    }

    private void Start()
    {
        currentHealth = maxHealth; // Ustaw aktualne zdrowie na maksymalne
        UpdateHealthSlider(); // Zaktualizuj suwak
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage; // Zmniejsz zdrowie
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Upewnij si�, �e zdrowie nie spadnie poni�ej 0
        UpdateHealthSlider(); // Zaktualizuj suwak

        if (currentHealth <= 0)
        {
            Debug.Log("Obiekt zosta� zniszczony!");
            // Mo�esz doda� tutaj logik�, co si� stanie, gdy zdrowie spadnie do 0
        }
    }

    private void UpdateHealthSlider()
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth / maxHealth; // Ustaw warto�� suwaka
            Debug.Log($"Slider aktualizowany: {healthSlider.value}"); // Debug log
        }
    }
}