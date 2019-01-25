using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovingBehaviour : GameBehaviour
{
    [SerializeField]
    float moveSpeed = 1;

    public void MoveTowards(Transform target)
    {
        if (target == null) return;
        MoveTowards(target.position);
    }

    public void MoveTowards(Vector3 target)
    {
        target.y = 0;
        var pos = transform.position;
        var worldDirection = Vector3.Normalize(pos - target);
        MoveInDirection(worldDirection);
    }

    public void MoveInDirection(Vector2 screenDirection)
    {
        var worldDirection = new Vector3(screenDirection.x, 0, screenDirection.y);
        MoveInDirection(worldDirection);
    }

    public void MoveInDirection(Vector3 worldDirection)
    {
        navAgent.Move(worldDirection * moveSpeed * Time.deltaTime);
    }

    public void SetMoveTarget(Transform target)
    {
        moveTarget = target;
        moveDestination = null;
    }

    public void SetMoveDestination(Vector3 destination)
    {
        moveDestination = destination;
        moveTarget = null;
    }

    public void LookInDirection(Vector3 direction, bool immediately = false)
    {
        direction.y = 0;
        var worldPositionToLookAt = direction + transform.position;
        if (immediately)
        {
            MoveTowards(worldPositionToLookAt);
        }
        else
        {
            SetMoveDestination(worldPositionToLookAt);
        }
    }

    protected virtual void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }

    protected virtual void FixedUpdate()
    {
        UpdateMoving();
    }

    void UpdateMoving()
    {
        if (moveTarget != null)
        {
            MoveTowards(moveTarget);
        }
        else if (moveDestination != null)
        {
            MoveTowards((Vector3)moveDestination);
        }
    }

    Transform moveTarget;
    Vector3? moveDestination;
    NavMeshAgent navAgent;
}
