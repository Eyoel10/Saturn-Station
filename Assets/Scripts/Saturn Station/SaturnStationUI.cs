using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class ProgressBar
{
    Label number;
    VisualElement bar;
    System.Func<float> getValue;

    public ProgressBar(VisualElement element, System.Func<float> getValue)
    {
        number = element.Q<Label>(className: "progress-number");
        bar = element.Q(className: "progress-bar");
        this.getValue = getValue;
    }

    public void UpdateValue()
    {
        float newValue = getValue();
        bar.style.height = new Length(newValue, LengthUnit.Percent);
        number.text = $"{newValue:0}%";
    }
}

public class SaturnStationUI : MonoBehaviour
{
    public VisualElement Root { get; private set; }

    [SerializeField] Ship ship;

    VisualElement hud;
    Label score;
    ProgressBar battery, shield;

    VisualElement gameOver;
    Label gameOverMessage;
    Label finalScore;

    VisualElement exitConfirm;

    public void SetHudVisbility(bool isVisible)
    {
        hud.style.visibility = isVisible ? Visibility.Visible : Visibility.Hidden;
    }

    public void GameOver()
    {
        Time.timeScale = 0.0f;
        SetHudVisbility(false);
        gameOver.style.visibility = Visibility.Visible;
        if (ship.Shield <= 0.0f)
            gameOverMessage.text = "Game over\nYour shield was destroyed!";
        else
            gameOverMessage.text = "Game over\nYou ran out of battery!";
        finalScore.text = $"Final score: {ship.Score:0} km";
    }

    void Awake()
    {
        Root = GetComponent<UIDocument>().rootVisualElement;

        Toggle autopilot = Root.Q<Toggle>("autopilot");
        autopilot.RegisterValueChangedCallback(ev =>
        {
            ship.isAutopilotEnabled = ev.newValue;
        });

        VisualElement controlsUp = Root.Q("controls-up");
        controlsUp.RegisterCallback<MouseDownEvent>(ev =>
        {
            ship.isUpHeld = true;
        });
        controlsUp.RegisterCallback<MouseUpEvent>(ev =>
        {
            ship.isUpHeld = false;
        });

        VisualElement controlsDown = Root.Q("controls-down");
        controlsDown.RegisterCallback<MouseDownEvent>(ev =>
        {
            ship.isDownHeld = true;
        });
        controlsDown.RegisterCallback<MouseUpEvent>(ev =>
        {
            ship.isDownHeld = false;
        });

        VisualElement controlsRight = Root.Q("controls-right");
        controlsRight.RegisterCallback<MouseDownEvent>(ev =>
        {
            ship.isRightHeld = true;
        });
        controlsRight.RegisterCallback<MouseUpEvent>(ev =>
        {
            ship.isRightHeld = false;
        });

        hud = Root.Q("hud");

        score = Root.Q<Label>("score");

        battery = new ProgressBar(Root.Q("battery"), () => ship.Battery);
        shield = new ProgressBar(Root.Q("shield"), () => ship.Shield);

        gameOver = Root.Q("game-over");
        gameOverMessage = Root.Q<Label>("game-over-message");
        finalScore = Root.Q<Label>("final-score");

        Button playAgain = Root.Q<Button>("play-again");
        playAgain.clicked += () =>
        {
            Time.timeScale = 1.0f;
            SceneManager.LoadScene("MainMenu");
        };

        exitConfirm = Root.Q("exit-confirm");

        Button exit = Root.Q<Button>("exit");
        exit.clicked += () =>
        {
            Time.timeScale = 0.0f;
            SetHudVisbility(false);
            exitConfirm.style.visibility = Visibility.Visible;
        };

        Button exitConfirmYes = Root.Q<Button>("exit-confirm-yes");
        exitConfirmYes.clicked += () =>
        {
            Time.timeScale = 1.0f;
            SceneManager.LoadScene("MainMenu");
        };

        Button exitConfirmNo = Root.Q<Button>("exit-confirm-no");
        exitConfirmNo.clicked += () =>
        {
            Time.timeScale = 1.0f;
            SetHudVisbility(true);
            exitConfirm.style.visibility = Visibility.Hidden;
        };
    }

    void Update()
    {
        score.text = $"SCORE: {ship.Score:0} KM";
        battery.UpdateValue();
        shield.UpdateValue();
    }
}