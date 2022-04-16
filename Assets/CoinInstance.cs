using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinInstance : MonoBehaviour
{
    public Sprite[] idleAnimation;

    // active frame in an animation
    internal int frame = 0;
    internal SpriteRenderer spriteRenderer;
    internal State state;


    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        state = State.Ready;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null && state == State.Ready)
        {
            state = State.Collected;
            StartCoroutine(disappear());
        }
    }

    private IEnumerator disappear()
    {
        Color originalColor = spriteRenderer.color;
        float alpha = 1f;

        // fade out over time
        while (alpha > 0f)
        {
            alpha -= .1f;
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return new WaitForSeconds(.025f);
        }
        state = State.Disappeared;
        Debug.Log("Collected and disappeared");
    }

    internal enum State
    {
        Ready,
        Collected,
        Disappeared
    }
}
