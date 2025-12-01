using UnityEngine;
using System.Collections.Generic;
using TMPro; // Dodano TextMeshPro
using UnityEngine.UI; // Dodano Button

[System.Serializable]
public class Unit
{
    public GameObject cubePrefab; // Prefabrykat Cube
    public GameObject modelPrefab; // Prefabrykat modelu
    public Button placeButton; // Przycisk do stawiania jednostki
    public int cost; // Cena jednostki
}

public class ObjectPlacer : MonoBehaviour
{
    public Unit[] units; // Tablica jednostek
    private GameObject currentPreview; // Hologram podglądowy
    private int selectedUnitIndex = -1; // Indeks wybranej jednostki
    private Dictionary<Vector3, GameObject> placedCubes = new Dictionary<Vector3, GameObject>(); // Słownik postawionych Cube'ów
    public bool isRemovingMode = false; // Zmienna do przechowywania stanu trybu
    public int playerMoney = 100; // Początkowa ilość pieniędzy gracza
    public TMP_Text moneyText; // Referencja do komponentu TextMeshPro

    void Start()
    {
        // Przypisz funkcję do każdego przycisku stawiania jednostki
        foreach (Unit unit in units)
        {
            unit.placeButton.onClick.AddListener(() => SelectUnit(System.Array.IndexOf(units, unit)));
        }
        UpdateMoneyText(); // Zaktualizuj tekst na początku
    }

    void Update()
    {
        // Hologram podglądowy
        if (selectedUnitIndex != -1 && !isRemovingMode)
        {
            UpdatePreview();
        }

        // Sprawdź, czy lewy przycisk myszy jest wciśnięty
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Sprawdź, czy kliknięto w platformę
                if (hit.transform.CompareTag("Part"))
                {
                    if (isRemovingMode)
                    {
                        RemoveCubeAtPosition(hit.transform.position); // Usuń Cube przypisany do platformy
                    }
                    else
                    {
                        PlaceCube(hit.transform.position); // Postaw Cube na platformie
                    }
                }
            }
        }
    }

    public void ToggleRemovingMode(bool state)
    {
        isRemovingMode = state; // Ustaw stan trybu usuwania
        if (isRemovingMode)
        {
            Debug.Log("Tryb usuwania aktywowany.");
        }
        else
        {
            Debug.Log("Tryb usuwania wyłączony.");
        }
    }

    private void UpdatePreview()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.CompareTag("Part"))
            {
                Vector3 position = hit.transform.position;
                position.y += 0.5f; // Ustaw wysokość na 0.5, aby unieść Cube nad powierzchnię
                currentPreview.transform.position = position;
            }
        }
    }

    private void PlaceCube(Vector3 position)
    {
        if (selectedUnitIndex != -1)
        {
            if (!placedCubes.ContainsKey(position)) // Sprawdź, czy na danej pozycji nie ma już Cube'a
            {
                if (playerMoney >= units[selectedUnitIndex].cost) // Sprawdź, czy gracz ma wystarczającą ilość pieniędzy
                {
                    GameObject newCube = Instantiate(units[selectedUnitIndex].cubePrefab, position, Quaternion.identity);
                    newCube.tag = "Cube"; // Ustaw tag dla nowego sześcianu
                    placedCubes[position] = newCube; // Dodaj do słownika postawionych Cube'ów
                    playerMoney -= units[selectedUnitIndex].cost; // Odejmij koszt jednostki od pieniędzy gracza
                    UpdateMoneyText(); // Zaktualizuj wyświetlaną ilość pieniędzy
                }
                else
                {
                    Debug.Log("Nie masz wystarczającej ilości pieniędzy, aby postawić tę jednostkę.");
                }
            }
            else
            {
                Debug.Log("Na tej platformie już stoi jednostka.");
            }
        }
    }

    private void RemoveCubeAtPosition(Vector3 position)
    {
        if (placedCubes.ContainsKey(position)) // Sprawdź, czy na danej pozycji jest Cube
        {
            GameObject cubeToRemove = placedCubes[position];
            RemoveSelectedCube(cubeToRemove); // Usuń zaznaczony sześcian
        }
        else
        {
            Debug.Log("Brak jednostki do usunięcia na tej platformie.");
        }
    }

    public void RemoveSelectedCube(GameObject cube)
    {
        if (cube != null)
        {
            Vector3 position = cube.transform.position;
            placedCubes.Remove(position); // Usuń wybrany Cube z słownika
            Destroy(cube); // Zniszcz wybrany Cube
            Debug.Log("Usunięto Cube.");
        }
    }

    public void RemoveCubes()
    {
        foreach (GameObject cube in placedCubes.Values)
        {
            Destroy(cube); // Usuń każdy postawiony Cube
        }
        placedCubes.Clear(); // Wyczyść słownik
    }

    // Dodana metoda SelectUnit
    public void SelectUnit(int index)
    {
        selectedUnitIndex = index; // Ustaw indeks wybranej jednostki
        if (currentPreview != null)
        {
            Destroy(currentPreview); // Zniszcz aktualny podgląd, jeśli istnieje
        }
        currentPreview = Instantiate(units[selectedUnitIndex].modelPrefab, Vector3.zero, Quaternion.identity); // Stwórz nowy podgląd
        currentPreview.tag = "Preview"; // Ustaw tag dla podglądu
    }

    // Dodaj metodę do dodawania pieniędzy
    public void AddMoney(int amount)
    {
        playerMoney += amount;
        UpdateMoneyText(); // Zaktualizuj wyświetlaną ilość pieniędzy
        Debug.Log($"Dodano {amount} pieniędzy. Aktualna ilość pieniędzy: {playerMoney}");
    }

    // Metoda do aktualizacji tekstu wyświetlającego ilość pieniędzy
    private void UpdateMoneyText()
    {
        if (moneyText != null)
        {
            moneyText.text = "" + playerMoney.ToString();
        }
    }
}
