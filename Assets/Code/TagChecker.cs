using UnityEngine;

public class TagChecker : MonoBehaviour
{
    // Tag, który bêdziemy sprawdzaæ (mo¿esz zmieniæ w inspektorze Unity)
    public string tagToCheck = "Player";

    // Lista elementów UI (GameObjects na Canvasie), które maj¹ siê w³¹czyæ, jeœli nie ma obiektu z tagiem
    public GameObject[] uiElementsToEnable;

    void Update()
    {
        // ZnajdŸ wszystkie obiekty z podanym tagiem
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tagToCheck);

        // Jeœli nie ma ¿adnych obiektów z tym tagiem
        if (objectsWithTag.Length == 0)
        {
            // W³¹cz wszystkie elementy UI
            foreach (GameObject uiElement in uiElementsToEnable)
            {
                if (uiElement != null)
                {
                    uiElement.SetActive(true);
                }
            }
        }
        else
        {
            // Opcjonalnie: wy³¹cz elementy UI, jeœli obiekt z tagiem jest obecny (mo¿esz to usun¹æ, jeœli nie chcesz)
            foreach (GameObject uiElement in uiElementsToEnable)
            {
                if (uiElement != null)
                {
                    uiElement.SetActive(false);
                }
            }
        }
    }
}
