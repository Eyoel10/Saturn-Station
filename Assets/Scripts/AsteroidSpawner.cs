using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] int asteroidCount;
    [SerializeField] float minTimeToNextAsteroid, maxTimeToNextAsteroid;
    [SerializeField] float minScale, maxScale;
    [SerializeField] float minSpeed, maxSpeed;
    [SerializeField] float minAngularVelocity, maxAngularVelocity;
    [SerializeField] float minHeadingAngle, maxHeadingAngle;
    [SerializeField] Asteroid asteroidPrefab;
    readonly List<Asteroid> asteroids = new();

    Vector2 screenSizeWorld;

    void Start()
    {
        screenSizeWorld = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));

        for (int i = 0; i < asteroidCount; ++i)
        {
            Asteroid asteroid = Instantiate(asteroidPrefab);
            asteroid.gameObject.SetActive(false);
            asteroid.isReadyToSpawn = true;
            asteroids.Add(asteroid);
        }
    }

    void Update()
    {
        foreach (Asteroid asteroid in asteroids)
            if (asteroid.isReadyToSpawn)
                StartCoroutine(EnterAsteroid(asteroid));
    }
    
    IEnumerator EnterAsteroid(Asteroid asteroid)
    {
        asteroid.isReadyToSpawn = false;

        yield return new WaitForSeconds(Random.Range(minTimeToNextAsteroid, maxTimeToNextAsteroid));

        asteroid.gameObject.SetActive(true);

        float y = Random.Range(-4.0f * screenSizeWorld.y, 4.0f * screenSizeWorld.y);
        asteroid.transform.position = new Vector2(screenSizeWorld.x + 4.0f, y);
        asteroid.transform.localScale = Random.Range(minScale, maxScale) * new Vector3(1.0f, 1.0f, 1.0f);

        float headingAngle = Random.Range(minHeadingAngle, maxHeadingAngle);
        Vector2 heading = Quaternion.Euler(0.0f, 0.0f, headingAngle) * new Vector3(-1.0f, 0.0f);
        Vector2 velocity = Random.Range(minSpeed, maxSpeed) * heading;

        float rotationDirection = Random.Range(0, 2) == 0 ? -1.0f : 1.0f;
        float angularVelocity = rotationDirection * Random.Range(minAngularVelocity, maxAngularVelocity);

        asteroid.SetVelocity(velocity, angularVelocity);
    }
}
