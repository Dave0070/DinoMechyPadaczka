using Unity.VisualScripting;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public int rewardMoney = 50;
    private ObjectPlacer objectPlacer;

    [Header("Tag obiektu zadaj¹cego obra¿enia")]
    public string damageDealerTag;  // <-- tag wpisywany w Inspectorze

    private void Start()
    {
        currentHealth = maxHealth;
        objectPlacer = FindObjectOfType<ObjectPlacer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // SprawdŸ, czy obiekt zderzaj¹cy siê ma skrypt Weapon
        Weapon weapon = collision.gameObject.GetComponent<Weapon>();

        // Zadaj obra¿enia tylko jeœli ma odpowiedni tag wpisany w Inspectorze
        if (weapon != null && collision.gameObject.CompareTag(damageDealerTag))
        {
            TakeDamage(weapon.damage);
            Debug.Log($"{gameObject.name} otrzyma³ {weapon.damage} obra¿eñ.");
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log($"{gameObject.name} pozosta³o {currentHealth} HP.");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log($"{gameObject.name} zosta³ zniszczony.");

        if (objectPlacer != null)
        {
            objectPlacer.AddMoney(rewardMoney);
        }

        SpawnerUnit spawner = FindObjectOfType<SpawnerUnit>();
        if (spawner != null)
        {
            spawner.RemoveUnit(gameObject);
        }

        Destroy(gameObject);
    }
}
