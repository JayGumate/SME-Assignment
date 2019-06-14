using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PrepareInvasionState : BaseState, IInputController,IUIController
{

    [SerializeField] GameObject enemiesParent;
    [SerializeField] GameObject barriersParent;
    [SerializeField] GameObject missilesParents;

    [SerializeField] List<GameObject> enemiesColumns;
    [SerializeField] TMP_Text ready_TMP;


    [Header("Prefabs")]
    [SerializeField] List<GameObject> enemyPrefabs;
    [SerializeField] GameObject missilePrefab;

    private bool isFading = false;


    public override void InitState(GameController gameController)
    {
        base.InitState(gameController);
        RegisterUiEvents();
        gameController.InputController.RegisterInputs(this);

        int prefabNumber = 1;
        foreach(GameObject prefab in enemyPrefabs)
        {
            EnemyShipCollider shipPrefab = prefab.GetComponent<EnemyShipCollider>();
            shipPrefab.dmg = gameController.InputFileController.GetStats(Keys.InputFiles.StatsKeys.Damage, prefabNumber);
            shipPrefab.fireRate = gameController.InputFileController.GetStats(Keys.InputFiles.StatsKeys.FireRate, prefabNumber);
            shipPrefab.reloadTime = gameController.InputFileController.GetStats(Keys.InputFiles.StatsKeys.ReloadTime, prefabNumber);
            shipPrefab.ammoPerReload = gameController.InputFileController.GetStats(Keys.InputFiles.StatsKeys.AmmoPerReload, prefabNumber);
            prefabNumber += 4;

        }

        InstantiatePrefabs();
        BarriersSetActive(true);


        foreach (GameObject column in enemiesColumns)
        {
            float offset = 0;
            int index = 0;
            int randomPrefabIndex = Random.Range(0, enemyPrefabs.Count);
            foreach (Transform enemy in column.transform)
            {
                if (index == randomPrefabIndex )
                {
                    enemy.Translate(new Vector3(0, offset, 0));
                    enemy.gameObject.SetActive(true);
                    offset+=0.7f;
                }
                index++;

                if (index % enemyPrefabs.Count == 0)
                {
                    index = 0;
                    randomPrefabIndex=Random.Range(0, enemyPrefabs.Count);
                }          
            }
        }

        gameController.UiController.TriggerEvent(Keys.Events.READY, 1);


    }

    public override void UpdateState()
    {
        gameController.InputController.UpdateInputs();

    }

    public override void DeinitState()
    {
        gameController.UiController.TriggerEvent(Keys.Events.READY, 0);
        gameController.InputController.UnregisterInputs();
        UnRegisterUiEvents();
    }


    private void InstantiatePrefabs()
    {
        if (missilesParents.transform.childCount <= 0)
        {
            for (int i = 0; i < Keys.Pools.MISSILE_POOL; i++)
            {
                GameObject missile = (GameObject)Instantiate(missilePrefab, missilesParents.transform);
                missile.SetActive(false);
                missile.transform.SetSiblingIndex(1);

                if (i < enemiesColumns.Count && enemiesColumns[i].transform.childCount <= 0)
                {
                    for (int j = 0; j < Keys.Pools.ENEMIES_PER_COLUMN; j++)
                    {
                        foreach (GameObject enemyPrefab in enemyPrefabs)
                        {
                            GameObject enemy = (GameObject)Instantiate(enemyPrefab, enemiesColumns[i].transform);
                            enemy.SetActive(false);
                            enemy.name = enemy.name + j;
                        }
                    }
                }
            }

        }
    }
    public void BarriersSetActive(bool isActive)
    {
        foreach (Transform child in barriersParent.transform)
        {
            child.gameObject.SetActive(isActive);
        }
    }

    public void ReadyFade(float ammount)
    {
        StartCoroutine(DoFadeReady(ammount));
    }

    private IEnumerator DoFadeReady(float ammount)
    {
        if (ammount > 0)
        {

            while (ready_TMP.alpha < 1)
            {
                ready_TMP.alpha += Time.deltaTime * 10;
                if (isFading) break;
                yield return null;
            }
        }
        else
        {
            isFading = true;

            while (ready_TMP.alpha > 0)
            {
                isFading = true;
                ready_TMP.alpha -= Time.deltaTime * 10;
                yield return null;
            }
            isFading = false;
        }
    }


    #region ----------------InputControllerImplementation-------------
    public void UpdateMovementInput(MovementInput receivedMovementInput)
    {
    }

    public void OnLeftMouseButtonPressed()
    {
       gameController.ChangeState(gameController.GameState);
    }

    public void OnCancelPressed()
    {
        gameController.ChangeState(gameController.GameState);

    }

    public void OnSubmitPressed()
    {
        gameController.ChangeState(gameController.GameState);

    }

    public void OnShootPressed()
    {
        gameController.ChangeState(gameController.GameState);

    }
    #endregion

    #region ----------------UIControllerImplemenetation-------------
    public void RegisterUiEvents()
    {
        if (gameController != null)
        {
            gameController.UiController.StartListening(Keys.Events.READY, ReadyFade);

        }
    }

    public void UnRegisterUiEvents()
    {
        if (gameController != null)
        {
            gameController.UiController.StopListening(Keys.Events.READY, ReadyFade);


        }
    }
    #endregion
}

