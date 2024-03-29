using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class QuestionDialogToFraction : QuestionDialog
{
    Label decimalLabel;
    decimal decimalValue;

    protected override void Start()
    {
        base.Start();
        answerBox = fraction;
        answerField = fraction.Q<TextField>();
        decimalLabel = dialog.Q<Label>(className: "decimal");
    }

    public override void Open()
    {
        base.Open();
        decimalValue = (decimal)Random.Range(1, denominatorValue + 1) / denominatorValue;
        decimalLabel.text = decimalValue.ToString();
    }

    protected override bool CheckAnswer(out int percentage)
    {
        percentage = 0;
        if (!int.TryParse(answerField.value, out int answer))
            return false;
        percentage = (int)(100M * answer / denominatorValue);

        return decimalValue == (decimal)answer / denominatorValue;
    }
}
