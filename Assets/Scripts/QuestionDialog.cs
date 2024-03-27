using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionDialog : MonoBehaviour
{
    SaturnStationUI ui;
    int numerator;
    int denominator = 10;

    public void OpenQuestionDialog()
    {
        Time.timeScale = 0.0f;
        numerator = Random.Range(1, denominator + 1);
        ui.OpenQuestionDialog(numerator, denominator);
    }

    public void SubmitAnswer(decimal answer)
    {
        if (answer == (decimal)numerator / denominator)
        {
            StartCoroutine(ui.CorrectAnswer(answer));
            if (denominator < 10000)
                denominator *= 10;
        }
        else
        {
            ui.IncorrectAnswer();
        }
    }

    void Awake()
    {
        ui = GetComponent<SaturnStationUI>();
    }
}
