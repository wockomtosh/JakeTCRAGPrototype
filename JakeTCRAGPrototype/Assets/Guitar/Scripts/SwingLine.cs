using UnityEngine;

namespace JakeTCRAGPPrototype.Controller.Guitar
{
    [System.Serializable]
    public class SwingLine
    {
        /// <summary>
        /// Begin of the line
        /// </summary>
        public Vector3 P1;
        /// <summary>
        /// End of the line
        /// </summary>
        public Vector3 P2;
        [HideInInspector] public int P1Index;
        [HideInInspector] public int P2Index;
        [HideInInspector] public float SpawnTime;

        public SwingLine(Vector3 p1, Vector3 p2, float spwanTime)
        {
            P1 = p1;
            P2 = p2;
            SpawnTime = spwanTime;
        }
    }

}
