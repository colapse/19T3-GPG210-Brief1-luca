using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateBase : MonoBehaviour
{
    public StateBase nextState;
    public virtual void Enter(){ }
    public virtual void Exit() { }
    public virtual void Execute() { }
}
