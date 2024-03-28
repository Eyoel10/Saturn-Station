using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : Spawner
{
    [SerializeField] float minScale, maxScale;
    [SerializeField] float minSpeed, maxSpeed;
    [SerializeField] float minAngularVelocity, maxAngularVelocity;
    [SerializeField] float minHeadingAngle, maxHeadingAngle;

    protected override void AfterSpawn(GameObject asteroid)
    {
        asteroid.transform.localScale = Random.Range(minScale, maxScale) * new Vector3(1.0f, 1.0f, 1.0f);

        float headingAngle = Random.Range(minHeadingAngle, maxHeadingAngle);
        Vector2 heading = Quaternion.Euler(0.0f, 0.0f, headingAngle) * new Vector3(-1.0f, 0.0f);
        Vector2 velocity = Random.Range(minSpeed, maxSpeed) * heading;

        float rotationDirection = Random.Range(0, 2) == 0 ? -1.0f : 1.0f;
        float angularVelocity = rotationDirection * Random.Range(minAngularVelocity, maxAngularVelocity);

        Rigidbody2D rigidBody = asteroid.GetComponent<Rigidbody2D>();
        rigidBody.velocity = velocity;
        rigidBody.angularVelocity = angularVelocity;
    }
}
