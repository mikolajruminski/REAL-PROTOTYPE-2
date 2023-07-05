using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBombs : MonoBehaviour
{
    [SerializeField] GameObject bomb;
    [SerializeField] Transform spawnPoint;
    
    // Start is called before the first frame update
    void Start()
    {
        repeatedlySpawn();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void repeatedlySpawn() 
    {
        InvokeRepeating("spawnBombs", 1f, 3f);
    }

    void spawnBombs() 
    {
        Instantiate(bomb, spawnPoint.position, Quaternion.identity);
    }

   
}
