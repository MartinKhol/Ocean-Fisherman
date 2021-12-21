using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayColor : MonoBehaviour
{
    [SerializeField]
    private Transform player;

    public Color topColor;
    public Color bottomColor;

    public float highest = 0f;
    public float deepestY = -50f;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float position = Mathf.Clamp(player.position.y, deepestY, highest);

        float distance = Mathf.Abs(highest - deepestY);

        position = Mathf.Abs(position - highest);

        spriteRenderer.color = Color.Lerp(topColor, bottomColor, position / distance);
    }
}
