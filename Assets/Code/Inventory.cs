using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public ObjectPlacer objectPlacer; // Referencja do skryptu ObjectPlacer
    public Button[] unitButtons; // Przycisk dla ka¿dej jednostki
    public Button removeButton; // Przycisk do usuwania Cube'ów

    void Start()
    {
        for (int i = 0; i < unitButtons.Length; i++)
        {
            int index = i; // Zapisz indeks w lokalnej zmiennej
            unitButtons[i].onClick.AddListener(() => SelectUnit(index));
        }

        removeButton.onClick.AddListener(ToggleRemoveMode);
    }

    private void SelectUnit(int index)
    {
        objectPlacer.ToggleRemovingMode(false); // Wy³¹cz tryb usuwania przy wyborze jednostki
        objectPlacer.SelectUnit(index); // Wywo³aj metodê SelectUnit
    }

    private void ToggleRemoveMode()
    {
        objectPlacer.ToggleRemovingMode(true); // W³¹cz tryb usuwania
    }
}
