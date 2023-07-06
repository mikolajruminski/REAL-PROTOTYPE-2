using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBombs : MonoBehaviour
{
    [SerializeField] GameObject bomb;
    [SerializeField] Transform spawnPoint;
    [SerializeField] float repeatRate;
   
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
        InvokeRepeating("spawnBombs", 1f, repeatRate);
    }

    void spawnBombs() 
    {
        Instantiate(bomb, spawnPoint.position, Quaternion.identity);
    }

   
}
