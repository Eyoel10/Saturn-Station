using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeDetector : MonoBehaviour
{
    public StablizeBalls bubbles;
    float warningInterval = 0.5f;
    public float elapsedTime;
    public int counter;
    Color[] borderColor = new Color[2];
    public SpriteRenderer redBorder;
    
    // Start is called before the first frame update
    void Start()
    {
        counter = 0;
        elapsedTime = 0;
        borderColor[0] = Color.white;
        borderColor[1] = Color.black;
    }

    // Update is called once per frame
    void Update()
    {
        if (getBubbleAmount() >= 22)
        {
            if (elapsedTime >= warningInterval)
            {
                redBorder.color = borderColor[counter % 2];
                counter += 1;
                elapsedTime = 0;
            }
            elapsedTime += Time.deltaTime;
        }
    }

    int getBubbleAmount()
    {
        int counter = 0;
        if (bubbles.balls != null)
        {
            for (int i = 0; i < bubbles.balls.GetLength(0); i++)
            {
                for (int j = 0; j < bubbles.balls.GetLength(1); j++)
                {
                    if (bubbles.balls[i, j] != null)
                    {
                        counter += 1;
                    }
                }
            }
        }
        return counter;
    }
}
