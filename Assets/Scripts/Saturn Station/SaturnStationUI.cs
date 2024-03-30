using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class ProgressBar
{
    Label number;
    VisualElement bar;
    System.Func<float> getValue;

    public ProgressBar(VisualElement element, System.Func<float> getValue)
    {
        number = element.Q<Label>(className: "progress-number");
        bar = element.Q(className: "progress-bar");
        this.getValue = getValue;
    }

    public void UpdateValue()
    {
        float newValue = getValue();
        bar.style.height = new Length(newValue, LengthUnit.Percent);
        number.text = $"{newValue:0}%";
    }
}

public class SaturnStationUI : MonoBehaviour
{
    public VisualElement Root { get; private set; }

    [SerializeField] Ship ship;

    VisualElement hud;
    Label score;
    ProgressBar battery, shield;

    public void SetHudVisbility(bool isVisible)
    {
        hud.style.visibility = isVisible ? Visibility.Visible : Visibility.Hidden;
    }

    void Awake()
    {
        Root = GetComponent<UIDocument>().rootVisualElement;

        Toggle autopilot = Root.Q<Toggle>("autopilot");
        autopilot.RegisterValueChangedCallback(ev =>
        {
            ship.isAutopilotEnabled = ev.newValue;
        });

        VisualElement controlsUp = Root.Q("controls-up");
        controlsUp.RegisterCallback<MouseDownEvent>(ev =>
        {
            ship.isUpHeld = true;
        });
        controlsUp.RegisterCallback<MouseUpEvent>(ev =>
        {
            ship.isUpHeld = false;
        });

        VisualElement controlsDown = Root.Q("controls-down");
        controlsDown.RegisterCallback<MouseDownEvent>(ev =>
        {
            ship.isDownHeld = true;
        });
        controlsDown.RegisterCallback<MouseUpEvent>(ev =>
        {
            ship.isDownHeld = false;
        });

        VisualElement controlsRight = Root.Q("controls-right");
        controlsRight.RegisterCallback<MouseDownEvent>(ev =>
        {
            ship.isRightHeld = true;
        });
        controlsRight.RegisterCallback<MouseUpEvent>(ev =>
        {
            ship.isRightHeld = false;
        });

        hud = Root.Q("hud");

        score = Root.Q<Label>("score");

        battery = new ProgressBar(Root.Q("battery"), () => ship.Battery);
        shield = new ProgressBar(Root.Q("shield"), () => ship.Shield);
    }

    void Update()
    {
        score.text = $"SCORE: {ship.Score:0} KM";
        battery.UpdateValue();
        shield.UpdateValue();
        if (ship.Battery <= 0)
        {
            SceneManager.LoadScene(0);
        }
        if (ship.Shield <= 0)
        {
            SceneManager.LoadScene(0);
        }
    }
}