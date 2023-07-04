using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    // Start is called before the first frame update
    float speed = 20f;
    Rigidbody2D rb;
    [SerializeField] GameObject impactEffect;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(destroySelf());
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(transform.localScale.x * speed, 0f);
    }

    IEnumerator destroySelf()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.GetComponent<PlayerController>())
        {
            Instantiate(impactEffect, transform.position, Quaternion.identity);
        }

        enemy enemy = other.GetComponent<enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(1);
            Destroy(gameObject);
        }
    }


}
