using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryPack : MonoBehaviour
{
    float dy = 0.0f;
    float tStart;

    void Start()
    {
        tStart = Time.time;
    }

    void Update()
    {
        // Move up and down.
        float dyNew = Mathf.Sin((Time.time - tStart) * 2.0f) * 0.2f;
        transform.position = new Vector3(transform.position.x, transform.position.y - dy + dyNew);
        dy = dyNew;
    }
}
