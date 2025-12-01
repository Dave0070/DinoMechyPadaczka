using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject aiPrefab; // Prefab AI do instancjonowania

    public void OnStartButtonClicked()
    {
        // Znajdü wszystkie obiekty AI w scenie
        AIController[] aiControllers = FindObjectsOfType<AIController>();

        // Uruchom bieg dla kaødego AI
        foreach (AIController ai in aiControllers)
        {
            ai.StartRunning();
        }
    }
}