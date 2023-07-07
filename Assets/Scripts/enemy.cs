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
    Transform playerPosition;

    //collision check
    [SerializeField] Transform obstacleCheck;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask wallLayer;
    [SerializeField] LayerMask groundLayer;

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
        randomDirX();
    }

    // Update is called once per frame
    void Update()
    {
        changeDirection();
        flipSprite();
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

    bool collisionChecking(Transform checkPoint)
    {
        return Physics2D.OverlapCircle(checkPoint.position, 0.2f);
    }

    void changeDirection()
    {
        if (collisionChecking(obstacleCheck))
        {
            StartCoroutine(waitWhenColliding());
            dirX *= -1;
        }
        else if (!collisionChecking(groundCheck))
        {
            StartCoroutine(waitWhenColliding());
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

    int randomDirX()
    {
        dirX = Random.Range(-1, 2);

        if (dirX == 0)
        {
            dirX += 1;
        }

        return dirX;
    }

    IEnumerator waitWhenColliding()
    {
        float orgMov = movementSpeed;
        movementSpeed = 0;
        int secondsCount = Random.Range(1, 5);
        yield return new WaitForSeconds(secondsCount);
        movementSpeed = orgMov;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>()) 
        {
            var player = other.gameObject.GetComponent<PlayerController>();
            GameManager.Instance.takeDamage(1);
            player.knockbackCounter = player.knockbackLength;
            if (other.transform.position.x < transform.position.x) 
            {
                player.knockbackFromRight = true;
            }
            else 
            {
                player.knockbackFromRight = false;
            }
        }

    }
}
