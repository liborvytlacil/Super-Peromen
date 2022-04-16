using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleTrigger : MonoBehaviour
{
    public List<Obstacle> obstacles;
    public Sprite triggeredSprite;

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null && !triggered)
        {
            triggered = true;
            GetComponent<SpriteRenderer>().sprite = triggeredSprite;

            if (obstacles != null)
            {
                obstacles.ForEach(obstacle =>
                {
                    obstacle.gameObject.SetActive(false);
                    Debug.Log("O B S ");
                }); // TODO
            }
        }
    }



}
