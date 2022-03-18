using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    public float velocityBoost = 4.2f;
    public float cooldownSeconds = .5f;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        ready();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!animator.GetBool("trigerred"))
        {
            PlayerController playerController = collision.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.bounce(velocityBoost);
                trigger();
            }
        }
    }

    public void trigger()
    {
        animator.SetBool("triggered", true);
        StartCoroutine(cooldown());
    }

    private IEnumerator cooldown()
    {
        yield return new WaitForSeconds(cooldownSeconds);
        ready();
    }

    private void ready()
    {
        animator.SetBool("triggered", false);
    }

}
