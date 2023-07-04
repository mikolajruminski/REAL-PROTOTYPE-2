using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform shootPoint;
    bool canShoot = true;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L) && canShoot && GameManager.Instance.hasWeapon) 
        {
            StartCoroutine(shootBullets());
        }
    }

    IEnumerator shootBullets() 
    {
        Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        animator.SetBool("isShooting", true);
        canShoot = false;
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("isShooting", false);
        canShoot = true;
    }
}
