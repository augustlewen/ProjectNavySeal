using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;
    public float spawnCooldown;
    private float spawnCountdown;
    private int direction = 1;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        spawnCountdown = spawnCooldown;

        GameObject seal = GameObject.FindGameObjectWithTag("Seal");
        if(transform.position.x < seal.transform.position.x)
        {
            direction = -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnCountdown > 0)
            spawnCountdown -= Time.deltaTime;
        else
        {
            Instantiate(enemy, transform.position, transform.rotation * Quaternion.Euler(0, 0, direction * 90));
            

            spawnCountdown = spawnCooldown;
        }
    }
}
