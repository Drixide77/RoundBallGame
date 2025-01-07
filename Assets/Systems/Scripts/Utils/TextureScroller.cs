using UnityEngine;

namespace RoundBallGame.Systems.Utils
{
    public class TextureScroller : MonoBehaviour
    {
        [Header("Material")]
        [SerializeField] private Material material;
        [Header("Settings")]
        [SerializeField] private Vector2 scrollSpeed = Vector2.zero;
        
        private Renderer _renderer;
        private Vector2 _textureOffset;

        void Start()
        {
            _renderer = gameObject.GetComponent<Renderer>();
            _renderer.material = new Material(material);
        }

        void Update()
        {
            _textureOffset = new Vector2(Time.time * scrollSpeed.x,Time.time * scrollSpeed.y);
            
            _renderer.material.mainTextureOffset = _textureOffset;
        }
    }
}