using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float speed, rotationSpeed;
    public Vector3 heading;
    public bool isReadyToSpawn;

    float xMin;

    void Start()
    {
        xMin = -Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0.0f)).x;
    }

    void Update()
    {
        transform.Translate(speed * Time.deltaTime * heading, Space.World);
        transform.Rotate(0.0f, 0.0f, rotationSpeed * Time.deltaTime);
        if (transform.position.x < xMin)
        {
            isReadyToSpawn = true;
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        isReadyToSpawn = true;
        gameObject.SetActive(false);
    }
}
