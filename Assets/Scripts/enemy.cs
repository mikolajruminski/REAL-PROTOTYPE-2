using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    [SerializeField] int healthPoints;
    [SerializeField] GameObject deathAnimation;
    SpriteRenderer spr;
    Color crColor;
    // Start is called before the first frame update
    void Start()
    {
     spr = GetComponent<SpriteRenderer>();
     crColor = spr.color;
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    public void TakeDamage(int damage)
    {
        healthPoints -= damage;
        StartCoroutine(flashDamageTaken());
        if (healthPoints <= 0) 
        {
            Destroy(gameObject);
            Instantiate(deathAnimation, transform.position, Quaternion.identity);
        }
    }

    IEnumerator flashDamageTaken() 
    {
        spr.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        spr.color = crColor;
    }

}
