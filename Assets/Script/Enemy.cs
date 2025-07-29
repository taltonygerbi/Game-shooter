using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public Transform player;
    public float MoveSpeed = 2.0f;
    public float Health = 50f;
    public GameObject cubePrefab;
    public int spawnCount = 2;
    public float respawnRangeX = 5f;
    private Vector3 startPosition;

    private SystemScore systemScore; // Reference to SystemScore

    private void Start()
    {
        startPosition = transform.position;
        systemScore = FindObjectOfType<SystemScore>(); // Find the SystemScore script in the scene
    }

    private void Update()
    {
        if (player == null)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < 1.0f)
        {
            Attack();
        }
        else
        {
            MoveTowardsPlayer();
        }
    }

    private void MoveTowardsPlayer()
    {
        if (player == null) return;
        transform.LookAt(player);
        transform.position = Vector3.MoveTowards(transform.position, player.position, MoveSpeed * Time.deltaTime);
    }

    private void Attack()
    {
        Debug.Log("Enemy Attack");
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
        Debug.Log("Enemy took damage! Health: " + Health);

        if (Health <= 0)
        {
            // Update score and kill count
            if (systemScore != null)
            {
                systemScore.AddScore();
                systemScore.AddKill();
            }
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        Debug.Log("Enemy Died!");
        GetComponent<MeshRenderer>().enabled = false;
        yield return new WaitForSeconds(1.5f);
        Respawn();
    }

    private void Respawn()
    {
        Debug.Log("Enemy Respawned with new cubes!");
        float newX = startPosition.x + Random.Range(-respawnRangeX, respawnRangeX);
        Vector3 newPosition = new Vector3(newX, startPosition.y, startPosition.z);

        Health = 50f;
        transform.position = newPosition;
        GetComponent<MeshRenderer>().enabled = true;

        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 spawnPosition = newPosition + new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2));
            Instantiate(cubePrefab, spawnPosition, Quaternion.identity);
        }
    }
}
