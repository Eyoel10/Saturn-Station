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
        numerator = Random.Range(0, denominator + 1);
        ui.OpenQuestionDialog(numerator, denominator);
    }

    public void SubmitAnswer(decimal answer)
    {
        if (answer == (decimal)numerator / denominator)
        {
            ui.CloseQuestionDialog();
            if (denominator < 10000)
                denominator *= 10;
            Time.timeScale = 1.0f;
        }
        else
        {
            print("incorrect");
        }
    }

    void Awake()
    {
        ui = GetComponent<SaturnStationUI>();
    }
}
