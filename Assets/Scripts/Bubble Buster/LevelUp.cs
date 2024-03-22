using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelUp : MonoBehaviour
{
    public GameObject levelUp;
    public TMP_Text levelUpText;
    public StablizeBalls spawner;
    public float levelUpTime = 5;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LevelUP(1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator LevelUP(int level)
    {
        levelUpText.text = "Level " + level;
        levelUp.SetActive(true);
        spawner.isLevelingUp = true;
        yield return new WaitForSecondsRealtime(levelUpTime);
        levelUp.SetActive(false);
        spawner.isLevelingUp = false;
    }
}
