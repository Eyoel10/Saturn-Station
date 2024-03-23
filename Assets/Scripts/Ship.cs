using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public bool isAutopilotEnabled;

    [SerializeField]
    float speed;

    BackgroundScroll bg, nearStars, farStars;
    MovingObject[] movingObjects;

    void Start()
    {
        isAutopilotEnabled = false;

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

        // Rotate ship.
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, v * 45.0f);

        // Scroll backgrounds.
        Vector2 bgTranslation = speed * Time.deltaTime * new Vector2(hRaw, vRaw);
        bg.ScrollBy(bgTranslation);
        nearStars.ScrollBy(bgTranslation);
        farStars.ScrollBy(bgTranslation);

        // Move objects in the opposite direction.
        foreach (MovingObject obj in movingObjects)
            obj.transform.Translate(-speed * Time.deltaTime * new Vector3(hRaw, vRaw).normalized, Space.World);
    }
}
