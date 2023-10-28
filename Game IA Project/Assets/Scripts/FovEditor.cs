using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CustomEditor(typeof(Patrol))]
public class FovEditor : Editor
{
    public void OnSceneGUI()
    {
        Patrol patrol = (Patrol)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(patrol.transform.position, Vector3.up, Vector3.forward, 360, patrol.detectionRange);
    }
}