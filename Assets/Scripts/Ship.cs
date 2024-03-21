using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        float v = Input.GetAxis("Vertical");
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, v * 45.0f);
    }
}
