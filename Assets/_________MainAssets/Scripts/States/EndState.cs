using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndState : BaseState, IInputController,IUIController
{


    public override void InitState(GameController gameController)
    {
        base.InitState(gameController);
        gameController.InputController.RegisterInputs(this);

    }

    public override void UpdateState()
    {
        if (gameController != null) gameController.InputController.UpdateInputs();

    }

    public override void DeinitState()
    {
        gameController.InputController.UnregisterInputs();
        gameController.UiController.TriggerEvent(Keys.Events.GAME_OVER, 0);
        gameController.UiController.TriggerEvent(Keys.Events.WIN, 0);

    }

    public void UpdateMovementInput(MovementInput receivedMovementInput)
    {
    }

    public void OnLeftMouseButtonPressed()
    {
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
    }

    public void RegisterUiEvents()
    {
        gameController.UiController.StartListening(Keys.Events.GAME_OVER, gameController.UiController.GameOverFade);
        gameController.UiController.StartListening(Keys.Events.WIN, gameController.UiController.WinFade);
    }

    public void UnRegisterUiEvents()
    {
        throw new System.NotImplementedException();
    }
}

