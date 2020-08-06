using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] float health = 100;
    [SerializeField] float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] GameObject enemylaser;
    [SerializeField] GameObject enemyexplosion;
    float projectileSpeed = 10f;


    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = "Enemy";
        shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0f)
        {
            Fire();
            shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void Fire()
    {
        GameObject laser = Instantiate(enemylaser, transform.position, Quaternion.identity) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
    }

    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Destroy(other.gameObject);
            Explode();
        }
        else
        {

            DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
            ProcessHit(damageDealer);
        }

    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            Die();
            Explode();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void Explode()
    {
        GameObject explosion = Instantiate(enemyexplosion, transform.position, transform.rotation);
        Destroy(explosion, 1f);
    }
}
