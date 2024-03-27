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
    Label message;
    TextField answerField;
    Button submit;
    Label dialogBatteryNumber;
    VisualElement dialogBatteryBar;

    public void OpenQuestionDialog(int numeratorValue, int denominatorValue)
    {
        numerator.text = numeratorValue.ToString();
        denominator.text = denominatorValue.ToString();
        message.text = "Type your answer:";
        answerField.value = "";

        answerField.style.unityBackgroundImageTintColor = new(new Color(0.5f, 0.5f, 0.5f));
        UpdateBar(dialogBatteryBar, dialogBatteryNumber, ship.Battery);

        hud.style.visibility = Visibility.Hidden;
        questionDialog.style.visibility = Visibility.Visible;

        answerField.Focus();
    }

    public IEnumerator CorrectAnswer(decimal answer)
    {
        message.text = "Correct!";
        answerField.style.unityBackgroundImageTintColor = new(new Color(0.0f, 1.0f, 0.0f));
        submit.style.visibility = Visibility.Hidden;

        int percentage = (int)(answer * 100);
        for (int i = 0; i < 100; ++i)
        {
            if (i < percentage)
            {
                ship.Battery = Mathf.Clamp(ship.Battery + 1.0f, 0.0f, 100.0f);
                UpdateBar(dialogBatteryBar, dialogBatteryNumber, ship.Battery);
            }
            yield return new WaitForSecondsRealtime(0.03f);
        }

        Time.timeScale = 1.0f;
        hud.style.visibility = Visibility.Visible;
        questionDialog.style.visibility = Visibility.Hidden;
        submit.style.visibility = StyleKeyword.Null;
    }

    public void IncorrectAnswer()
    {
        message.text = "Try again:";
        answerField.style.unityBackgroundImageTintColor = new(new Color(1.0f, 0.0f, 0.0f));
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

        message = root.Q<Label>("message");
        numerator = root.Q<Label>("numerator");
        denominator = root.Q<Label>("denominator");

        answerField = root.Q<TextField>("answer-field");
        answerField.RegisterValueChangedCallback(ev =>
        {
            if (!decimal.TryParse(ev.newValue, out decimal answer))
                answerField.value = "";
        });

        submit = root.Q<Button>("submit");
        submit.clicked += SubmitAnswer;

        dialogBatteryNumber = root.Q<Label>("dialog-battery-number");
        dialogBatteryBar = root.Q("dialog-battery-bar");
    }

    void Update()
    {
        score.text = $"SCORE: {ship.Score:0} KM";
        UpdateBar(batteryBar, batteryNumber, ship.Battery);
        UpdateBar(shieldBar, shieldNumber, ship.Shield);
    }

    void UpdateBar(VisualElement bar, Label number, float value)
    {
        bar.style.height = new Length(value, LengthUnit.Percent);
        number.text = $"{value:0}%";
    }

    void SubmitAnswer()
    {
        if (decimal.TryParse(answerField.value, out decimal answer))
            questionDialogScript.SubmitAnswer(answer);
    }
}