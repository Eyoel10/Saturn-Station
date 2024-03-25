using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SaturnStationUI : MonoBehaviour
{
    [SerializeField]
    Ship ship;

    Label score;

    void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        Toggle autopilot = root.Q<Toggle>("autopilot");
        autopilot.RegisterValueChangedCallback(ev =>
        {
            ship.isAutopilotEnabled = ev.newValue;
        });

        score = root.Q<Label>("score");
    }

    void Update()
    {
        score.text = $"SCORE: {ship.score:0} KM";
    }
}
