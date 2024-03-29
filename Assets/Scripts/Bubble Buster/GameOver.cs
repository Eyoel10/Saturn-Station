using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public BubbleScore scorer;
    // Start is called before the first frame update
    void Start()
    {
        scorer = GameObject.Find("Scorer").GetComponent<BubbleScore>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // If Balls get to the top game restarts
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Edge"))
        {
            if (scorer.level > 3)
            {
                scorer.DecreaseLevel(3);
            }
            else if (scorer.level > 1)
            {
                scorer.DecreaseLevel(scorer.level - 1);
            }
            else
            {
                scorer.DecreaseLevel(0);
            }
            //SceneManager.LoadScene("Bubble Buster");
        }
    }
}
