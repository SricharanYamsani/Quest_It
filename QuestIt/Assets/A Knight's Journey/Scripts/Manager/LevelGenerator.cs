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

        public Texture2D levelBuildingTexture;

        public Texture2D levelTreeTexture;

        public Texture2D levelRoadTexture;

        public Transform worldTransform;

        public Transform treeTransforms;

        public Transform roadTransform;

        public List<MapReferences> buildingMapReferences = new List<MapReferences>();

        public List<MapReferences> treeMapReferences = new List<MapReferences>();

        public List<MapReferences> roadMapReferences = new List<MapReferences>();

        public List<GameObject> worldObjects = new List<GameObject>();

        public List<GameObject> treeObjects = new List<GameObject>();

        public List<GameObject> roadObjects = new List<GameObject>();

        public void DeleteData()
        {
            DeleteRoadData();

            DeleteTreeData();

            DeleteBuildingData();
        }

        public void DeleteRoadData()
        {
            for (int i = 0; i < roadObjects.Count; i++)
            {
                DestroyImmediate(roadObjects[i].gameObject);
            }

            roadObjects.Clear();
        }

        public void DeleteTreeData()
        {
            for (int i = 0; i < treeObjects.Count; i++)
            {
                DestroyImmediate(treeObjects[i].gameObject);
            }

            treeObjects.Clear();
        }

        public void DeleteBuildingData()
        {
            for (int i = 0; i < worldObjects.Count; i++)
            {
                DestroyImmediate(worldObjects[i].gameObject);
            }

            worldObjects.Clear();
        }


        public void LoadBuildingLevelFromTexture()
        {
            Debug.Log("Building Map : " + levelBuildingTexture.width + "," + levelBuildingTexture.height);

            for (int i = 0; i < levelBuildingTexture.height; i++)
            {
                for (int j = 0; j < levelBuildingTexture.width; j++)
                {
                    Color myColor = levelBuildingTexture.GetPixel(i, j);

                    foreach (MapReferences mR in buildingMapReferences)
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

        public void LoadTreeLevelFromTexture()
        {
            Debug.Log("Tree Map : " + levelTreeTexture.width + "," + levelTreeTexture.height);

            for (int i = 0; i < levelTreeTexture.height; i++)
            {
                for (int j = 0; j < levelTreeTexture.width; j++)
                {
                    Color myColor = levelTreeTexture.GetPixel(i, j);

                    foreach (MapReferences mR in treeMapReferences)
                    {
                        if (mR.referenceColor.Equals(myColor))
                        {
                            GameObject myObject = Instantiate<GameObject>(mR.referenceObject, treeTransforms);

                            myObject.transform.position = new Vector3(i * 5, 0, j * 5);

                            treeObjects.Add(myObject);
                        }
                    }
                }
            }
        }

        public void LoadRoadLevelFromTexture()
        {
            Debug.Log("Road Map : " + levelRoadTexture.width + "," + levelRoadTexture.height);

            for (int i = 0; i < levelRoadTexture.height; i++)
            {
                for (int j = 0; j < levelRoadTexture.width; j++)
                {
                    Color myColor = levelRoadTexture.GetPixel(i, j);

                    foreach (MapReferences mR in roadMapReferences)
                    {
                        if (mR.referenceColor.Equals(myColor))
                        {
                            GameObject myObject = Instantiate<GameObject>(mR.referenceObject, roadTransform);

                            myObject.transform.position = new Vector3(i * 5, 0, j * 5);

                            roadObjects.Add(myObject);
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
