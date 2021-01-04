using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float health = 500;
    [SerializeField] int scoreValue = 150;
    [SerializeField] float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] public GameObject projectile;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] public float fireRateEnemy = 0.8f;
    float nextFireEnemy = 0.0f;
    Vector2 bulletPos;


    // Start is called before the first frame update
    void Start()
    {
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if(shotCounter <= 0f)
        {
            if(nextFireEnemy < Time.time)
                Fire();
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
            //nextFire = Time.time + fireRate;

        }
    }

    private void Fire()
    {
        nextFireEnemy = Time.time + fireRateEnemy;
        bulletPos = transform.position;
        bulletPos += new Vector2(0f, -projectileSpeed);
        Instantiate(projectile, bulletPos, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if(!damageDealer)
            return;
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.hit();
        if (health <= 0)
        {
            FindObjectOfType<GameSession>().AddToScore(scoreValue);
            Destroy(gameObject);
        }
    }
}
