using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Tutorial : MonoBehaviour
{
    [SerializeField] Ship ship;
    [SerializeField] GameObject spawner;
    [SerializeField] Asteroid asteroidPrefab;
    [SerializeField] BatteryPack batteryPrefab;

    SaturnStationUI ui;
    Label popup;
    VisualElement controls, battery, shield;
    Toggle autopilot;

    Asteroid asteroid;
    BatteryPack batteryPack;

    Vector2 screenSizeWorld;

    VisualElement flashingElement;
    IEnumerator flashUIElementCoroutine;
    System.Func<bool> isTutorialConditionMet;
    System.Action nextTutorialFunction;

    void Start()
    {
        screenSizeWorld = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
        ui = FindFirstObjectByType<SaturnStationUI>();
        popup = ui.Root.Q<Label>("popup");
        controls = ui.Root.Q("controls");
        battery = ui.Root.Q("battery");
        shield = ui.Root.Q("shield");
        autopilot = ui.Root.Q<Toggle>("autopilot");

        ship.doDrainBattery = false;
        spawner.SetActive(false);

        battery.style.visibility = Visibility.Hidden;
        shield.style.visibility = Visibility.Hidden;
        autopilot.style.visibility = Visibility.Hidden;

        TeachMovement();
    }

    void Update()
    {
        if (isTutorialConditionMet())
        {
            isTutorialConditionMet = () => false;
            popup.style.visibility = Visibility.Hidden;
            StopFlash();
            StartCoroutine(StartNextTutorialAfterSeconds(2.0f));
        }
    }

    IEnumerator StartNextTutorialAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        nextTutorialFunction();
    }

    void TeachMovement()
    {
        isTutorialConditionMet = () => Input.GetAxisRaw("Horizontal") != 0.0f || Input.GetAxisRaw("Vertical") != 0.0f || ship.isDownHeld || ship.isRightHeld || ship.isUpHeld;
        nextTutorialFunction = TeachAsteroids;

        popup.style.visibility = Visibility.Visible;
        popup.text = "Use the arrow keys or\nclick the controls to fly.";

        Flash(controls);
    }

    void TeachAsteroids()
    {
        float x = screenSizeWorld.x + 1.0f;

        isTutorialConditionMet = () => asteroid.transform.position.x < -x - 3.0f;
        nextTutorialFunction = TeachBattery;

        asteroid = Instantiate(asteroidPrefab);
        asteroid.transform.position = new Vector2(x, ship.transform.position.y);
        asteroid.transform.localScale = new(2.0f, 2.0f, 1.0f);
        Rigidbody2D rb = asteroid.GetComponent<Rigidbody2D>();
        rb.velocityX = -1.5f;
        rb.angularVelocity = 15.0f;

        popup.style.visibility = Visibility.Visible;
        popup.text = "Here comes an asteroid!\nMove out of the way or your shields will get damaged.\nIf your shields are destroyed, it's game over!";

        shield.style.visibility = StyleKeyword.Null;
        Flash(shield);
    }

    void TeachBattery()
    {
        isTutorialConditionMet = () => !batteryPack.gameObject.activeInHierarchy || batteryPack.transform.position.x < -screenSizeWorld.x - 4.0f;
        nextTutorialFunction = EndTutorial;

        ship.movingObjects.Remove(asteroid.GetComponent<MovingObject>());
        Destroy(asteroid.gameObject);

        batteryPack = Instantiate(batteryPrefab);
        batteryPack.transform.position = new Vector2(screenSizeWorld.x - 2.0f, ship.transform.position.y);

        popup.style.visibility = Visibility.Visible;
        popup.text = "Battery drains over time.\nIf battery runs out, it's game over!\nPick up the battery pack to recharge.";

        ship.doDrainBattery = true;
        battery.style.visibility = StyleKeyword.Null;
        Flash(battery);
    }

    void EndTutorial()
    {
        isTutorialConditionMet = () => false;

        ship.movingObjects.Remove(batteryPack.GetComponent<MovingObject>());
        Destroy(batteryPack.gameObject);

        autopilot.style.visibility = StyleKeyword.Null;
        spawner.SetActive(true);

        popup.style.visibility = Visibility.Visible;
        popup.text = "Here come some more asteroids!\nGood luck, captain!";

        StartCoroutine(AfterEndTutorial());
    }

    IEnumerator AfterEndTutorial()
    {
        yield return new WaitForSeconds(3.0f);
        popup.style.visibility = Visibility.Hidden;
        StopFlash();
        Destroy(this);
    }

    void Flash(VisualElement element)
    {
        flashingElement = element;
        flashUIElementCoroutine = FlashUIElement(flashingElement);
        StartCoroutine(flashUIElementCoroutine);
    }

    void StopFlash()
    {
        if (flashingElement != null)
        {
            flashingElement.RemoveFromClassList("flash");
            StopCoroutine(flashUIElementCoroutine);
        }
    }

    IEnumerator FlashUIElement(VisualElement element)
    {
        while (true)
        {
            element.AddToClassList("flash");
            yield return new WaitForSeconds(0.5f);
            element.RemoveFromClassList("flash");
            yield return new WaitForSeconds(0.5f);
        }
    }
}
