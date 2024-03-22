using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class BubbleScore : MonoBehaviour
{
    public float timer = 0;
    public float half_life = 8;
    public float a = 250;
    public bool timeCounting = true;
    public int score = 0;
    public int pair = 0;
    public int level = 1;
    public int minScore = 7;
    public int regScore = 10;
    public float regTime = 10;
    public float elapsedRegTime = 0;
    public TMP_Text scoreUI;
    public TMP_Text pairUI;
    public TMP_Text LevelUI;
    public GameObject background;
    public LevelUp lvlUp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timeCounting)
        {
            timer += Time.deltaTime;
        }

        // Passively gives the player a regScore every regTime seconds
        elapsedRegTime += Time.deltaTime;
        if (elapsedRegTime >=  regTime)
        {
            score += regScore;
            UpdateUI(scoreUI, score);
            elapsedRegTime = 0;
        }
    }

    public void StartTimer()
    {
        if (timer != 0)
        {
            timer = 0;
        }
        timeCounting = true;
    }

    public void StopTimer()
    {
        timeCounting = false;
    }

    public void ResetTimer()
    {
        timer = 0;
    }

    public void AddPair()
    {
        pair += 1;
        pairUI.text = pair.ToString();
        if (pair % 6 == 0)
        {
            level += 1;
            UpdateUI(LevelUI, level);
            background.transform.DOMove(background.transform.position + Vector3.down * 2, 1);
            StartCoroutine(lvlUp.LevelUP(level));
        }
    }

    public void UpdateScore()
    {
        if (timer < 50 && timer >= 0)
        {
            print("Score: " + Mathf.RoundToInt(a*Mathf.Pow(0.5f,(timer/half_life))) + " added");
            score += Mathf.RoundToInt(a * Mathf.Pow(0.5f, (timer / half_life)));
        }
        else
        {
            print("Score: " + minScore + " added");
            score += minScore;
        }
        scoreUI.text = score.ToString();
    }

    public void UpdateUI(TMP_Text textUI, int value)
    {
        textUI.text = value.ToString();
    }
}
