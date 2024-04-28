using UnityEngine;
using System.Collections;

namespace LowPolyWater
{
    public class LowPolyWater : MonoBehaviour
    {
        public float targetY = 0.3f; // Target Y pos
        public float duration = 60f; // Duration for the scaling animation
        public float waveHeight = 0.3f;
        public float waveFrequency = 0.15f;
        public float waveLength = 1f;

        private bool hasSetFirstBehavior, hasSetSecondBehavior, hasSetThirdBehavior, isActive;

        private void SetBehavior(int level)
        {
            if (level == 1) //set on first aftershock
            {
                targetY = 0.3f;
                waveHeight = 0.3f;
                waveFrequency = 0.15f;
                waveLength = 40f;
                //duration = 60f;
                duration = 30f;
            }
            if (level == 2) //set when find bandages for dan objective is active
            {
                targetY = 5.2f;
                waveHeight = 0.8f;
                waveFrequency = 0.17f;
                waveLength = 40f;
                //duration = 600f;
                duration = 60f;
            }
            if (level == 3) //set after completing find bandages for dan objective 
            {
                targetY = 10f;
                waveHeight = 1.5f;
                waveFrequency = 0.2f;
                waveLength = 40f;
                //duration = 1200f;
                duration = 60f;
            }
        }
        //Position where the waves originate from
        public Vector3 waveOriginPosition = new Vector3(0.0f, 0.0f, 0.0f);

        MeshFilter meshFilter;
        Mesh mesh;
        Vector3[] vertices;

        private void Awake()
        {
            //Get the Mesh Filter of the gameobject
            meshFilter = GetComponent<MeshFilter>();
        }

        void Start()
        {
            CreateMeshLowPoly(meshFilter);
            //StartCoroutine(ScaleObject());
            //StartCoroutine(ScaleWaveHeight());
        }
        IEnumerator ScaleObject()
        {
            Vector3 initialPos = transform.position;
            Vector3 targetPos = new Vector3(initialPos.x, targetY, initialPos.z);


            float currentTime = 0f;

            while (currentTime < duration)
            {
                float scaleFactor = Mathf.Lerp(initialPos.y, targetPos.y, currentTime / duration);
                transform.position = new Vector3(initialPos.x, scaleFactor, initialPos.z);
                //setFloodLevel(transform.localScale);
                currentTime += Time.deltaTime;
                yield return null;
            }

            // Ensure final scale is set correctly to avoid precision errors
            transform.position = targetPos;
        }
        IEnumerator ScaleWaveHeight()
        {
            float initialHeight = waveHeight;
            float targetHeight = 2;


            float currentTime = 0f;

            while (currentTime < duration)
            {
                float scaleFactor = Mathf.Lerp(initialHeight, targetHeight, currentTime / duration);
                SetWaveHeight(scaleFactor);
                currentTime += Time.deltaTime;
                yield return null;
            }

            // Ensure final scale is set correctly to avoid precision errors
            waveHeight = targetHeight;
        }
        private void SetWaveHeight(float x)
        {
            waveHeight = x;
        }

        /// <summary>
        /// Rearranges the mesh vertices to create a 'low poly' effect
        /// </summary>
        /// <param name="mf">Mesh filter of gamobject</param>
        /// <returns></returns>
        MeshFilter CreateMeshLowPoly(MeshFilter mf)
        {
            mesh = mf.sharedMesh;

            //Get the original vertices of the gameobject's mesh
            Vector3[] originalVertices = mesh.vertices;

            //Get the list of triangle indices of the gameobject's mesh
            int[] triangles = mesh.triangles;

            //Create a vector array for new vertices 
            Vector3[] vertices = new Vector3[triangles.Length];

            //Assign vertices to create triangles out of the mesh
            for (int i = 0; i < triangles.Length; i++)
            {
                vertices[i] = originalVertices[triangles[i]];
                triangles[i] = i;
            }

            //Update the gameobject's mesh with new vertices
            mesh.vertices = vertices;
            mesh.SetTriangles(triangles, 0);
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            this.vertices = mesh.vertices;

            return mf;
        }

        void Update()
        {
            if (DataHub.WorldEvents.hasFirstAftershockHappened && !hasSetFirstBehavior)
            {
                SetBehavior(1);
                hasSetFirstBehavior = true;
                isActive = true;
                StopAllCoroutines();
                StartCoroutine(ScaleObject());
                StartCoroutine(ScaleWaveHeight());
            }
            if (DataHub.WorldEvents.hasFoundDan && !hasSetSecondBehavior)
            {
                SetBehavior(2);
                hasSetSecondBehavior = true;
                StopAllCoroutines();
                StartCoroutine(ScaleObject());
                StartCoroutine(ScaleWaveHeight());
            }
            if (DataHub.ObjectiveHelper.hasGivenDanBandages && !hasSetThirdBehavior)
            {
                SetBehavior(3);
                hasSetThirdBehavior = true;
                StopAllCoroutines();
                StartCoroutine(ScaleObject());
                StartCoroutine(ScaleWaveHeight());
            }
            if (isActive)
            {
                GenerateWaves();
            }
            

        }

        /// <summary>
        /// Based on the specified wave height and frequency, generate 
        /// wave motion originating from waveOriginPosition
        /// </summary>
        void GenerateWaves()
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3 v = vertices[i];

                //Initially set the wave height to 0
                v.y = 0.0f;

                //Get the distance between wave origin position and the current vertex
                float distance = Vector3.Distance(v, waveOriginPosition);
                distance = (distance % waveLength) / waveLength;

                //Oscilate the wave height via sine to create a wave effect
                v.y = waveHeight * Mathf.Sin(Time.time * Mathf.PI * 2.0f * waveFrequency
                + (Mathf.PI * 2.0f * distance));

                //Update the vertex
                vertices[i] = v;
            }

            //Update the mesh properties
            mesh.vertices = vertices;
            mesh.RecalculateNormals();
            mesh.MarkDynamic();
            meshFilter.mesh = mesh;
        }
    }
}