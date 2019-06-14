using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IntroState : BaseState, IInputController
{
    [SerializeField] private GameObject introMenu;
    [SerializeField] private GameObject gameView;
    [SerializeField] private TMP_Text diff_TMP;

    public override void InitState(GameController gameController)
    {
        base.InitState(gameController);
        introMenu.SetActive(true);
        gameController.InputController.RegisterInputs(this);
        gameView.SetActive(false);
        diff_TMP.text = gameController.InputFileController.CurrentDifficulty;
    }

    public override void UpdateState()
    {
        gameController.InputController.UpdateInputs();

    }

    public override void DeinitState()
    {
        introMenu.SetActive(false);
        gameController.InputController.UnregisterInputs();
        gameView.SetActive(true);


    }

    public void StartGame()
    {
        gameController.ChangeState(gameController.PrepareInvasionState);
    }
    public void ChangeDifficulty()
    {
        int index = gameController.InputFileController.GetAvalaibleDifficulties().IndexOf(gameController.InputFileController.CurrentDifficulty);
        index++;
        if (index.Equals(gameController.InputFileController.GetAvalaibleDifficulties().Count))
        {
            index = 0;
        }
        gameController.InputFileController.ChangeDifficulty(gameController.InputFileController.GetAvalaibleDifficulties()[index]);
        diff_TMP.text = gameController.InputFileController.CurrentDifficulty;

    }
   public void Exit()
    {
        Application.Quit();
    }


    #region ----------------InputControllerImplementation-------------
    public void UpdateMovementInput(MovementInput receivedMovementInput)
    {
    }

    public void OnLeftMouseButtonPressed()
    {
    }

    public void OnCancelPressed()
    {
    }

    public void OnSubmitPressed()
    {
    }

    public void OnShootPressed()
    {
    }
    #endregion

}

