using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public bool isReadyToSpawn;

    Rigidbody2D rigidBody;

    static Bounds bounds = new(new Vector3(), new Vector3(40.0f, 40.0f));

    public void SetVelocity(Vector2 velocity, float angularVelocity)
    {
        rigidBody.velocity = velocity;
        rigidBody.angularVelocity = angularVelocity;
    }

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!bounds.Contains(transform.position))
            ResetAsteroid();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Ship")
            ResetAsteroid();
    }

    void ResetAsteroid()
    {
        isReadyToSpawn = true;
        gameObject.SetActive(false);
    }
}
