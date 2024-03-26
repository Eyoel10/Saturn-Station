using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public bool isAutopilotEnabled;
    public float Score { get; private set; }
    public float Battery { get; private set; }
    public float Shield { get; private set; }

    [SerializeField]
    float speed;

    BackgroundScroll bg, nearStars, farStars;
    MovingObject[] movingObjects;

    void Start()
    {
        isAutopilotEnabled = false;
        Score = 0.0f;
        Battery = 100.0f;
        Shield = 100.0f;

        bg = GameObject.Find("Background").GetComponent<BackgroundScroll>();
        nearStars = GameObject.Find("NearStars").GetComponent<BackgroundScroll>();
        farStars = GameObject.Find("FarStars").GetComponent<BackgroundScroll>();
        movingObjects = FindObjectsByType<MovingObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);
    }

    void Update()
    {
        float v = Input.GetAxis("Vertical");
        float vRaw = Input.GetAxisRaw("Vertical");
        float hRaw = isAutopilotEnabled ? 1.0f : Mathf.Max(Input.GetAxisRaw("Horizontal"), 0.0f);

        Vector2 d = speed * Time.deltaTime * new Vector2(hRaw, vRaw).normalized;

        Score += d.x * 10.0f;
        Battery -= Time.deltaTime * 0.5f;

        // Rotate ship.
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, v * 45.0f);

        // Scroll backgrounds.
        bg.ScrollBy(d);
        nearStars.ScrollBy(d);
        farStars.ScrollBy(d);

        // Move objects in the opposite direction.
        foreach (MovingObject obj in movingObjects)
            obj.transform.Translate(-d, Space.World);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Shield -= 10.0f;
    }
}
