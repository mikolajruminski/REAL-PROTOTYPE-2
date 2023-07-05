using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    //health
    [SerializeField] int numOfHearts;
    public int playerHealth = 3;
    [SerializeField] Image[] hearts;
    [SerializeField] Sprite fullHeart, emptyHeart;
    [SerializeField] PlayerController playerController;

    public bool isGameActive = true;

    //player related
    public bool hasWeapon = false;



    //singleton
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("Game manager is null");

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }
    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
        setPlayerHP();
        playerDeath();

        if (playerHealth > numOfHearts)
        {
            playerHealth = numOfHearts;
        }
    }

    void setPlayerHP()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < playerHealth)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            if (playerHealth == 0) 
            {
                hearts[i].sprite = emptyHeart;
            }
            if (i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    void playerDeath()
    {
        if (playerHealth < 1)
        {
            playerController.animator.SetTrigger("playerDeath");
            isGameActive = false;
            playerController.playerRB.isKinematic = true;
        }
    }

    public void takeDamage(int damage)
    {
        playerHealth -= damage;
    }
}
