using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    public GameObject[] pickupOptions;
    public int aliveTime = 15; // Time pickup is alive
    public int respawnTime = 10; // Time pickup is dead
    GameObject currentPickup;

    void Awake()
    {
        SpawnRandom();
    }

    IEnumerator WaitAndSpawn()
    {
        yield return new WaitForSeconds(respawnTime);
        SpawnRandom();
    }

    IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(aliveTime);
        if (currentPickup) Destroy(currentPickup);
        StartCoroutine(WaitAndSpawn());
    }

    void SpawnRandom()
    {
        GameObject pickup = pickupOptions[Random.Range(0, pickupOptions.Length)];
        currentPickup = Instantiate(pickup);
        currentPickup.transform.parent = transform;
        currentPickup.transform.position = transform.position;
        StartCoroutine(WaitAndDestroy());
    }
}
