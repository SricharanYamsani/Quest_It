using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(BattlePlayer))]
public class BattlePlayerInspectorWindow : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.BeginVertical();

        if (GUILayout.Button("Set up player transforms"))
        {
            Process();
        }

        EditorGUILayout.EndVertical();
    }

    private void Process()
    {
        GameObject game = new GameObject();

        BattlePlayer player = (BattlePlayer)target;

        GameObject rightInside = Instantiate<GameObject>(game, player.rightHand);

        rightInside.name = "Right Hand Inside";

        rightInside.transform.localPosition = new Vector3(0.125f, 0.04f, 0);

        GameObject rightOutside = Instantiate<GameObject>(game, player.rightHand);

        rightOutside.name = "Right Hand Outside";

        rightOutside.transform.localPosition = new Vector3(0.08f, -0.04f, 0);

        GameObject leftInside = Instantiate<GameObject>(game, player.leftHand);

        leftInside.name = "Left Hand Inside";

        leftInside.transform.localPosition = new Vector3(-0.125f, -0.04f, 0);

        GameObject leftOutside = Instantiate<GameObject>(game, player.leftHand);

        leftOutside.name = "Left Hand Outside";

        leftOutside.transform.localPosition = new Vector3(-0.08f, 0.04f, 0);

        GameObject face = Instantiate<GameObject>(game, player.faceSpawn_face);

        face.name = "Face Spawn for mask";

        face.transform.localPosition = new Vector3(0, 0.005f, 0.015f);

        GameObject head = Instantiate<GameObject>(game, player.faceSpawn_head);

        head.name = "Head Spawn";

        head.transform.localPosition = new Vector3(0, 0.0225f, 0);

        GameObject torso = Instantiate<GameObject>(game, player.TorsoParent);

        torso.name = "Torso";

        torso.transform.localPosition = new Vector3(0, -0.015f, 0);

        player.rightHandSpawnInside = rightInside.transform;

        player.rightHandSpawnOutside = rightOutside.transform;

        player.leftHandSpawnInside = leftInside.transform;

        player.leftHandSpawnOutside = leftOutside.transform;

        player.faceSpawn = face.transform;

        player.headSpawn = head.transform;

        player.torsoTransform = torso.transform;
    }
}
