using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class QuestionDialogToDecimal : QuestionDialog
{
    Label numerator;
    int numeratorValue;

    protected override void Start()
    {
        base.Start();
        numerator = fraction.Query<Label>().AtIndex(0);
        answerBox = dialog.Q<TextField>(className: "decimal");
        answerField = dialog.Q<TextField>(className: "decimal");
    }

    public override void Open()
    {
        base.Open();
        numeratorValue = Random.Range(1, denominatorValue + 1);
        numerator.text = numeratorValue.ToString();
    }

    protected override bool CheckAnswer(out int percentage)
    {
        percentage = 0;
        if (!decimal.TryParse(answerField.value, out decimal answer))
            return false;
        percentage = (int)(100M * answer);

        return answer == (decimal)numeratorValue / denominatorValue;
    }
}
