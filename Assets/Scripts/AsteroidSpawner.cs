using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject asteroidPrefab;
    public readonly List<(GameObject, Asteroid)> asteroids = new();
    public int asteroidCount;
    public float minTimeToNextAsteroid, maxTimeToNextAsteroid;
    public float minScale, maxScale;
    public float minSpeed, maxSpeed;
    public float minRotationSpeed, maxRotationSpeed;
    public float minHeadingAngle, maxHeadingAngle;

    Vector2 screenSizeWorld;

    void Start()
    {
        screenSizeWorld = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));

        for (int i = 0; i < asteroidCount; ++i)
        {
            GameObject asteroid = Instantiate(asteroidPrefab);
            Asteroid asteroidScript = asteroid.GetComponent<Asteroid>();
            asteroid.SetActive(false);
            asteroidScript.isReadyToSpawn = true;
            asteroids.Add((asteroid, asteroidScript));
        }
    }

    void Update()
    {
        foreach ((GameObject asteroid, Asteroid asteroidScript) in asteroids)
            if (asteroidScript.isReadyToSpawn)
                StartCoroutine(EnterAsteroid(asteroid, asteroidScript));
    }
    
    IEnumerator EnterAsteroid(GameObject asteroid, Asteroid asteroidScript)
    {
        asteroidScript.isReadyToSpawn = false;

        yield return new WaitForSeconds(Random.Range(minTimeToNextAsteroid, maxTimeToNextAsteroid));

        asteroid.SetActive(true);

        float y = Random.Range(-4.0f * screenSizeWorld.y, 4.0f * screenSizeWorld.y);
        asteroid.transform.position = new Vector2(screenSizeWorld.x + 4.0f, y);

        asteroid.transform.localScale = Random.Range(minScale, maxScale) * new Vector3(1.0f, 1.0f, 1.0f);

        asteroidScript.speed = Random.Range(minSpeed, maxSpeed);

        float rotationDirection = Random.Range(0, 2) == 0 ? -1.0f : 1.0f;
        asteroidScript.rotationSpeed = rotationDirection * Random.Range(minRotationSpeed, maxRotationSpeed);

        float headingAngle = Random.Range(minHeadingAngle, maxHeadingAngle);
        asteroidScript.heading = Quaternion.Euler(0.0f, 0.0f, headingAngle) * new Vector3(-1.0f, 0.0f);
    }
}
