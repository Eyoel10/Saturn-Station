using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SaturnStationUI : MonoBehaviour
{
    [SerializeField]
    Ship ship;

    Label score, batteryNumber, shieldNumber;
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
        batteryNumber = root.Q<Label>("battery-number");
        shieldNumber = root.Q<Label>("shield-number");
        batteryBar = root.Q("battery-bar");
        shieldBar = root.Q("shield-bar");
    }

    void Update()
    {
        score.text = $"SCORE: {ship.Score:0} KM";
        batteryBar.style.height = new Length(ship.Battery, LengthUnit.Percent);
        batteryNumber.text = $"{ship.Battery:0}%";
        shieldBar.style.height = new Length(ship.Shield, LengthUnit.Percent);
        shieldNumber.text = $"{ship.Shield:0}%";
    }
}