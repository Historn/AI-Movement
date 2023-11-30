using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CustomEditor(typeof(ThiefBehaviour))]
public class FovEditor : Editor
{
    public void OnSceneGUI()
    {
        ThiefBehaviour patrol = (ThiefBehaviour)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(patrol.cop.transform.position, Vector3.up, Vector3.forward, 360, patrol.copDetectionRadius);
    }
}