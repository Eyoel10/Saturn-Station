using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class QuestionDialog : MonoBehaviour
{
    [SerializeField] Ship ship;
    [SerializeField] SaturnStationUI ui;

    VisualElement dialog;
    Label numerator;
    Label denominator;
    Label message;
    TextField answerField;
    Button submit;
    ProgressBar battery;

    int maxDenominatorPow = 1;
    int numeratorValue, denominatorValue;

    void Start()
    {
        dialog = ui.Root.Q("question-dialog");
        VisualElement fraction = dialog.Q(className: "fraction");
        numerator = fraction.Query<Label>().AtIndex(0);
        denominator = fraction.Query<Label>().AtIndex(1);
        message = dialog.Q<Label>(className: "message");
        answerField = dialog.Q<TextField>(className: "decimal");
        submit = dialog.Q<Button>(className: "submit");
        submit.clicked += SubmitAnswer;
        battery = new ProgressBar(dialog.Q(className: "battery"), () => ship.Battery);
    }

    public void Open()
    {
        Time.timeScale = 0.0f;

        denominatorValue = (int)Mathf.Pow(10.0f, Random.Range(1, maxDenominatorPow + 1));
        numeratorValue = Random.Range(1, denominatorValue + 1);

        numerator.text = numeratorValue.ToString();
        denominator.text = denominatorValue.ToString();
        message.text = "Type your answer:";
        answerField.value = "";

        answerField.style.unityBackgroundImageTintColor = new(new Color(0.5f, 0.5f, 0.5f));
        battery.UpdateValue();

        ui.SetHudVisbility(false);
        dialog.style.visibility = Visibility.Visible;

        answerField.Focus();
    }

    public void SubmitAnswer()
    {
        if (!decimal.TryParse(answerField.value, out decimal answer))
            return;

        if (answer == (decimal)numeratorValue / denominatorValue)
        {
            StartCoroutine(CorrectAnswer(answer));
        }
        else
        {
            message.text = "Try again:";
            answerField.style.unityBackgroundImageTintColor = new(new Color(1.0f, 0.0f, 0.0f));
        }
    }

    IEnumerator CorrectAnswer(decimal answer)
    {
        if (maxDenominatorPow < 3)
            maxDenominatorPow += 1;

        message.text = "Correct!";
        answerField.style.unityBackgroundImageTintColor = new(new Color(0.0f, 1.0f, 0.0f));
        submit.style.visibility = Visibility.Hidden;

        int percentage = (int)(answer * 100);
        for (int i = 0; i < 100; ++i)
        {
            if (i < percentage)
            {
                ship.Battery = Mathf.Clamp(ship.Battery + 1.0f, 0.0f, 100.0f);
                battery.UpdateValue();
            }
            yield return new WaitForSecondsRealtime(0.03f);
        }

        Time.timeScale = 1.0f;
        ui.SetHudVisbility(true);
        dialog.style.visibility = Visibility.Hidden;
        submit.style.visibility = StyleKeyword.Null;
    }
}
