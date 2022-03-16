using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    public float velocityBoost = 5.6f;
    public float cooldownSeconds = .5f;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        ready();
    }


    public bool isTriggered()
    {
        return animator.GetBool("triggered");
    }

    public void trigger()
    {
        Debug.Log("W W  W");
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
