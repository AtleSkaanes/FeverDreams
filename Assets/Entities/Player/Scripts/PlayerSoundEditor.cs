using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerSoundTest))]
public class PlayerSoundEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PlayerSoundTest player = target as PlayerSoundTest;

        if (GUILayout.Button("Make noise"))
        {
            GameManager.Instance.OnNoise(target.GameObject().transform.position);
        }
    }

}
