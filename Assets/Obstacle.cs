using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void FadeOutAndDisable()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float alpha = 1.0f;
        Color originalColor = spriteRenderer.color;
        while (alpha > 0)
        {
            yield return new WaitForSeconds(.1f);
            alpha -= .1f;
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            
        }
        gameObject.SetActive(false);
    }
}
