using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] GameObject playerlaser;
    [SerializeField] float projectileFiringPeriod = 0.1f;

    Coroutine firingCoroutine;
    float xMin;
    float xMax;
    float yMin;
    float yMax;
    float padding = 1f;
    float projectileSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
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

            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }

}
