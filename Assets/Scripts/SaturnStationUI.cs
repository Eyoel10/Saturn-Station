using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SaturnStationUI : MonoBehaviour
{
    [SerializeField]
    Ship ship;

    Label score;
    VisualElement batteryBar, shieldBar;

    void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        Toggle autopilot = root.Q<Toggle>("autopilot");
        autopilot.RegisterValueChangedCallback(ev =>
        {
            ship.isAutopilotEnabled = ev.newValue;
        });

        score = root.Q<Label>("score");
        batteryBar = root.Q("battery-bar");
        shieldBar = root.Q("shield-bar");
    }

    void Update()
    {
        score.text = $"SCORE: {ship.Score:0} KM";
        batteryBar.style.height = new Length(ship.Battery, LengthUnit.Percent);
        shieldBar.style.height = new Length(ship.Shield, LengthUnit.Percent);
    }
}