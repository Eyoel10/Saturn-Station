using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Is used to store the balls info on the gameobject and also move each gameobject (ball) into their target position
public class BallType : MonoBehaviour
{
    public Ball ball;
    public TMP_Text value;
    float ballMoveSpeed = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, ball.position, ballMoveSpeed * Time.deltaTime);
    }
}
