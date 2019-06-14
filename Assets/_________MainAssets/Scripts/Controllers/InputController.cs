using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MovementInput
{
    public float xKeyboard;
    public float yKeyboard;
    public float xMouse;
    public float yMouse;
}

public class InputController : MonoBehaviour {

    private IInputController listener;
    private MovementInput receivedMovementInput;

    private bool submitIsDown = false;
    private bool cancelIsDown = false;

    public void RegisterInputs(IInputController listener)
    {
        this.listener = listener;
    }
    public void UnregisterInputs()
    {
        listener = null;
    }

    public void UpdateInputs()
    {
        if (listener != null)
        {
            UpdateCancel();
            UpdateShoot();
            UpdateMovement();
            UpdatSubmit();
            UpdateMouse();
        }
    }


    private void UpdateMovement()
    {
        receivedMovementInput.xKeyboard = Input.GetAxisRaw("Horizontal");
        receivedMovementInput.yKeyboard = Input.GetAxisRaw("Vertical");
        receivedMovementInput.xMouse = Input.GetAxisRaw("Mouse X");
        receivedMovementInput.yMouse = Input.GetAxisRaw("Mouse Y");
        listener.UpdateMovementInput(receivedMovementInput);
    }

    private void UpdateCancel()
    {
        if (Input.GetAxisRaw("Cancel") > 0 && !cancelIsDown)
        {
            cancelIsDown = true;
            listener.OnCancelPressed();

        }
        else if (Input.GetAxisRaw("Cancel") <= 0)
        {
            cancelIsDown = false;
        }
    }

        private void UpdatSubmit()
    {
        if (Input.GetAxisRaw("Submit") > 0 && !submitIsDown)
        {
            submitIsDown = true;
            listener.OnSubmitPressed();

        }else if (Input.GetAxisRaw("Submit") <= 0)
        {
            submitIsDown = false;
        }
    }

    private void UpdateShoot()
    {
        if (Input.GetAxisRaw("Shoot") > 0)
        {
            listener.OnShootPressed();
        }
    }

    private void UpdateMouse()
    {
        if (Input.GetMouseButton(0))
        {
            listener.OnLeftMouseButtonPressed();
        }
    }

}
