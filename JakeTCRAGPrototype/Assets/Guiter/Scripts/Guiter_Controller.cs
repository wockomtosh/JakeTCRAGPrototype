using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JakeTCRAGPPrototype.Controller.Guiter {
    public class Guiter_Controller : MonoBehaviour
    {
        [Header("Developer Setting")]
        [Tooltip("Change this to have a dynamic change of fadding speed, set the curve to a straight line of value 1 if a persist fadding is wanted")]
        [SerializeField] AnimationCurve FadeOutSpeedCurve;
        [Tooltip("The reference of the parent space of Guiter")]
        [SerializeField] Transform HoldingTrans;
        [Tooltip("The range of swinging")]
        [SerializeField] [Range(1f, 360f)] float Range;
        [Tooltip("How fast is the swing")]
        [SerializeField] [Range(1f, 1000f)] float SwingSpeed = 1f;
        [Tooltip("How fast is the fading out of the swing tail")]
        [SerializeField] [Range(1f, 20f)] float FadeOutSpeed = 1f;
        [Tooltip("If the swing tail should start fading out after the swinging is finished")]
        [SerializeField] bool EffectFadeAwayAfterStopSwinging = false;

        [Header("SDK Settings & Debug")]
        [SerializeField] bool IsSwinging = false;
        [SerializeField] Transform GuiterMeshTrans;
        [SerializeField] SwingMeshGenerater SwingMeshGntr;

        [HideInInspector] public bool GetIsSwinging { get { return IsSwinging; } }
        [HideInInspector] public bool GetEffectFadeAwayAfterStopSwinging { get { return EffectFadeAwayAfterStopSwinging; } }
        [HideInInspector] public float GetFadeOutSpeed { get { return FadeOutSpeed; } }
        [HideInInspector] public AnimationCurve GetFadeOutSpeedCurve { get { return FadeOutSpeedCurve; } }


        void LateUpdate()
        {
            transform.position = HoldingTrans.position;
            transform.rotation = HoldingTrans.rotation;
        }

        /// <summary>
        /// Call this fuction to tell Guiter_Controller you want to start swinging.
        /// </summary>
        /// <returns>Wheather if it is triggered successfully.</returns>
        public bool TriggerSwinging()
        {
            if (IsSwinging)
            {
                return false;
            }
            if (SwingMeshGntr.GetIsGenerating)
            {
                return false;
            }
            StartCoroutine(Swinging());
            return true;
        }

        /// <summary>
        /// Set the range of swinging
        /// </summary>
        /// <param name="range">range will always be treated as positive, if an opposite direction of swinging is needed, rotate the y-axis of HoldiingTrans</param>
        /// <returns>If succedded</returns>
        public bool SetSwingingRange(float range)
        {
            Range = Mathf.Abs(range);
            return true;
        }

        /// <summary>
        /// How fast does the swing tail fades out
        /// </summary>
        /// <param name="fadeOutSpeed">Fade out speed</param>
        /// <returns>If succedded</returns>
        public bool SetFadeOutSpeed(float fadeOutSpeed)
        {
            FadeOutSpeed = fadeOutSpeed;
            return true;
        }

        /// <summary>
        /// Get the two edge of the current swing tail shape
        /// </summary>
        /// <param name="beginLine">one end of the tail shape</param>
        /// <param name="endLine">the other end of the tail shape</param>
        /// <returns>if the data is valid</returns>
        public bool GetSwingTrailEdges(out SwingLine beginLine, out SwingLine endLine)
        {
            if(SwingMeshGntr.GetSwingLines.Count <= 1)
            {
                beginLine = null;
                endLine = null;
                return false;
            }

            beginLine = new SwingLine(SwingMeshGntr.GetSwingLines[0].P1, SwingMeshGntr.GetSwingLines[0].P2, SwingMeshGntr.GetSwingLines[0].SpawnTime);
            endLine = new SwingLine(SwingMeshGntr.GetSwingLines[SwingMeshGntr.GetSwingLines.Count - 1].P1, SwingMeshGntr.GetSwingLines[SwingMeshGntr.GetSwingLines.Count - 1].P2, SwingMeshGntr.GetSwingLines[SwingMeshGntr.GetSwingLines.Count - 1].SpawnTime);
            return true;
        }

        IEnumerator Swinging()
        {
            IsSwinging = true;
            StartCoroutine(SwingMeshGntr.GeneratingSwingMesh());
            float currentAngle = 0;
            YieldInstruction wait = new WaitForEndOfFrame();
            GuiterMeshTrans.localRotation = Quaternion.identity;
            while (currentAngle <= Range)
            {
                GuiterMeshTrans.localRotation = Quaternion.Euler(0, 0, currentAngle);
                currentAngle += Time.deltaTime * SwingSpeed;
                yield return wait;
            }
            IsSwinging = false;
            while (SwingMeshGntr.GetIsGenerating)
            {
                yield return wait;
            }
            GuiterMeshTrans.localRotation = Quaternion.identity;
        }
    }
}