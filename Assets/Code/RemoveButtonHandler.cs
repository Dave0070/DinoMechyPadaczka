using UnityEngine;
using UnityEngine.UI;

public class RemoveButtonHandler : MonoBehaviour
{
    public ObjectPlacer objectPlacer; // Referencja do skryptu ObjectPlacer
    public Button removeButton; // Przycisk do usuwania Cube'ów

    void Start()
    {
        removeButton.onClick.AddListener(RemoveCubes);
    }

    private void RemoveCubes()
    {
        objectPlacer.ToggleRemovingMode(true); // W³¹cz tryb usuwania
        Debug.Log("Tryb usuwania aktywowany. Kliknij na Cube, aby je usun¹æ.");
    }
}