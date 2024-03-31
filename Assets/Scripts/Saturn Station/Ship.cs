using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class Ship : MonoBehaviour
{
    public bool isAutopilotEnabled;
    public float Score { get; private set; }
    public float Battery { get; set; }
    public float Shield { get; private set; }
    public bool isUpHeld, isDownHeld, isRightHeld;
    public LevelLoader levelLoader;

    [SerializeField] float speed;
    [SerializeField] float batteryDrainRate;

    float vertical;
    BackgroundScroll bg, nearStars, farStars;
    MovingObject[] movingObjects;

    SpriteRenderer shieldBubble;
    IEnumerator shieldBubbleCoroutine;

    SaturnStationUI ui;
    QuestionDialog questionDialog;

    float EaseOut(float t)
    {
        float s = 1 - t;
        return 3 * t * s * s + 3 * t * t * s + t * t * t;
    }

    public void LoadShipandControls()
    {

        SelectedShipData shipData = Resources.LoadAll<SelectedShipData>("Saturn Station")[0];
        if (shipData.shipSprite != null)
        {
            GetComponent<SpriteRenderer>().sprite = shipData.shipSprite;
            VisualElement controls = FindFirstObjectByType<SaturnStationUI>().Root.Q("controls");
            controls.style.backgroundImage = new StyleBackground(shipData.controlSprite);
        }
    }
    public void ActivateShieldBubble(bool doTransition = true)
    {
        if (shieldBubbleCoroutine != null)
            StopCoroutine(shieldBubbleCoroutine);
        shieldBubbleCoroutine = CoActivateShieldBubble(doTransition);
        StartCoroutine(shieldBubbleCoroutine);
    }

    public IEnumerator CoActivateShieldBubble(bool doTransition)
    {
        shieldBubble.gameObject.SetActive(true);
        shieldBubble.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        shieldBubble.transform.localScale = new(2.0f, 2.0f, 1.0f);

        if (doTransition)
        {
            Vector3 startScale = new();
            Vector3 endScale = new(2.0f, 2.0f, 1.0f);
            float progress = 0.0f;
            while (progress < 1.0f)
            {
                shieldBubble.transform.localScale = Vector3.Lerp(startScale, endScale, EaseOut(progress));
                progress += Time.deltaTime * 4.0f;
                yield return null;
            }
        }

        yield return new WaitForSeconds(2.0f);

        for (int i = 0; i < 4; ++i)
        {
            yield return new WaitForSeconds(0.15f);
            shieldBubble.color = new Color(1.0f, 1.0f, 1.0f, 0.2f);
            yield return new WaitForSeconds(0.15f);
            shieldBubble.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }

        shieldBubble.gameObject.SetActive(false);
    }

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
        shieldBubble = GameObject.Find("Shield").GetComponent<SpriteRenderer>();
        shieldBubble.gameObject.SetActive(false);

        ui = FindFirstObjectByType<SaturnStationUI>();
        questionDialog = FindFirstObjectByType<QuestionDialogToDecimal>();
        //questionDialog = FindFirstObjectByType<QuestionDialogToFraction>();

        LoadShipandControls();
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
        if (PlayerPrefs.GetInt("HighScore") < Score)
        {
            PlayerPrefs.SetInt("HighScore", (int)Mathf.Floor(Score));
        }
        Battery -= Time.deltaTime * batteryDrainRate;
        if (Battery <= 0.0f)
            ui.GameOver();

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
        if (!shieldBubble.gameObject.activeInHierarchy)
        {
            Shield -= 20.0f;
            ActivateShieldBubble();
            if (Shield <= 0.0f)
                ui.GameOver();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        BatteryPack battery = collision.gameObject.GetComponent<BatteryPack>();
        if (battery != null)
        {
            battery.gameObject.SetActive(false);
            if (collision.gameObject.tag == "Battery")
            {
                questionDialog.Open();
            }
            else if(collision.gameObject.tag == "Shield")
            {
                levelLoader.LoadNextLevel();
            }
        }
    }
}
