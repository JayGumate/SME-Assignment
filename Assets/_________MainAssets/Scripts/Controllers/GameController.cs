using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    private BaseState currentState;

    #region ------Controllers---------
    [Header("Controllers")]

    [SerializeField] private InputController inputController;
    public InputController InputController { get { return inputController; } }

    [SerializeField] private UiController uiController;
    public UiController UiController { get { return uiController; } }

    [SerializeField] private InputFileController inputFileController;
    public InputFileController InputFileController { get { return inputFileController; } }

    #endregion

    #region ------States---------

    [Header("States")]
    [SerializeField] private GameState gameState;
    public GameState GameState { get { return gameState; } }


    [SerializeField] private PrepareInvasionState prepareInvasionState;
    public PrepareInvasionState PrepareInvasionState { get { return prepareInvasionState; } }


    [SerializeField] private IntroState introState;
    public IntroState IntroState { get { return introState; } }

    [SerializeField] private EndState endState;
    public EndState EndState { get { return endState; } }

    #endregion


    private void Awake()
    {
        Initialization();
    }

    private void Initialization()
    {
        //PlayerInputListener = playerInput;
        //localData.Initialization(this);
        //menuController.Initialization(this);
        //coregameplayController.Initialization(this);

        // DataStorage.LoadData(); //preliminary data about profiles
        inputFileController.LoadDifficulty();
        ChangeState(introState);
    }


    public void ChangeState(BaseState newState)
    {
        if (currentState != null) currentState.DeinitState();
        currentState = newState;
        if (currentState != null) currentState.InitState(this);
    }

    private void Update()
    {
        if (currentState != null) currentState.UpdateState();
    }

}
