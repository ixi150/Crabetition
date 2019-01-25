using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookingBehaviour : GameBehaviour
{
    [SerializeField]
    float rotationSpeed = 1;

    [SerializeField]
    bool lockX = false,
        lockY = false,
        lockZ = false;

    [SerializeField]
    Transform lookingTarget = null;

    [SerializeField]
    float rotationTreshold = .1f;

    public void LookAt(Transform target, bool immediately = false)
    {
        if (target == null) return;
        LookAt(target.position, immediately);
    }

    public void LookAt(Vector3 worldPosition, bool immediately = false)
    {
        var currentPosition = transform.position;
        var currentEuler = transform.eulerAngles;
        var targetRotation = Quaternion.LookRotation(worldPosition - currentPosition);
        var lerpValue = immediately ? 1 : Time.deltaTime * rotationSpeed;
        var lerpedRotation = Quaternion.Lerp(transform.rotation, targetRotation, lerpValue);
        var lerpedEuler = lerpedRotation.eulerAngles;
        if (lockX) lerpedEuler.x = currentEuler.x;
        if (lockY) lerpedEuler.y = currentEuler.y;
        if (lockZ) lerpedEuler.z = currentEuler.z;
        transform.eulerAngles = lerpedEuler;
        //worldPosition.y = transform.position.y;
        //transform.LookAt(worldPosition);
    }

    public void SetLookingAt(Transform target)
    {
        lookingTarget = target;
        lookingPosition = null;
    }

    public void SetLookingAt(Vector3 targetPosition)
    {
        lookingPosition = targetPosition;
        lookingTarget = null;
    }

    public void LookInDirection(Vector2 screenDirection, bool immediately = false)
    {
        Vector3 worldDirection = new Vector3(screenDirection.x, 0, screenDirection.y);
        LookInDirection(worldDirection, immediately);
    }

    public void LookInDirection(Vector3 worldDirection, bool immediately = false)
    {
        if (worldDirection.magnitude <= rotationTreshold) return;

        var worldPositionToLookAt = worldDirection + transform.position;
        if (immediately)
        {
            LookAt(worldPositionToLookAt, immediately);
        }
        else
        {
            SetLookingAt(worldPositionToLookAt);
        }
    }

    protected virtual void LateUpdate()
    {
        UpdateLooking();
    }

    void UpdateLooking()
    {
        if (lookingTarget != null)
        {
            LookAt(lookingTarget);
        }
        else if (lookingPosition != null)
        {
            LookAt((Vector3)lookingPosition);
        }
    }

    Vector3? lookingPosition;
}
