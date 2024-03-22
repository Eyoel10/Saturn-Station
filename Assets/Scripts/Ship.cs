using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public float speed;

    BackgroundScroll bg, nearStars, farStars;
    List<(GameObject, Asteroid)> asteroids;

    void Start()
    {
        bg = GameObject.Find("Background").GetComponent<BackgroundScroll>();
        nearStars = GameObject.Find("NearStars").GetComponent<BackgroundScroll>();
        farStars = GameObject.Find("FarStars").GetComponent<BackgroundScroll>();
        asteroids = GameObject.Find("AsteroidSpawner").GetComponent<AsteroidSpawner>().asteroids;
    }

    void Update()
    {
        float v = Input.GetAxis("Vertical");
        float hRaw = Mathf.Max(Input.GetAxis("Horizontal"), 0.0f);
        float vRaw = Input.GetAxis("Vertical");

        // Rotate ship.
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, v * 45.0f);

        // Scroll backgrounds.
        Vector2 bgTranslation = speed * Time.deltaTime * new Vector2(hRaw, vRaw);
        bg.ScrollBy(bgTranslation);
        nearStars.ScrollBy(bgTranslation);
        farStars.ScrollBy(bgTranslation);

        // Move asteroids.
        foreach ((GameObject asteroid, _) in asteroids)
            asteroid.transform.Translate(-speed * Time.deltaTime * new Vector3(hRaw, vRaw).normalized, Space.World);
    }
}
