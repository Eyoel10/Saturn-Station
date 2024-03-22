using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubblePopper : MonoBehaviour
{
    int noClicked;
    GameObject[,] balls;
    StablizeBalls spawner;
    float wrongTime = 0.5f;
    float correctTime = 1.5f;
    bool hasCountDownFinished = false;
    bool isCountDownCalled = false;
    public GameObject[] clickedBubbles = new GameObject[2];
    public BubbleScore Scorer;
    // Start is called before the first frame update
    void Start()
    {
        spawner = GameObject.Find("StablizeBalls").GetComponent<StablizeBalls>();
        // Balls in the scene
        balls = spawner.balls;
        for (int i = 0; i < clickedBubbles.Length; i++)
        {
            clickedBubbles[i] = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Checks if two balls have been clicked
        noClicked = 0;
        for (int i = 0; i < clickedBubbles.Length; i++)
        {
            if (clickedBubbles[i] != null)
            {
                noClicked += 1;
            }
        }

        // If 2 bubbles are clicked
        if (noClicked == clickedBubbles.Length)
        {
            // If the sum is 10
            if (checkSum(clickedBubbles, 10))
            {
                print("Sum of " + clickedBubbles[0].GetComponent<BallType>().ball.value + " and " + clickedBubbles[1].GetComponent<BallType>().ball.value + " is 10");
                // Render the Cyan border for both bubbles
                for (int i = 0; i < clickedBubbles.Length; i++)
                {
                    clickedBubbles[i].GetComponent<PopBubble>().myBorder.GetComponent<SpriteRenderer>().sprite = clickedBubbles[i].GetComponent<PopBubble>().borders[0];
                }
                // Wait for a certain amount of time to Pop bubbles
                if (!isCountDownCalled)
                {
                    isCountDownCalled = true;
                    Scorer.StopTimer();
                    StartCoroutine(Countdown(correctTime));
                }
            }
            // If not remove the bubbles from array
            else
            {
                print("A");
                // Render the Red border for both bubbles
                for (int i = 0; i < clickedBubbles.Length; i++)
                {
                    clickedBubbles[i].GetComponent<PopBubble>().myBorder.GetComponent<SpriteRenderer>().sprite = clickedBubbles[i].GetComponent<PopBubble>().borders[2];
                }
                // Wait for a certain amount of time to remove bubbles from clickedBubbles array
                StartCoroutine(Countdown(wrongTime));
            }
        }
    }

    // Sums up all the values of the bubbles and compares it to 'value'
    bool checkSum(GameObject[] bubbles, int value)
    {
        int sum = 0;
        for (int i = 0; i < bubbles.Length; i++)
        {
            sum += bubbles[i].GetComponent<BallType>().ball.value;
        }
        return sum == value;
    }

    // Destroys all objects in the array and updates the balls array
    void PopBubbles()
    {
        for (int i = 0; i < clickedBubbles.Length; i++)
        {
            Destroy(clickedBubbles[i]);
            for (int j = 0; j < balls.GetLength(0); j++)
            {
                for (int k = 0; k < balls.GetLength(1); k++)
                {
                    if (balls[j, k] != null && balls[j, k].GetComponent<BallType>() != null)
                    {
                        if (balls[j, k] == clickedBubbles[i])
                        {
                            balls[j, k] = null;
                        }
                    }
                }
            }
            clickedBubbles[i].GetComponent<PopBubble>().myBorder.GetComponent<SpriteRenderer>().sprite = null;
            clickedBubbles[i] = null;
        }
    }

    // Makes all the elements of the array null
    void ClearArray()
    {
        print("Clear array called");
        for (int i = 0; i < clickedBubbles.Length; i++)
        {
            // Removes border if it is on the bubble
            if (clickedBubbles[i] != null)
            {
                clickedBubbles[i].GetComponent<PopBubble>().myBorder.GetComponent<SpriteRenderer>().sprite = null;
            }
            clickedBubbles[i] = null;
        }
        print("Cleared Array: " + clickedBubbles[0]);
    }

    // Counts down from a duration on a thread outside the main thread and does a function after depending on the given duration time
    IEnumerator Countdown(float duration)
    {
        float timer = 0f;
        hasCountDownFinished = false;
        //print("Count down called");
        while (timer < duration)
        {
            timer += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        if (!hasCountDownFinished)
        {
            if (duration == correctTime)
            {
                print("Pop bubble and clear array with duration: " + duration);
                PopBubbles();
                ClearArray();
                Scorer.UpdateScore();
                Scorer.AddPair();
                Scorer.StartTimer();
                hasCountDownFinished = true;
                isCountDownCalled = false;
            }
            else if (duration == wrongTime)
            {
                print("Wrong time clear array with duration: " + duration);
                ClearArray();
                hasCountDownFinished = true;
                isCountDownCalled = false;
            }
        }
    }
}
