using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameState : BaseState, IInputController,IUIController
{

    [SerializeField] Rigidbody2D player;

    [SerializeField] GameObject enemiesParent;
    [SerializeField] GameObject barriersParent;
    [SerializeField] GameObject missilesParents;


    [SerializeField] List<GameObject> enemiesColumns;


    [Header("UI")]

    [SerializeField] TMP_Text hp_TMP;
    [SerializeField] TMP_Text score_TMP;
    [SerializeField] TMP_Text levelUp_TMP;
    [SerializeField] TMP_Text pause_TMP;

    private MovementInput movementInput;
    public bool isGameOver = false;
    private bool canShoot = true;
    private float speedMultiplayer = 1;
    private Vector2 startEnemiesPos;


    private float timeLeftToPlayerShoot;
    private float timeLeftToEnemyMove;
    private float timeLeftToSpeedUp;


    private int direction = 1;
    private bool isFading = false;
    private float score;
    private float hp;

    public override void InitState(GameController gameController)
    {
        base.InitState(gameController);
        gameController.InputController.RegisterInputs(this);
        RegisterUiEvents();
        timeLeftToPlayerShoot = Keys.Times.TIME_TO_NEXT_PLAYER_SHOOT;
        timeLeftToEnemyMove = Keys.Times.TIME_TO_NEXT_ENEMY_MOVE;
        timeLeftToSpeedUp = Keys.Times.TIME_TO_NEXT_LEVEL;
        startEnemiesPos = enemiesParent.transform.position;
        isGameOver = false;
        speedMultiplayer = 1;
        hp = 100;
        score = 0;
        gameController.UiController.TriggerEvent(Keys.Events.SCORE, score);
        gameController.UiController.TriggerEvent(Keys.Events.HP, hp);
        gameController.UiController.TriggerEvent(Keys.Events.GAME_OVER, 0);
        gameController.UiController.TriggerEvent(Keys.Events.WIN, 0);
        EnemyShipCollider.isGameState = true;
        EnemyShipCollider.speedMultiplayer = speedMultiplayer;
        BarriersSetActive(true);


    }

    public override void UpdateState()
    {
        if(gameController!=null) gameController.InputController.UpdateInputs();
        timeLeftToPlayerShoot -= Time.deltaTime;
        timeLeftToEnemyMove -= Time.deltaTime;
        timeLeftToSpeedUp -= Time.deltaTime;


        if (timeLeftToPlayerShoot < 0)
        {
            timeLeftToPlayerShoot = Keys.Times.TIME_TO_NEXT_PLAYER_SHOOT;
            canShoot = true;
        }

        if (timeLeftToEnemyMove < 0)
        {
            timeLeftToEnemyMove = Keys.Times.TIME_TO_NEXT_ENEMY_MOVE / speedMultiplayer;
            MoveEnemies();
        }

        if (timeLeftToSpeedUp < 0)
        {
            timeLeftToSpeedUp = Keys.Times.TIME_TO_NEXT_LEVEL;
            speedMultiplayer *= 1.5f;
            EnemyShipCollider.speedMultiplayer = speedMultiplayer;
            score += 100;
            gameController.UiController.TriggerEvent(Keys.Events.SCORE, score);
            gameController.UiController.TriggerEvent(Keys.Events.LEVEL_UP, 0);
        }

        MovePlayer();
        MoveMissiles();

    }

    public override void DeinitState()
    {
        gameController.InputController.UnregisterInputs();
        UnRegisterUiEvents();

        BarriersSetActive(false);

        enemiesParent.transform.position = startEnemiesPos;

        foreach (GameObject column in enemiesColumns)
        {
            foreach (Transform enemy in column.transform)
            {
                    enemy.position = column.transform.position;
                    enemy.gameObject.SetActive(false);               
            }
        }

        foreach(Transform missile in missilesParents.transform)
        {
            missile.gameObject.SetActive(false);
        }
        EnemyShipCollider.isGameState = false;
        player.velocity = Vector2.zero;
    }

    private void MoveMissiles()
    {
        if (!isGameOver)
        {
            foreach (Transform missile in missilesParents.transform)
            {
                if (missile.gameObject.activeInHierarchy)
                {
                    missile.Translate(new Vector3(0, -Time.deltaTime * 4f, 0));
                }
            }
        }
    }


    private void MovePlayer()
    {
        if (!isGameOver)
        {
            if (movementInput.xKeyboard != 0)
            {
                var vel = player.velocity;
                vel.x = 5 * movementInput.xKeyboard;
                player.velocity = vel;
            }
            else if (movementInput.xMouse != 0)
            {
                var vel = player.velocity;
                vel.x = 14 * movementInput.xMouse;
                player.velocity = vel;
            }
            else
            {
                player.velocity = Vector2.zero;
            }
        }
        else
        {
            player.velocity = Vector2.zero;
        }
    }

    private void Shoot()
    {
        if (canShoot && !isGameOver)
        {
            GameObject missile = GetPooledInactiveMissile().gameObject;
            missile.gameObject.SetActive(true);
            missile.transform.position = player.transform.position + new Vector3(0, 0.9f, 0);
            missile.transform.eulerAngles=new Vector3(0, 0, 180);
            canShoot = false;
        }
    }

    public void EnemyShoot(float dmg,GameObject enemy)
    {
        
        GameObject missile = GetPooledInactiveMissile().gameObject;
        missile.gameObject.SetActive(true);
        missile.GetComponent<MissileCollider>().dmg=dmg;
        missile.transform.eulerAngles = new Vector3(0, 0, 0);
        missile.transform.position = enemy.transform.position - new Vector3(0, 0.7f, 0);
    }



    public void EnemyHit()
    {
        score += 60;
       gameController.UiController.TriggerEvent(Keys.Events.SCORE, score);

        foreach(GameObject col in enemiesColumns)
        {
            foreach( Transform enemy in col.transform)
            {
                if (enemy.gameObject.activeInHierarchy) return;
            }
        }
        Win();
    }

    public void PlayerHit(float dmg)
    {
        hp -= dmg;
        if (hp <= 0)
        {
            hp = 0;
            GameOver();

        }
        gameController.UiController.TriggerEvent(Keys.Events.HP, hp);

    }

    public void GameOver()
    {
        isGameOver = true;
        gameController.UiController.TriggerEvent(Keys.Events.GAME_OVER, 1);
        gameController.ChangeState(gameController.EndState);
    }

    public void Win()
    {
        isGameOver = true;
        gameController.UiController.TriggerEvent(Keys.Events.WIN, 1);
        gameController.ChangeState(gameController.EndState);

    }

    public void ChangeDirection()
    {
        direction = -direction;
        enemiesParent.transform.Translate(new Vector3(0,-Keys.Distances.ENEMIES_Y_MOVE , 0));
    }

    private void MoveEnemies()
    {
        if (!isGameOver)
        {
            enemiesParent.transform.Translate(new Vector3(Keys.Distances.ENEMIES_X_MOVE * direction, 0, 0));
        }

    }

    public void BarriersSetActive(bool isActive)
    {
        foreach (Transform child in barriersParent.transform)
        {
            child.gameObject.SetActive(isActive);
        }
    }



    private Transform GetPooledInactiveMissile()
    {
        foreach (Transform missile in missilesParents.transform)
        {
            if (!missile.gameObject.activeInHierarchy)
            {
                return missile;
            }
        }
        return null;
    }

    #region ----------------UIImplemenetation-------------
    public void ChangeScore(float ammount)
    {
        score_TMP.text = Keys.Strings.SCORE + ammount;
    }

    public void ChangeHP(float ammount)
    {
        hp_TMP.text = Keys.Strings.HP + ammount;
    }

    public void LevelUpFade(float ammount)
    {
        StartCoroutine(DoFadeNewLevel());
    }



    private IEnumerator DoFadeNewLevel()
    {
        while (levelUp_TMP.alpha < 1)
        {
            levelUp_TMP.alpha += Time.deltaTime * 2;
            yield return null;
        }
        while (levelUp_TMP.alpha > 0)
        {
            levelUp_TMP.alpha -= Time.deltaTime * 2;
            yield return null;
        }

    }



    public void PauseFade(float ammount)
    {
        StartCoroutine(DoPauseFade(ammount));
    }

    private IEnumerator DoPauseFade(float ammount)
    {
        if (ammount > 0)
        {

            while (pause_TMP.alpha < 1)
            {
                pause_TMP.alpha += Time.deltaTime * 10;
                if (isFading) break;
                yield return null;
            }
        }
        else
        {
            isFading = true;

            while (pause_TMP.alpha > 0)
            {
                isFading = true;
                pause_TMP.alpha -= Time.deltaTime * 10;
                yield return null;
            }
            isFading = false;
        }
    }



    #endregion

    #region ----------------InputControllerImplementation-------------
    public void UpdateMovementInput(MovementInput receivedMovementInput)
    {
        movementInput = receivedMovementInput;
    }

    public void OnLeftMouseButtonPressed()
    {
        Shoot();
    }

    public void OnCancelPressed()
    {
        gameController.ChangeState(gameController.IntroState);

    }

    public void OnSubmitPressed()
    {
        gameController.ChangeState(gameController.PrepareInvasionState);
    }

    public void OnShootPressed()
    {
        Shoot();
    }

    #endregion

    #region ----------------UIControllerImplemenetation-------------
    public void RegisterUiEvents()
    {
        if (gameController != null)
        {
            gameController.UiController.StartListening(Keys.Events.GAME_OVER, gameController.UiController.GameOverFade);
            gameController.UiController.StartListening(Keys.Events.WIN, gameController.UiController.WinFade);
            gameController.UiController.StartListening(Keys.Events.LEVEL_UP, LevelUpFade);
            gameController.UiController.StartListening(Keys.Events.SCORE, ChangeScore);
            gameController.UiController.StartListening(Keys.Events.HP, ChangeHP);
            gameController.UiController.StartListening(Keys.Events.PAUSE, PauseFade);

        }
    }

    public void UnRegisterUiEvents()
    {
        if (gameController != null)
        {
            gameController.UiController.StopListening(Keys.Events.GAME_OVER, gameController.UiController.GameOverFade);
            gameController.UiController.StopListening(Keys.Events.WIN, gameController.UiController.WinFade);
            gameController.UiController.StopListening(Keys.Events.LEVEL_UP, LevelUpFade);
            gameController.UiController.StopListening(Keys.Events.SCORE, ChangeScore);
            gameController.UiController.StopListening(Keys.Events.HP, ChangeHP);
            gameController.UiController.StopListening(Keys.Events.PAUSE, PauseFade);

        }
    }
    #endregion


}

