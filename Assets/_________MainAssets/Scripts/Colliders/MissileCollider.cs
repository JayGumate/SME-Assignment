using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileCollider : MonoBehaviour
{

    private GameState gameState;
    public float dmg;

    private void Awake()
    {
        gameState = transform.parent.parent.GetComponent<GameState>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name.Contains("Border"))
        {
            gameObject.SetActive(false);
        }
        if (other.name.Contains("Missile"))
        {
            other.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
        if (other.name.Contains("Enemy"))
        {
            other.gameObject.SetActive(false);
            gameObject.SetActive(false);
            gameState.EnemyHit();
        }
        if (other.name.Contains("Player"))
        {
            gameObject.SetActive(false);
            gameState.PlayerHit(dmg);
        }
        if (other.name.Contains("Barrier"))
        {
            other.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }

    }

}

