using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SaturnStationUI : MonoBehaviour
{
    [SerializeField] Ship ship;

    QuestionDialog questionDialogScript;

    VisualElement hud, questionDialog;

    Label score, batteryNumber, shieldNumber;
    VisualElement batteryBar, shieldBar;

    Label numerator, denominator;
    TextField answerField;
    Button submit;

    public void OpenQuestionDialog(int numeratorValue, int denominatorValue)
    {
        numerator.text = numeratorValue.ToString();
        denominator.text = denominatorValue.ToString();
        answerField.value = "";

        hud.style.visibility = Visibility.Hidden;
        questionDialog.style.visibility = Visibility.Visible;

        answerField.Focus();
    }

    public void CloseQuestionDialog()
    {
        hud.style.visibility = Visibility.Visible;
        questionDialog.style.visibility = Visibility.Hidden;
    }

    void Awake()
    {
        questionDialogScript = GetComponent<QuestionDialog>();
    }

    void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        Toggle autopilot = root.Q<Toggle>("autopilot");
        autopilot.RegisterValueChangedCallback(ev =>
        {
            ship.isAutopilotEnabled = ev.newValue;
        });

        VisualElement controlsUp = root.Q("controls-up");
        controlsUp.RegisterCallback<MouseDownEvent>(ev =>
        {
            ship.isUpHeld = true;
        });
        controlsUp.RegisterCallback<MouseUpEvent>(ev =>
        {
            ship.isUpHeld = false;
        });

        VisualElement controlsDown = root.Q("controls-down");
        controlsDown.RegisterCallback<MouseDownEvent>(ev =>
        {
            ship.isDownHeld = true;
        });
        controlsDown.RegisterCallback<MouseUpEvent>(ev =>
        {
            ship.isDownHeld = false;
        });

        VisualElement controlsRight = root.Q("controls-right");
        controlsRight.RegisterCallback<MouseDownEvent>(ev =>
        {
            ship.isRightHeld = true;
        });
        controlsRight.RegisterCallback<MouseUpEvent>(ev =>
        {
            ship.isRightHeld = false;
        });

        hud = root.Q("hud");
        questionDialog = root.Q("question-dialog");

        score = root.Q<Label>("score");
        batteryNumber = root.Q<Label>("battery-number");
        shieldNumber = root.Q<Label>("shield-number");
        batteryBar = root.Q("battery-bar");
        shieldBar = root.Q("shield-bar");

        numerator = root.Q<Label>("numerator");
        denominator = root.Q<Label>("denominator");

        answerField = root.Q<TextField>("answer-field");
        answerField.RegisterValueChangedCallback(ev =>
        {
            if (!decimal.TryParse(ev.newValue, out decimal answer))
                answerField.value = "";
        });

        submit = root.Q<Button>("submit");
        submit.clicked += () =>
        {
            if (decimal.TryParse(answerField.value, out decimal answer))
                questionDialogScript.SubmitAnswer(answer);
            
        };
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