using UnityEngine;

namespace RoundBallGame.Gameplay.Camera
{
    [CreateAssetMenu(fileName = "CameraConfig", menuName = "RoundBallGame/CameraConfig")]
    public class CameraConfig : ScriptableObject
    {
        [Header("Follow")]
        public float HorizontalMargin = 0.3f;
        public float VerticalMargin = 0.4f;
        public float Depth = -10;
        public float SmoothTime = 0.25f;
        [Header("Size")]
        public float DefaultSize = 8f;
        public float CannonModeSize = 10f;
        public float SizeSmoothTime = 0.1f;
    }
}