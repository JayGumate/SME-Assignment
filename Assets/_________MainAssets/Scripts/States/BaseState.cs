using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState: MonoBehaviour
{
    protected GameController gameController;

    public virtual void InitState(GameController gameController)
    {
        this.gameController = gameController;
    }
    public virtual void UpdateState() { }
    public virtual void DeinitState() { }
}

