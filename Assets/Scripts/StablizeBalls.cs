using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class StablizeBalls : MonoBehaviour
{
    
    public float spawnTimer;
    public float ballMoveSpeed;
    public GameObject[] ballsObj;
    public string[] ballsColor;
    // The Spawn point of the balls
    public Transform[] spawnPoints;
    // Holds the position of the points any ball can be on in the scene
    public Vector3[,] placementPoints = new Vector3[8, 5];
    float timer;
    bool goodRandFound;
    public GameObject[,] balls = new GameObject[8, 5];
    // Contains available columns that haven't been filled by a ball
    int[] availableSpots = new int[40];
    Dictionary<string, int[]> valueMap = new Dictionary<string, int[]>();

    int randomBall = 0;
    int randomPlacementPoint = 0;
    int randomSpawnPoint = 0;
    int height = -1;

    // Start is called before the first frame update
    void Start()
    {
        timer = spawnTimer;
        goodRandFound = false;
        int counter = 0;
        for (int i = 0; i < balls.GetLength(0); i++)
        {
            for (int j = 0; j < balls.GetLength(1); j++)
            {
                balls[i, j] = null;
                placementPoints[i, j] = GameObject.Find("Layer " + i).transform.Find("Ball " + j).TransformPoint(Vector3.zero);
                availableSpots[counter] = j;
                counter += 1;
            }
        }
        // Maps the numbers to colors. Forexample a blue bubble will always have the number 6 on it
        valueMap.Add("Blue", new int[] { 6 });
        valueMap.Add("Red", new int[] { 3,8 });
        valueMap.Add("Orange", new int[] { 4 });
        valueMap.Add("Green", new int[] { 9 });
        valueMap.Add("Cyan", new int[] { 1,5 });
        valueMap.Add("Purple", new int[] { 2,7 });
    }

    // Update is called once per frame
    void Update()
    {
        // Moves around the bubbles into their appropriate position
        for (int i = 0; i < balls.GetLength(0); i++)
        {
            for (int j = 0; j < balls.GetLength(1); j++)
            {
                // For all empty spaces
                if (balls[i,j] == null)
                {
                    // If it is not Root
                    if (i+1 < balls.GetLength(0)-1)
                    {
                        // Even Layer
                        if (i % 2 == 0)
                        {
                            // Middle Layer
                            if (j != 0)
                            {
                                // If parent on j-1 exists
                                if (balls[i+1,j-1] != null)
                                {
                                    // Moves the top left ball into the current empty space
                                    balls[i, j] = balls[i + 1, j - 1];
                                    balls[i + 1, j - 1] = null;
                                }
                                // Else If parent on j
                                else if (balls[i+1, j] != null)
                                {
                                    // Moves the top right ball into the current empty space
                                    balls[i, j] = balls[i + 1, j];
                                    balls[i + 1, j] = null;
                                }
                            }
                            // Edge Layer
                            else
                            {
                                // Check parent on j
                                if (balls[i+1, j] != null)
                                {
                                    // Moves the top ball into the current empty space
                                    balls[i, j] = balls[i + 1, j];
                                    balls[i + 1, j] = null;
                                }
                            }
                        }
                        // Odd Layer
                        else
                        {
                            // Middle Layer
                            if (j != balls.GetLength(1)-1)
                            {
                                // If parent on j
                                if (balls[i+1, j] != null)
                                {
                                    // Moves the top left ball into the current empty space
                                    balls[i, j] = balls[i + 1, j];
                                    balls[i + 1, j] = null;
                                }
                                // Else If parent on j+1
                                else if (balls[i+1, j+1] != null)
                                {
                                    // Moves the top right ball into the current empty space
                                    balls[i, j] = balls[i + 1, j+1];
                                    balls[i + 1, j+1] = null;
                                }
                            }
                            // Edge Layer
                            else
                            {
                                // Check parent on j
                                if (balls[i+1, j] != null)
                                {
                                    // Moves the top ball into the current empty space
                                    balls[i, j] = balls[i + 1, j];
                                    balls[i + 1, j] = null;
                                }
                            }
                        }
                    }
                }
            }
        }

        // Update the position of all the balls in the scene
        for (int i = 0; i < balls.GetLength(0); i++)
        {
            for (int j = 0; j < balls.GetLength(1); j++)
            {
                if (balls[i,j] != null && balls[i,j].GetComponent<BallType>() != null)
                {
                    balls[i, j].GetComponent<BallType>().ball.position = placementPoints[i, j];
                }
            }
        }

        // count down timer for spawning bubbles
        timer -= Time.deltaTime;

        // Searches for a good place for a bubble to be in
        if (!goodRandFound)
        {
            randomBall = UnityEngine.Random.Range(0, ballsObj.Length);
            randomPlacementPoint = getRandom();
            randomSpawnPoint = randomPlacementPoint;
            height = -1;

            for (int i = 0; i < balls.GetLength(0); i++)
            {
                if (balls[i, randomPlacementPoint] == null)
                {
                    // Even Level
                    if (i % 2 == 0)
                    {
                        // Middle Case
                        if (randomPlacementPoint != 0)
                        {
                            // Is not leaf
                            if (i-1 >= 0)
                            {
                                // Has two children
                                if (balls[i - 1, randomPlacementPoint] != null && balls[i - 1, randomPlacementPoint - 1] != null)
                                {
                                    height = i;
                                    goodRandFound = true;
                                    break;
                                }
                            }
                            // Is leaf
                            else
                            {
                                height = i;
                                goodRandFound = true;
                                break;
                            }
                        }
                        // Edge case
                        else
                        {
                            if (i-1 >= 0)
                            {
                                if (balls[i - 1, randomPlacementPoint] != null)
                                {
                                    height = i;
                                    goodRandFound = true;
                                    break;
                                }
                            }
                            else
                            {
                                height = i;
                                goodRandFound = true;
                                break;
                            }
                            
                        }
                    }
                    // Odd Level
                    else if (i % 2 == 1)
                    {
                        // Middle Case
                        if (randomPlacementPoint != balls.GetLength(1)-1)
                        {
                            // Has two children
                            if (balls[i - 1, randomPlacementPoint] != null && balls[i - 1, randomPlacementPoint + 1] != null)
                            {
                                height = i;
                                goodRandFound = true;
                                break;
                            }
                        }
                        // Edge case
                        else
                        {
                            if (balls[i - 1, randomPlacementPoint] != null)
                            {
                                height = i;
                                goodRandFound = true;
                                break;
                            }
                        }
                    }

                }
            }
        }
        // If spawn timer runs out, a good empty spot has been found and the spot is on a valid height
        if (timer <= 0 && goodRandFound && height != -1)
        {
            // Makes a new ball, resets time and changes 'goodRandFound' to false so that another good empty spot can be found
            MakeNewBall(randomBall, randomSpawnPoint, height, randomPlacementPoint);
            timer = spawnTimer;
            goodRandFound = false;
        }
    }

    // Makes a new ball from the randomly generated numbers and initializes it with all the appropriate values
    void MakeNewBall(int randomBall, int randomSpawnPoint, int i, int randomPlacementPoint)
    {
        GameObject ball = Instantiate(ballsObj[randomBall]);
        ball.transform.position = spawnPoints[randomSpawnPoint].position;
        ball.SetActive(true);

        Vector3 targetPosition = placementPoints[i, randomPlacementPoint];

        // Sets up all the necessary info about the ball
        ball.GetComponent<BallType>().ball = ScriptableObject.CreateInstance<Ball>();
        ball.GetComponent<BallType>().ball.name = ball.name;
        ball.GetComponent<BallType>().ball.color = ballsColor[randomBall];
        ball.GetComponent<BallType>().ball.value = valueMap[ballsColor[randomBall]][UnityEngine.Random.Range(0, valueMap[ballsColor[randomBall]].Length)];
        ball.GetComponent<BallType>().ball.position = targetPosition;
        ball.GetComponent<BallType>().value.text = ball.GetComponent<BallType>().ball.value.ToString();

        balls[i, randomPlacementPoint] = ball;

        // Removes an available spot from 'availableSpots' since it is filled by the new ball
        int indexToRemove = Array.IndexOf(availableSpots, randomPlacementPoint);
        RemoveAt(ref availableSpots, indexToRemove);
    }

    // Returns the a random number that has a likely chance of being a good empty spot for a new bubble
    int getRandom()
    {
        return availableSpots[UnityEngine.Random.Range(0, availableSpots.Length)];
    }

    // Method to remove an element at a specific index and shift the rest of the elements
    public static void RemoveAt(ref int[] array, int index)
    {
        for (int i = index; i < array.Length - 1; i++)
        {
            array[i] = array[i + 1];
        }

        Array.Resize(ref array, array.Length - 1);
    }

    // Prints array to the console
    static void PrintArray(int[] array)
    {
        string str = "";
        for (int i = 0; i < array.Length; i++)
        { 
            str += array[i] + " ";
        }
        Debug.Log(str);
    }

    // Prints matrix to the console
    void PrintMatrix(GameObject[,] mat)
    {
        string str = "";
        for (int i = 0; i < mat.GetLength(0); i++)
        {
            for (int j = 0; j < mat.GetLength(1); j++)
            {
                str += mat[i, j].name + " ";
            }
            str += "\n";
        }
        Debug.Log(str);
    }

}
