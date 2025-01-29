using RoundBallGame.Systems.Data;
using RoundBallGame.Systems.Services;
using UnityEngine;

namespace RoundBallGame.Gameplay.Elements
{
    public class PlayerBallController : MonoBehaviour
    {
        private Rigidbody2D rigidBody;
        
        private void Awake()
        {
            rigidBody = GetComponent<Rigidbody2D>();
        }

        public void Initialize()
        {
            rigidBody.velocity = Vector2.zero;
            rigidBody.angularVelocity = 0f;
            transform.rotation = Quaternion.identity;
        }
        
        public void Show()
        {
            gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            gameObject.SetActive(false);
        }
        
        public void MoveTo(Vector3 position)
        {
            transform.position = position;
            rigidBody.velocity = Vector2.zero;
        }
        
        public void ApplyVelocity(Vector2 velocity)
        {
            rigidBody.velocity = velocity;
        }
        
        private void OnCollisionEnter2D(Collision2D _)
        {
            // Boing!
            AudioService.Instance.PlaySFXClip(AudioRepositoryEntryId.PlayerBounceSound);
        }
    }
}
