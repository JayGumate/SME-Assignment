using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemiesParentCollider : MonoBehaviour
{

    private GameState gameState;

    private void Awake()
    {
        gameState = transform.parent.GetComponent<GameState>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name.Contains("LeftBorder") || other.name.Contains("RightBorder"))
        {
            gameState.ChangeDirection();
        }
    }

}

