using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    [SerializeField] int healthPoints;
    [SerializeField] GameObject deathAnimation;
    SpriteRenderer spr;
    Rigidbody2D rb;
    Color crColor;

    [SerializeField] Transform obstacleCheck;
    [SerializeField] LayerMask wallLayer;

    //moving
    [SerializeField] float movementSpeed = 3f;
    int dirX;
    Vector3 localScale;
    bool facingRight;
    // Start is called before the first frame update
    void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        crColor = spr.color;
        localScale = transform.localScale;
        dirX = Random.Range(-1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        changeDirection();
        flipSprite();
        obstacleChecking();
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(dirX * movementSpeed, rb.velocity.y);
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

    bool obstacleChecking()
    {
        return Physics2D.OverlapCircle(obstacleCheck.position, 0.2f);
    }

    void changeDirection()
    {
        if (obstacleChecking())
        {
            dirX *= -1;
        }
    }

    void flipSprite()
    {
        if (dirX > 0)
        {
            facingRight = true;
        }
        else if (dirX < 0)
        {
            facingRight = false;
        }

        if (((facingRight) && (localScale.x < 0) || ((!facingRight) && (localScale.x > 0))))
        {
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }
}
