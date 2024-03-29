using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class QuestionDialog : MonoBehaviour
{
    [SerializeField] string dialogElementName;
    [SerializeField] Ship ship;
    [SerializeField] SaturnStationUI ui;

    protected VisualElement dialog;
    protected VisualElement fraction;
    Label denominator;
    Label message;
    protected VisualElement answerBox;
    protected TextField answerField;
    Button submit;
    ProgressBar battery;

    int maxDenominatorPow = 1;
    protected int denominatorValue;

    protected virtual void Start()
    {
        dialog = ui.Root.Q(dialogElementName);

        fraction = dialog.Q(className: "fraction");
        denominator = fraction.Query<Label>().Last();

        message = dialog.Q<Label>(className: "message");
        submit = dialog.Q<Button>(className: "submit");
        submit.clicked += SubmitAnswer;

        battery = new ProgressBar(dialog.Q(className: "battery"), () => ship.Battery);
    }

    public virtual void Open()
    {
        Time.timeScale = 0.0f;

        denominatorValue = (int)Mathf.Pow(10.0f, Random.Range(1, maxDenominatorPow + 1));
        denominator.text = denominatorValue.ToString();

        message.text = "Type your answer:";
        answerField.value = "";
        answerBox.style.unityBackgroundImageTintColor = new(new Color(0.5f, 0.5f, 0.5f));

        battery.UpdateValue();

        ui.SetHudVisbility(false);
        dialog.style.visibility = Visibility.Visible;

        answerField.Focus();
    }

    protected abstract bool CheckAnswer(out int percentage);

    public void SubmitAnswer()
    {
        if (CheckAnswer(out int percentage))
        {
            StartCoroutine(CorrectAnswer(percentage));
        }
        else
        {
            message.text = "Try again:";
            answerBox.style.unityBackgroundImageTintColor = new(new Color(1.0f, 0.0f, 0.0f));
        }
    }

    IEnumerator CorrectAnswer(int batteryChargePercent)
    {
        if (maxDenominatorPow < 3)
            maxDenominatorPow += 1;

        message.text = "Correct!";
        answerBox.style.unityBackgroundImageTintColor = new(new Color(0.0f, 1.0f, 0.0f));
        submit.style.visibility = Visibility.Hidden;

        for (int i = 0; i < 100; ++i)
        {
            if (i < batteryChargePercent)
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

        ship.ActivateShieldBubble(false);
    }
}
