using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float padding = 1f;
    [Header("Player Stats")]
    [SerializeField] int health = 200;
    [SerializeField] GameObject playerexplosion;
    [Header("Player Sounds")]
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip laserSound;
    [SerializeField] [Range(0, 1)] float SoundVolume = 0.5f;
    [Header("Laser Related")]
    [SerializeField] GameObject playerlaser;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileFiringPeriod = 0.5f;



    Coroutine firingCoroutine;
    float xMin;
    float xMax;
    float yMin;
    float yMax;
    
    

    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = "Player";
        SetUpMovementBoundaries();
        
    }

    private void SetUpMovementBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0,0,0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 1, 0)).x - padding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 1, 0)).y -padding;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            firingCoroutine= StartCoroutine(FireContinously());
        }
        if (Input.GetButtonUp("Fire1"))
        {
            // StopAllCoroutines();
            StopCoroutine(firingCoroutine);
        }
    }

    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal");
        var deltaY = Input.GetAxis("Vertical");

        var newXPos = Mathf.Clamp(transform.position.x + deltaX * Time.deltaTime * moveSpeed, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY * Time.deltaTime * moveSpeed, yMin, yMax);

        transform.position = new Vector2(newXPos, newYPos);

        
    }

    IEnumerator FireContinously()
    {
        while (true)
        {
            GameObject laser = Instantiate(playerlaser, transform.position, Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            PlayLaserSound();
            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            Destroy(other.gameObject);
            Explode();
        }
        else
        {

            DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
            if (!damageDealer) { return; }
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
        GameObject explosion = Instantiate(playerexplosion, transform.position, transform.rotation);
        Destroy(explosion, 1f);
        PlayDeathSound();
        FindObjectOfType<SceneLoader>().LoadLoseScene();
    }
    private void PlayDeathSound()
    {
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, SoundVolume);
    }
    private void PlayLaserSound()
    {
        AudioSource.PlayClipAtPoint(laserSound, Camera.main.transform.position, SoundVolume);
    }

}
