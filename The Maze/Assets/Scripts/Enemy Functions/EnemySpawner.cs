using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    public float SpawnDelay;
    public GameObject EnemyPrefab;
    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        WaitForSeconds wait = new WaitForSeconds(SpawnDelay);
        int spawnedEnemies = 0;

        while (spawnedEnemies < GameManager.NumberOfEnemies)
        {
            spawnedEnemies += TryToSpawnEnemy();
            yield return wait;
        }        
    }

    int TryToSpawnEnemy()
    {
        transform.localPosition = new Vector3(Random.Range(0, GameManager.Length), 5, Random.Range(0, GameManager.Width));
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 20f))
            {
                if (hit.collider.CompareTag("Floor"))
                {
                    GameObject present = Instantiate(EnemyPrefab, new Vector3(hit.point.x, hit.point.y + 0.5f, hit.point.z), Quaternion.identity, FindObjectOfType<MazeGenerator>().transform);
                    return 1;
                }
            }
        return 0;
    }
}
