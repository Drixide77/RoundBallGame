using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace RoundBallGame.Gameplay.Camera
{
    public class CameraController : MonoBehaviour
    {
        [Header("Scene References")]
        [SerializeField] private Transform targetTransform;
        [SerializeField] private UnityEngine.Camera followCamera;
        [Header("Settings")]
        [SerializeField] private CameraConfig cameraConfig;
        
        private Vector3 target;
        private Vector3 lastPosition;
        private Vector3 currentVelocity;
        private float currentSizeDelta;
        private float lastSize;

        private Transform playerCachedTransform;
        bool isInCannonMode = false;

        private void Start()
        {
            lastSize = cameraConfig.DefaultSize;
            followCamera.orthographicSize = lastSize;
        }

        private void LateUpdate()
        {
            UpdateTarget();
            MoveCamera();
            UpdateScale();
        }
        
        public void SetTarget(Transform followTarget, bool isPlayer = true)
        {
            targetTransform = followTarget;
            lastPosition = targetTransform.position;
            if (isPlayer)
                playerCachedTransform = followTarget;
        }

        public void MoveToTarget(bool instant = false)
        {
            target = targetTransform.position;
            target.z = cameraConfig.Depth;
            if (instant) followCamera.transform.position = target;
        }
        
        public void SetCannonMode(bool isEnabled)
        {
            isInCannonMode = isEnabled;
            // Set cannon mode to false handles the camera changes
            if (!isInCannonMode)
            {
                targetTransform = playerCachedTransform;
            }
        }

        private void UpdateTarget()
        {
            if (targetTransform == null) return;
            Vector3 movementDelta = targetTransform.position - lastPosition;
            Vector3 screenPos = followCamera.WorldToScreenPoint(targetTransform.position);
            Vector3 bottomLeft = followCamera.ViewportToScreenPoint(new Vector3(cameraConfig.HorizontalMargin,cameraConfig.VerticalMargin,0));
            Vector3 topRight = followCamera.ViewportToScreenPoint(new Vector3(1 - cameraConfig.HorizontalMargin, 1 - cameraConfig.VerticalMargin, 0));

            if (screenPos.x < bottomLeft.x || screenPos.x > topRight.x)
            {
                target.x += movementDelta.x;
            }

            if (screenPos.y < bottomLeft.y || screenPos.y > topRight.y)
            {
                target.y += movementDelta.y;
            }

            target.z = cameraConfig.Depth;
            lastPosition = targetTransform.position;
        }

        private void MoveCamera()
        {
            followCamera.transform.position = Vector3.SmoothDamp(followCamera.transform.position, target, ref currentVelocity, cameraConfig.SmoothTime);
        }

        private void UpdateScale()
        {
            followCamera.orthographicSize = Mathf.SmoothDamp(followCamera.orthographicSize, isInCannonMode ? cameraConfig.CannonModeSize : cameraConfig.DefaultSize, ref currentSizeDelta, cameraConfig.SizeSmoothTime);
        }
    }
}
