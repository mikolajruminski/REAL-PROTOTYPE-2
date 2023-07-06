using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoBoom : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rb;
    [SerializeField] public float speed = 8f;
    Collider2D col;
    float timeToExplode = 2f;
    [SerializeField] bool fromTop = true;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        if (fromTop)
        {
            rb.velocity = Vector2.down * speed;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        col.enabled = false;
        anim.Play("goBoom");
        speed = 0f;

        if (other.gameObject.GetComponent<PlayerController>())
        {
            GameManager.Instance.takeDamage(1);
        }
    }
}
