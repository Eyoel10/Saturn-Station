using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    public float speed = 0.4f;

    private float texAspectRatio;
    private float xOffset = 0.0f, yOffset = 0.0f;
    private Renderer render;

    void Start()
    {
        render = GetComponent<Renderer>();
        texAspectRatio = (float)render.material.mainTexture.width / render.material.mainTexture.height;
    }

    void Update()
    {
        float dx = Mathf.Max(Input.GetAxisRaw("Horizontal"), 0);
        float dy = Input.GetAxisRaw("Vertical");
        Vector2 d = speed * Time.deltaTime * new Vector2(dx, dy).normalized;
        xOffset = Mathf.Repeat(xOffset + d.x, 1.0f);
        yOffset = Mathf.Repeat(yOffset + d.y * texAspectRatio, 1.0f);
        render.material.mainTextureOffset = new Vector2(xOffset, yOffset);
    }
}
