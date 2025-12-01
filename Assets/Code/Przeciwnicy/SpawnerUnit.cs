using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerUnit : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject unitPrefab;
    public float spawnInterval = 5f;
    public int spawnAmount = 1;
    public Transform[] spawnPoints;

    private List<GameObject> spawnedUnits = new List<GameObject>();
    private Coroutine spawnRoutine; // Zmienna do przechowywania rutyny spawnowania

    private void Start()
    {
        if (unitPrefab == null)
        {
            Debug.LogError("Unit Prefab is not assigned in Spawner.");
            enabled = false;
            return;
        }

        spawnRoutine = StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            SpawnUnits();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnUnits()
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            int spawnIndex = i % spawnPoints.Length;
            Transform spawnPoint = spawnPoints[spawnIndex];
            if (spawnPoint != null)
            {
                GameObject unit = Instantiate(unitPrefab, spawnPoint.position, spawnPoint.rotation);
                spawnedUnits.Add(unit);
            }
            else
            {
                Debug.LogWarning($"Spawn point at index {spawnIndex} is null.");
            }
        }
    }

    public void RemoveUnit(GameObject unit)
    {
        if (spawnedUnits.Contains(unit))
        {
            spawnedUnits.Remove(unit);
            Debug.Log($"{unit.name} zosta�o usuni�te z listy spawner�w.");
        }
        else
        {
            Debug.LogWarning($"{unit.name} nie znajduje si� w li�cie spawner�w.");
        }
    }

    private void Update()
    {
        for (int i = spawnedUnits.Count - 1; i >= 0; i--)
        {
            if (spawnedUnits[i] == null)
            {
                spawnedUnits.RemoveAt(i);
            }
        }
    }

    // Metoda do wstrzymywania spawnowania
    public void ToggleSpawner(bool isActive)
    {
        if (isActive)
        {
            if (spawnRoutine == null)
            {
                spawnRoutine = StartCoroutine(SpawnRoutine());
            }
        }
        else
        {
            if (spawnRoutine != null)
            {
                StopCoroutine(spawnRoutine);
                spawnRoutine = null;
            }
        }
    }
}