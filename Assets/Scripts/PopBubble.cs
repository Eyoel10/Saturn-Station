using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopBubble : MonoBehaviour
{
    BubblePopper popper;
    GameObject[] clickedBubbles;
    public GameObject myBorder;
    public Sprite[] borders = new Sprite[3];
    // Start is called before the first frame update
    void Start()
    {
        popper = GameObject.Find("BubblePopper").GetComponent<BubblePopper>();
        // Balls in the scene
        clickedBubbles = popper.clickedBubbles;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // When a bubble is clicked its info gets stored in 'clickedBubbles' so that the BubblePopper can decide whether to pop it or not
    private void OnMouseDown()
    {
        for (int i = 0; i < clickedBubbles.Length; i++)
        {
            if (clickedBubbles[i] == null)
            {
                clickedBubbles[i] = gameObject;
                // Apply yellow border sprite to this game object
                myBorder.GetComponent<SpriteRenderer>().sprite = borders[1];
                break;
            }
        }
    }
}
