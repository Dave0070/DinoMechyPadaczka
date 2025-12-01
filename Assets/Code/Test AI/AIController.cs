using UnityEngine;

public class AIController : MonoBehaviour
{
    public float speed = 5f; // Prêdkoœæ poruszania siê
    public float changeDirectionTime = 2f; // Czas do zmiany kierunku
    private Vector3 direction; // Kierunek ruchu
    private float timer; // Timer do zmiany kierunku
    private bool isRunning = false; // Flaga, czy AI biega

    void Start()
    {
        // Inicjalizuj kierunek
        ChangeDirection();
    }

    void Update()
    {
        if (isRunning)
        {
            // Poruszaj AI
            transform.position += direction * speed * Time.deltaTime;

            // Zwiêksz timer
            timer += Time.deltaTime;

            // SprawdŸ, czy czas na zmianê kierunku
            if (timer >= changeDirectionTime)
            {
                ChangeDirection();
                timer = 0f; // Resetuj timer
            }
        }
    }

    public void StartRunning()
    {
        isRunning = true; // Ustaw flagê na true, aby AI zaczê³o biegaæ
    }

    private void ChangeDirection()
    {
        // Losowy kierunek
        direction = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
    }
}