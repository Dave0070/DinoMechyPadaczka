using UnityEngine;

public class CubeTouchDetector : MonoBehaviour
{
    // Publiczna zmienna do wyboru tagu w inspektorze
    public string tagToDetect = "black";

    void OnTriggerEnter(Collider other)
    {
        // Sprawdzenie, czy obiekt ma odpowiedni tag
        if (other.gameObject.CompareTag(tagToDetect))
        {
            Debug.Log("Cube A dotkn¹³ Cube B!");
        }
    }
}
