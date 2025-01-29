using RoundBallGame.Gameplay.Camera;
using RoundBallGame.Systems.Data;
using RoundBallGame.Systems.Services;
using UnityEngine;

namespace RoundBallGame.Gameplay.Elements
{
    public class CannonController : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Transform cannonPivot;
        [SerializeField] private Transform playerBallOriginOnShoot;
        [SerializeField] private GameObject cannonGraphic;
        [Header("Settings")]
        [SerializeField] private float shootingVelocity = 25f;
        [Header("Scene References")]
        [SerializeField] private Transform aimingCameraTarget;
        
        private bool inAimingMode = false;
        private BoxCollider2D cannonCollider;
        private PlayerBallController playerReference;
        private UnityEngine.Camera mainCamera;
        private CameraController _cameraController;

        private void Awake()
        {
            cannonCollider = GetComponent<BoxCollider2D>();
            mainCamera = UnityEngine.Camera.main;
        }

        private void Update()
        {
            if (inAimingMode && Time.timeScale > 0.1f) // Prevents aiming or firing while game is paused
            {
                Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                Vector3 direction = (mousePosition - cannonPivot.position).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                cannonPivot.rotation = Quaternion.Euler(0, 0, angle);

                if (Input.GetMouseButtonDown(0))
                {
                    ShootPlayerBall();
                }
            }
        }

        public void Initialize(CameraController cameraController)
        {
            cannonGraphic.SetActive(false);
            inAimingMode = false;
            cannonCollider.enabled = true;
            _cameraController = cameraController;
        }

        private void ShootPlayerBall()
        {
            AudioService.Instance.PlaySFXClip(AudioRepositoryEntryId.CannonShotSound);
            inAimingMode = false;
            _cameraController.SetCannonMode(false);
            cannonGraphic.SetActive(false);
            playerReference.MoveTo(playerBallOriginOnShoot.position);
            playerReference.Show();
            playerReference.ApplyVelocity(cannonPivot.right * shootingVelocity);
        }

        // Collisions
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                playerReference = other.gameObject.GetComponent<PlayerBallController>();
                playerReference.Hide();
                inAimingMode = true;
                _cameraController.SetCannonMode(true);
                _cameraController.SetTarget(aimingCameraTarget, false);
                _cameraController.MoveToTarget();
                cannonCollider.enabled = false;
                cannonGraphic.SetActive(true);
            }
        }
    }
}