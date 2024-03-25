using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float speed, rotationSpeed;
    public Vector3 heading;
    public bool isReadyToSpawn;

    static Bounds bounds = new(new Vector3(), new Vector3(40.0f, 40.0f));

    void Update()
    {
        transform.Translate(speed * Time.deltaTime * heading, Space.World);
        transform.Rotate(0.0f, 0.0f, rotationSpeed * Time.deltaTime);
        if (!bounds.Contains(transform.position))
            ResetAsteroid();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        ResetAsteroid();
    }

    void ResetAsteroid()
    {
        isReadyToSpawn = true;
        gameObject.SetActive(false);
    }
}
