using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public bool isAutopilotEnabled;
    public float Score { get; private set; }
    public float Battery { get; set; }
    public float Shield { get; private set; }
    public bool isUpHeld, isDownHeld, isRightHeld;

    [SerializeField] float speed;
    [SerializeField] float batteryDrainRate;

    float vertical;
    BackgroundScroll bg, nearStars, farStars;
    MovingObject[] movingObjects;

    QuestionDialog questionDialog;

    void Start()
    {
        isAutopilotEnabled = false;
        Score = 0.0f;
        Battery = 100.0f;
        Shield = 100.0f;

        vertical = 0.0f;

        bg = GameObject.Find("Background").GetComponent<BackgroundScroll>();
        nearStars = GameObject.Find("NearStars").GetComponent<BackgroundScroll>();
        farStars = GameObject.Find("FarStars").GetComponent<BackgroundScroll>();
        movingObjects = FindObjectsByType<MovingObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        questionDialog = FindFirstObjectByType<QuestionDialog>();
    }

    void Update()
    {
        float hRaw = isAutopilotEnabled ? 1.0f : Mathf.Max(Input.GetAxisRaw("Horizontal"), 0.0f);
        float vRaw = Input.GetAxisRaw("Vertical");

        // Check on-screen controls.
        if (isUpHeld)
            vRaw = 1.0f;
        else if (isDownHeld)
            vRaw = -1.0f;
        else if (isRightHeld)
            hRaw = 1.0f;

        // Smooth input.
        if (vRaw > 0.0f)
            vertical = Mathf.Clamp01(vertical + 3.0f * Time.deltaTime);
        else if (vRaw < 0.0f)
            vertical = Mathf.Clamp(vertical - 3.0f * Time.deltaTime, -1.0f, 0.0f);
        else if (vertical > 0.0f)
            vertical = Mathf.Clamp01(vertical - 3.0f * Time.deltaTime);
        else if (vertical < 0.0f)
            vertical = Mathf.Clamp(vertical + 3.0f * Time.deltaTime, -1.0f, 0.0f);

        Vector2 d = speed * Time.deltaTime * new Vector2(hRaw, vRaw).normalized;

        Score += d.x * 10.0f;
        Battery -= Time.deltaTime * batteryDrainRate;

        // Rotate ship.
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, vertical * 45.0f);

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

    void OnTriggerEnter2D(Collider2D collision)
    {
        BatteryPack battery = collision.gameObject.GetComponent<BatteryPack>();
        if (battery != null)
        {
            battery.gameObject.SetActive(false);
            questionDialog.Open();
        }
    }
}
