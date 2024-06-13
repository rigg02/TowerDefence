using Array2DEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public int Wave;
    private int currentWave = 0;
    public GameObject[] Enemies;
    public bool[] isSpawned;
    [SerializeField] private Array2DInt numberOfEnemies;
    [SerializeField] private Array2DInt EnemyType;
    public float SpawnDelay;
    public float WaveDelay;

    // Start is called before the first frame update
    void Start()
    {
        SpawnNextWave();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWave < Wave && isSpawned[currentWave])
        {
            currentWave++;
            StartCoroutine(StartSpawning(currentWave));
        }
    }
    public void SpawnNextWave()
    {
        StartCoroutine(StartSpawning(currentWave));
    }
    IEnumerator StartSpawning(int i)
    {
        for (int j = 0; j < numberOfEnemies.GridSize.x; j++)
        {
            if(numberOfEnemies.GetCell(j,i) == 0)
            {
                continue;
            }
            yield return new WaitForSeconds(SpawnDelay * 2);
            for (int k = 0; k < numberOfEnemies.GetCell(j,i); k++)
            {
                yield return new WaitForSeconds(SpawnDelay);
                Instantiate(Enemies[EnemyType.GetCell(j,i)], transform.position, Quaternion.identity);
            }
        }
        isSpawned[i] = true;
    }
}
