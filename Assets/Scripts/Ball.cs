using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A Scriptable object that contains info about a ball
[CreateAssetMenu(fileName = "ball", menuName = "ball", order = 0)]
public class Ball : ScriptableObject
{
    public Vector3 position;
    public int value;
    public string color;
}
