using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShipCollider : MonoBehaviour
{

    private GameState gameState;

    public float dmg;
    public float fireRate;
    public float reloadTime;
    public float ammoPerReload;

    public static float speedMultiplayer = 1;
    public static bool isGameState = false;

    private float timeToNextShoot;
    private float currentAmmoSize;
    private float currentReloadTime;

    [SerializeField]
    private bool isEnemyFirstActive;

    private void Awake()
    {
        gameState = transform.parent.parent.parent.GetComponent<GameState>();
        timeToNextShoot = fireRate;
        currentReloadTime = 0;
        currentAmmoSize = ammoPerReload;
    }
    private void Update()
    {
        if (isGameState)
        {
            isEnemyFirstActive = false;
            GameObject firstShip = getFirstActiveShip();
            if (firstShip != null)
            {
                if(firstShip.name == gameObject.name) { isEnemyFirstActive = true; }
            }

            if (!gameState.isGameOver && isEnemyFirstActive)
            {
                timeToNextShoot -= Time.deltaTime;
                if (currentReloadTime > 0)
                {
                    currentAmmoSize = ammoPerReload;
                    currentReloadTime -= Time.deltaTime;
                }
                else
                {
                    if (currentAmmoSize <= 0)
                    {
                        currentReloadTime = reloadTime / speedMultiplayer;
                    }
                    if (timeToNextShoot < 0 && currentAmmoSize > 0)
                    {
                        timeToNextShoot = fireRate / speedMultiplayer;
                        currentAmmoSize--;
                        EnemyShoot();
                    }
                }
            }

        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name.Contains("Barrier"))
        {
            gameState.BarriersSetActive(false);
        }
        else if (other.name.Contains("Player") || other.name.Contains("BottomBorder"))
        {
            gameState.GameOver();
        }
    }

    public void EnemyShoot()
    {
        gameState.EnemyShoot(dmg, gameObject);
    }

    private GameObject getFirstActiveShip()
    {
        foreach (Transform ship in gameObject.transform.parent)
        {
            if (ship.gameObject.activeInHierarchy)
                return ship.gameObject;
            
        }
        return null;
    }



}

