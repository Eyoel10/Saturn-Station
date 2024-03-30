using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    [SerializeField] float speed = 1.0f;
    float xOffset = 0.0f, yOffset = 0.0f;
    Renderer render;

    void Start()
    {
        render = GetComponent<Renderer>();
    }

    public void ScrollBy(Vector2 d)
    {
        xOffset = Mathf.Repeat(xOffset + (speed * d.x / render.bounds.size.x), 1.0f);
        yOffset = Mathf.Repeat(yOffset + (speed * d.y / render.bounds.size.y), 1.0f);
        render.material.mainTextureOffset = new Vector2(xOffset, yOffset);
    }
}
