using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputController {

    void UpdateMovementInput(MovementInput receivedMovementInput);

    void OnLeftMouseButtonPressed();
    void OnCancelPressed();
    void OnSubmitPressed();
    void OnShootPressed();

}
