using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

[ExecuteInEditMode]
[RequireComponent(typeof(LookingBehaviour))]
public class CameraController : GameBehaviour
{
    [SerializeField]
    bool executeInEditMode = true;

    [SerializeField]
    bool following = true;

    [SerializeField]
    Transform[] cameraTargets = null;

    [SerializeField, Range(0, 100)]
    float moveSpeed = 1;

    [SerializeField]
    Vector3 offsetPosition = new Vector3(0, 10, -5);

    [SerializeField, Range(0, 10)]
    float targetsFrontLength = 1;

    LookingBehaviour looking;

    void Awake()
    {
        looking = GetComponent<LookingBehaviour>();
    }

    void Update()
    {
        if (!Application.isPlaying && !executeInEditMode) return;
        if (!following) return;
        if (GetTargetsToFollow().Count() == 0) return;

        var center = FindCenterPoint();
        var oldPos = transform.position;
        var newPos = center + offsetPosition;
        transform.position = Vector3.Lerp(oldPos, newPos, moveSpeed * Time.deltaTime);
        looking.LookAt(center, immediately: false);
    }

    Vector3 FindCenterPoint()
    {
        return GetTargetsToFollow()
            .Select(_ => GetFrontPositionOfTarget(_))
            .FindCenterPoint();
    }

    IEnumerable<Transform> GetTargetsToFollow()
    {
        foreach (var target in cameraTargets)
        {
            if (target == null) continue;
            yield return target;
        }
    }

    Vector3 GetFrontPositionOfTarget(Transform target)
    {
        return target.position + target.forward * targetsFrontLength;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        var center = FindCenterPoint();
        Gizmos.DrawCube(center, Vector3.one * .1f);
        foreach (var target in GetTargetsToFollow())
        {
            var frontPosition = GetFrontPositionOfTarget(target);
            Gizmos.DrawLine(center, frontPosition);
            Gizmos.DrawLine(target.position, frontPosition);
        }
    }
}
