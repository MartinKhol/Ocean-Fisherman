using System.Collections;
using UnityEngine;

//class that instantiates new prefabs every given time interval until maximum amount is reached
public class FishSpawner : MonoBehaviour
{
    [SerializeField] private float timeInterval = 5f;
    [SerializeField] private int maxObjects = 150; //default value for maximum amount of opbjects spawned
    [SerializeField] private GameObject[] fishPool; //pool of prefabs to spawn from

    void Start()
    {
        StartCoroutine(SpawnCycle()); //start the SpawnCycle coroutine immmediatly after instantiation
    }

    //coroutine for instantiating new prefab every timeInterval
    IEnumerator SpawnCycle()
    {
        while (true)
        {
            //wait for timeInterval before spawning new fish
            yield return new WaitForSeconds(timeInterval);

            //check if more prefabs can be spawned
            yield return new WaitUntil(() => Fish.fishCount < maxObjects);

            InstantiateRandomPrefab();
        }
    }

    private void InstantiateRandomPrefab()
    {
        int id = Random.Range(0, fishPool.Length);
        Instantiate(fishPool[id], transform.position, Quaternion.identity, transform);
    }
}

