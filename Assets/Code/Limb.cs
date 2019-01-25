using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class Limb : GameBehaviour
{
    Crab crab;

    float EPS { get { return crab.EPS; } }
    float skyHeight { get { return crab.skyHeight; } }
    float minDistance { get { return crab.minDistance; } }
    float maxDistance { get { return crab.maxDistance; } }
    float angleTollerance { get { return crab.angleTollerance; } }

    [SerializeField]
    bool isLeg = true;

    [SerializeField]
    SlerpingPoint target = null;

    //float LimbLength { get { return joints.Select(_=>_.LimbLength).Sum(); } }
    //float minDistance { get { return LimbLength * minDistancePercent; } }
    //float maxDistance { get { return LimbLength * maxDistancePercent; } }

    Vector3 GoalPosition
    {
        get { return target.transform.position; }
        //set { target.transform.position = value; }
        set { target.SetWantedPosition(value); }
    }
    Vector3 EndEffectorPosition { get { return FinalLimb.transform.position; } }
    Joint[] joints;
    Joint FinalLimb { get { return joints.LastOrDefault(); } }
    Joint RootLimb { get { return joints.FirstOrDefault(); } }
    Vector3 originalRootPosition;
    Vector3 ClosePoint { get { return CastOnNavmesh(GetDirectedPoint(minDistance)); } }
    Vector3 FarPoint { get { return CastOnNavmesh(GetDirectedPoint(maxDistance)); } }
    Vector3 LimbDirection { get { return GetDeltaVector2D(transform.position, transform.parent.position).normalized; } }
    Vector3 GetDirectedPoint(float distance)
    {
        return transform.position + LimbDirection * distance;
    }

    Vector3 GetDeltaVector2D(Vector3 a, Vector3 b)
    {
        a.y = b.y = 0;
        return a - b;
    }

    Vector3 CastOnNavmesh(Vector3 pos)
    {
        pos.y = transform.position.y;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(pos, out hit, 1000, -1))
        {
            return hit.position;
        }
        return pos;
    }

    private void Start()
    {
        crab = FindInParent<Crab>(transform);
        joints = GetComponentsInChildren<Joint>();
        if (target == null)
        {
            target = new GameObject("Target-" + name).AddComponent<SlerpingPoint>();
            target.transform.position = ClosePoint;
        }
    }

    void Update()
    {
        if (isLeg)
        {
            UpdateGoalPositions();
            HackIKsRotation();
        }
        DoIK();
    }

    void UpdateGoalPositions()
    {
        var pos = transform.position;
        var delta2D = GetDeltaVector2D(GoalPosition, pos);
        var sqrDist2D = Vector3.SqrMagnitude(delta2D);
        if (sqrDist2D > maxDistance * maxDistance)
        {
            GoalPosition = ClosePoint;
        }
        else if (sqrDist2D < minDistance * minDistance)
        {
            GoalPosition = FarPoint;
        }
        var angle = Vector3.Angle(delta2D, LimbDirection);
        if (Mathf.Abs(angle) > angleTollerance)
        {
            GoalPosition = CastOnNavmesh(GetDirectedPoint(Vector3.Magnitude(delta2D)));
        }
    }

    void HackIKsRotation()
    {
        var skyGoalPosition = GoalPosition + Vector3.up * skyHeight;
        foreach (var joint in joints)
        {
            joint.LookAt(skyGoalPosition);
        }
    }

    void DoIK()
    {
        originalRootPosition = RootLimb.InboardPosition;
        int i = 0;
        do
        {
            FinalToRoot(); // PartOne
            RootToFinal(); // PartTwo
            if (i++ > 100) break;
        }
        while (Mathf.Abs(Vector3.SqrMagnitude(EndEffectorPosition - GoalPosition)) > EPS);
    }

    /* Part One */
    void FinalToRoot()
    {
        //currentGoal = goalPosition;
        //currentLimb = finalLimb;
        //while (currentLimb != NULL)
        //{
        //    currentLimb.rotation = RotFromTo(Vector.UP,
        //        currentGoal — currentLimb.inboardPosition);
        //    currentLimb.outboardPosition = currentGoal;
        //    currentGoal = currentLimb.inboardPosition;
        //    currentLimb = currentLimb->inboardLimb;
        //}
        var currentGoal = GoalPosition;
        var currentLimb = FinalLimb;
        while (currentLimb != null)
        {
            currentLimb.LookAt(currentGoal);
            currentLimb.OutboardPosition = currentGoal;
            currentGoal = currentLimb.InboardPosition;
            currentLimb = currentLimb.Parent;
        }
    }

    /* Part Two */
    void RootToFinal()
    {
        //currentInboardPosition = rootLimb.inboardPosition;
        //currentLimb = rootLimb;
        //while (currentLimb != NULL)
        //{
        //    currentLimb.inboardPosition = currentInboardPosition;
        //    currentInboardPosition = currentLimb.outboardPosition;
        //    currentLimb = currentLimb->outboardLimb;
        //}
        var currentLimb = RootLimb;
        var currentInboardPosition = originalRootPosition;
        while (currentLimb != null)
        {
            currentLimb.InboardPosition = currentInboardPosition;
            currentInboardPosition = currentLimb.OutboardPosition;
            currentLimb = currentLimb.Child;
        }
    }

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.green;
    //    DrawSphere(minDistance);
    //    Gizmos.color = Color.red;
    //    DrawSphere(maxDistance);
    //}

    //void DrawSphere(float radius)
    //{
    //    Gizmos.DrawWireSphere(transform.position, radius);
    //}

    public static T FindInParent<T>(Transform parent) where T : MonoBehaviour
    {
        var component = parent.GetComponentInParent<T>();
        if (component != null) return component;
        if (parent.parent == null) return default(T);
        return FindInParent<T>(parent.parent);
    }
}
