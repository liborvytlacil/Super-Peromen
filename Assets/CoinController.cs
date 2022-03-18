using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    public float framerate = 12;
    public CoinInstance[] coinInstances;

    private float nextFrameTime = 0f;

    [ContextMenu("Find all coins")]
    void findAllCoinsInScene()
    {
        coinInstances = UnityEngine.Object.FindObjectsOfType<CoinInstance>();
    }

    private void Start()
    {
        nextFrameTime = Time.time;
        if (coinInstances.Length == 0)
        {
            findAllCoinsInScene();
        }
    }

    private void Update()
    {
        if (Time.time - nextFrameTime > (1f / framerate))
        {
            for (int i = 0; i < coinInstances.Length; ++i)
            {
                CoinInstance coin = coinInstances[i];
                if (coin != null)
                {
                    if (coin.state == CoinInstance.State.Disappeared)
                    {
                        coin.gameObject.SetActive(false);
                        coinInstances[i] = null;
                    }
                    coin.spriteRenderer.sprite = coin.idleAnimation[coin.frame];
                    coin.frame = (coin.frame + 1) % coin.idleAnimation.Length;
                }
            }

            nextFrameTime = Time.time + (1f / framerate);
        }
    }

}
