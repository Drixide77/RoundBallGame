using UnityEngine;

namespace RoundBallGame.Systems.Utils
{
    public class SimpleGizmo : MonoBehaviour
    {
        [Header("Gizmo Settings")]
        [SerializeField] private Color gizmoColor = Color.green;
        [SerializeField] private Vector3 gizmoSize = Vector3.one;
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            // Draw a semitransparent green cube at the transforms position
            Gizmos.color = gizmoColor;
            Gizmos.DrawCube(transform.position, gizmoSize);
        } 
#endif
    }
}