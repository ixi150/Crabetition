using UnityEngine;
using UnityEditor;

//[CanEditMultipleObjects]
//[CustomEditor(typeof(Joint))]
public class JointEditor : Editor
{
    Joint joint { get { return target as Joint; } }
    float radius = 1;

    void OnSceneGUI()
    {
        //var min = serializedObject.FindProperty("minRotationClamp").vector3Value;
        //var max = serializedObject.FindProperty("maxRotationClamp").vector3Value;
        //var min = joint.minRotationClamp;
        //var max = joint.maxRotationClamp;
        //var t = joint.transform;
        //DrawArcs(Color.red, min.x, max.x, t.right, t.up);
        //DrawArcs(Color.green, min.y, max.y, t.up, t.forward);
        //DrawArcs(Color.blue, min.z, max.z, t.forward, -t.up);
    }

    void DrawArcs(Color color, float min, float max, Vector3 normal, Vector3 from)
    {
        Handles.color = color;
        DrawArc(min, normal, from);
        DrawArc(max, normal, from);
    }

    void DrawArc(float angle, Vector3 normal, Vector3 from)
    {
        Handles.DrawWireArc(joint.transform.position, normal, from, angle, radius);
    }
}