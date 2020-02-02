#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SAR.LEVELGENERATOR
{

    public class LevelGeneratorEditor : EditorWindow
    {
        [MenuItem("Tools/ Level Generator")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow<LevelGeneratorEditor>("Level Generator Window");
        }


        private void OnGUI()
        {
            GUILayout.BeginVertical();

            GenerateLevel();

            GUILayout.EndVertical();
        }


        private List<GameObject> resource = new List<GameObject>();

        private GameObject grassPrefab;

        public Transform grassParent;

        private string x, y;
        /// <summary>Generates a grass bound area currently from co-ordinates (0,0) </summary>
        private void GenerateLevel()
        {

            GUILayout.BeginHorizontal();

            GUILayout.Label("Generate a Grass Map");

            x = EditorGUILayout.TextField("Number of Rows", x);

            y = EditorGUILayout.TextField("Number of Columns", y);


            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("GENERATE"))
            {
                DumpGrass();

                int mX = int.Parse(x);

                int mY = int.Parse(y);

                for (int i = 0; i < mX; i++)
                {
                    for (int j = 0; j < mY; j++)
                    {
                        Vector3 pos = new Vector3(i * 5, 0, j * 5);

                        GameObject myObject = PrefabUtility.InstantiatePrefab(SelectAPrefab(PrefabType.GROUND), LevelGenerator.Instance.grassTransform) as GameObject;

                        myObject.transform.position = pos;

                        LevelGenerator.Instance.grassObjects.Add(myObject);
                    }
                }
            }

            if (GUILayout.Button("Delete Ground"))
            {
                DumpGrass();
            }

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            GUILayout.Label("Generate Level From Texture");

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("LoadMap"))
            {
                LevelGenerator.Instance.LoadLevelFromTexture();
            }

            if(GUILayout.Button("Delete Level Data"))
            {
                DumpLevel();
            }

            GUILayout.EndHorizontal();
        }

        private GameObject SelectAPrefab(PrefabType type)
        {
            if (type == PrefabType.GROUND)
            {
                return LevelGenerator.Instance.GrassPrefab;
            }

            return null;
        }
        private void DumpGrass()
        {
            for (int i = 0; i < LevelGenerator.Instance.grassObjects.Count; i++)
            {
                DestroyImmediate(LevelGenerator.Instance.grassObjects[i].gameObject);
            }

            LevelGenerator.Instance.grassObjects.Clear();
        }

        private void DumpLevel()
        {
            LevelGenerator.Instance.DeleteData();
        }
    }
    public enum PrefabType
    {
        GROUND,
        ETC
    }
}
#endif