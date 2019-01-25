using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(MovingBehaviour))]
[RequireComponent(typeof(LookingBehaviour))]
public class Character : GameBehaviour
{
    public MovingBehaviour Moving { get; private set; }
    public LookingBehaviour Looking { get; private set; }
    public PickingBehaviour Picking { get; private set; }

    protected virtual void Awake()
    {
        Moving = GetComponent<MovingBehaviour>();
        Looking = GetComponent<LookingBehaviour>();
        Picking = GetComponent<PickingBehaviour>();
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {

    }

    protected virtual void LateUpdate()
    {

    }

}
