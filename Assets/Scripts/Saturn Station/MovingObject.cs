using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    void Start()
    {
        FindFirstObjectByType<Ship>().movingObjects.Add(this);
    }
}
