using System;
using UnityEngine;

namespace RoundBallGame.Gameplay
{
    public class CannonController : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Transform cannonPivot;
        [SerializeField] private Transform playerBallOriginOnShoot;
        [SerializeField] private GameObject cannonGraphic;
        [Header("Settings")]
        [SerializeField] private float shootingVelocity = 25f;
        
        private bool inAimingMode = false;
        private BoxCollider2D cannonCollider;
        private PlayerBallController playerReference;
        private Camera mainCamera;

        private void Awake()
        {
            cannonCollider = GetComponent<BoxCollider2D>();
            mainCamera = Camera.main;
            Initialize();
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

        public void Initialize()
        {
            cannonGraphic.SetActive(false);
            inAimingMode = false;
            cannonCollider.enabled = true;
        }

        private void ShootPlayerBall()
        {
            inAimingMode = false;
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
                cannonCollider.enabled = false;
                cannonGraphic.SetActive(true);
            }
        }
    }
}