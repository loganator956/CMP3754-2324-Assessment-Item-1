using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    private const float MIN_SPAWN_FREQ = 1f;
    private const float MAX_SPAWN_FREQ = 6f;

    private float _nextSpawnDelta = 1f;
    private float _spawnT = 0f;

    public GameObject[] CarPrefabs;
    public Vector3[] Way1;
    public Vector3[] Way2;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _spawnT += Time.deltaTime;
        if (_spawnT > _nextSpawnDelta)
        {
            _spawnT = 0f;
            _nextSpawnDelta = Random.Range(MIN_SPAWN_FREQ, MAX_SPAWN_FREQ);
            Vector3 spawnPos = Way1[0];
            Vector3 targetPos = Way1[1];
            if (Random.Range(0, 2) != 0)
            {
                spawnPos = Way2[0];
                targetPos = Way2[1];
            }
            GameObject newCar = Instantiate(CarPrefabs[Random.Range(0,CarPrefabs.Length)]);
            newCar.transform.position = spawnPos;
            newCar.GetComponent<CarController>().TargetPos = targetPos;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(Way1[0], 0.25f);
        Gizmos.DrawSphere(Way2[0], 0.25f);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(Way1[1], 0.25f);
        Gizmos.DrawSphere(Way2[1], 0.25f);
    }
}
