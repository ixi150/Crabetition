using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joint : GameBehaviour
{
    static Vector3 DefaultMinRotationClamp { get { return -DefaultMaxRotationClamp; } }
    static Vector3 DefaultMaxRotationClamp { get { return new Vector3(360, 360, 0); } }

    
    Vector3 minRotationClamp = DefaultMinRotationClamp,
            maxRotationClamp = DefaultMaxRotationClamp;

    //public float LimbLength { get { return Vector3.Magnitude(transform.position - transform.GetChild(0).position); } }
    public float LimbLength { get; private set; }
    public bool IsRoot { get { return Parent == null; } }
    public bool IsFinal { get { return Child == null; } }

    public Joint Child { get; private set; }
    public Joint Parent { get; private set; }
    float maxAngleChange = 10000;


    public Vector3 InboardPosition
    {
        get
        {
            return transform.position;
        }
        set
        {
            transform.position = value;
        }
    }

    public Vector3 OutboardPosition
    {
        get
        {
            return InboardPosition + transform.forward * LimbLength;
        }
        set
        {
            transform.position = value - transform.forward * LimbLength;
        }
    }

    public void LookAt(Vector3 target)
    {
        var look = Quaternion.LookRotation(target - InboardPosition);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, look, maxAngleChange);
        var rotation = transform.localEulerAngles;
        rotation.x = Mathf.Clamp(rotation.x, minRotationClamp.x, maxRotationClamp.x);
        rotation.y = Mathf.Clamp(rotation.y, minRotationClamp.y, maxRotationClamp.y);
        rotation.z = Mathf.Clamp(rotation.z, minRotationClamp.z, maxRotationClamp.z);
        transform.localEulerAngles = rotation;
    }

    private void Start()
    {
        Child = GetComponentOnlyInChildren<Joint>();
        Parent = GetComponentOnlyInParent<Joint>();
        LimbLength = Vector3.Magnitude(transform.position - transform.GetChild(0).position);//3
    }
}
