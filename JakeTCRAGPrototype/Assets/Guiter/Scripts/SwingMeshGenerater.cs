using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JakeTCRAGPPrototype.Controller.Guiter
{
    public class SwingMeshGenerater : MonoBehaviour
    {
        [Header("SDK Settings & Debug")]
        [SerializeField] Guiter_Controller GuiterCtrl;
        [SerializeField] Transform Point1, Point2;
        [SerializeField] List<SwingLine> SwingLines;
        [SerializeField] float LifeTime = 1f;
        [SerializeField] float FadeOutAnimTime = 1f;
        [SerializeField] MeshFilter MeshF;
        [SerializeField] bool IsGenerating;


        public bool GetIsGenerating { get { return IsGenerating; } }
        public List<SwingLine> GetSwingLines { get { return SwingLines; } }
        private void LateUpdate()
        {
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
        }

        void UpdateSwingLines(bool addNewLines = true, float deadLifeTime = -1)
        {
            if (deadLifeTime > 0)
            {
                for (int i = 0; i < SwingLines.Count; i++)
                {
                    if (SwingLines[i].SpawnTime <= deadLifeTime)
                    {
                        SwingLines.RemoveAt(i);
                        i--;
                    }
                }
            }



            if (addNewLines)
            {
                SwingLines.Add(new SwingLine(Point1.position, Point2.position, Time.time));
            }
        }

        bool UpdateSwingMesh(ref MeshFilter meshFOut, List<SwingLine> swingLines)
        {
            if (swingLines.Count <= 1)
            {
                return false;
            }
            Mesh tmpMesh = new Mesh();

            #region Vertex
            List<Vector3> vertexs = new List<Vector3>();
            for (int i = 0, currentIndex = 0; i < swingLines.Count; i++)
            {
                vertexs.Add(swingLines[i].P1);
                swingLines[i].P1Index = currentIndex;
                currentIndex++;
                vertexs.Add(swingLines[i].P2);
                swingLines[i].P1Index = currentIndex;
                currentIndex++;
            }
            #endregion

            #region Indexs
            List<int> indexs = new List<int>();
            for (int i = 0; i < swingLines.Count; i++)
            {
                if (i != swingLines.Count - 1)
                {
                    indexs.Add(swingLines[i].P1Index);
                    indexs.Add(swingLines[i].P2Index);
                    indexs.Add(swingLines[i + 1].P1Index);
                }

                if (i != 0)
                {
                    indexs.Add(swingLines[i].P2Index);
                    indexs.Add(swingLines[i].P1Index);
                    indexs.Add(swingLines[i - 1].P2Index);
                }

            }
            #endregion
            tmpMesh.vertices = vertexs.ToArray();
            tmpMesh.triangles = indexs.ToArray();
            meshFOut.mesh = tmpMesh;

            return true;
        }

        bool CleanSwingMesh(ref MeshFilter meshFOut)
        {
            Mesh tmpMesh = new Mesh();
            meshFOut.mesh = tmpMesh;
            return true;
        }

        public IEnumerator GeneratingSwingMesh()
        {
            IsGenerating = true;
            YieldInstruction wait = new WaitForEndOfFrame();
            float startFadingTime = Time.time;
            float deadLifeTime = Time.time;
            while (GuiterCtrl.GetIsSwinging)
            {
                UpdateSwingLines(deadLifeTime: GuiterCtrl.GetEffectFadeAwayAfterStopSwinging ? -1.0f : deadLifeTime);
                UpdateSwingMesh(ref MeshF, SwingLines);
                if (!GuiterCtrl.GetEffectFadeAwayAfterStopSwinging)
                {
                    deadLifeTime += Time.deltaTime * GuiterCtrl.GetFadeOutSpeed * GuiterCtrl.GetFadeOutSpeedCurve.Evaluate((Time.time - startFadingTime) / FadeOutAnimTime);
                }
                yield return wait;
            }

            if (!GuiterCtrl.GetEffectFadeAwayAfterStopSwinging)
            {
                startFadingTime = Time.time;
            }

            while (SwingLines.Count > 0)
            {
                UpdateSwingLines(addNewLines: false, deadLifeTime: deadLifeTime);
                UpdateSwingMesh(ref MeshF, SwingLines);
                deadLifeTime += Time.deltaTime * GuiterCtrl.GetFadeOutSpeed * GuiterCtrl.GetFadeOutSpeedCurve.Evaluate((Time.time - startFadingTime) / FadeOutAnimTime);
                yield return wait;
            }
            CleanSwingMesh(ref MeshF);
            IsGenerating = false;
        }

    }

}