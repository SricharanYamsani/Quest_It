using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace SAR.LEVELGENERATOR
{
    public class LevelGenerator : Singleton<LevelGenerator>
    {
        public GameObject GrassPrefab;

        public List<GameObject> grassObjects = new List<GameObject>();

        public Transform grassTransform;

        public Texture2D levelTexture;

        public Transform worldTransform;

        public List<MapReferences> mapReferences = new List<MapReferences>();

        public List<GameObject> worldObjects = new List<GameObject>();

        public void DeleteData()
        {
            for (int i = 0; i < worldObjects.Count; i++)
            {
                DestroyImmediate(worldObjects[i].gameObject);
            }

            worldObjects.Clear();
        }

        public void LoadLevelFromTexture()
        {
            for (int i = 0; i < levelTexture.height; i++)
            {
                for (int j = 0; j < levelTexture.width; j++)
                {
                    Color myColor = levelTexture.GetPixel(i, j);

                    foreach (MapReferences mR in mapReferences)
                    {
                        if (mR.referenceColor.Equals(myColor))
                        {
                            GameObject myObject = Instantiate<GameObject>(mR.referenceObject, worldTransform);

                            myObject.transform.position = new Vector3(i * 5, 0, j * 5);

                            worldObjects.Add(myObject);
                        }
                    }
                }
            }
        }
    }

    [System.Serializable]
    public class MapReferences
    {
        public Color referenceColor;

        public GameObject referenceObject;

        public Transform referenceTransform;
    }
}
