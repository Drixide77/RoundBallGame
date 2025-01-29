using RoundBallGame.Systems.Data;
using RoundBallGame.Systems.Services;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RoundBallGame.Systems.UI
{
    public class UIButtonAudioEvents : MonoBehaviour, IPointerClickHandler
    {
        [Header("Audio Clips")]
        [SerializeField] private AudioRepositoryEntryId OnClick;
        
        private Button _button;
        private Toggle _toggle;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _toggle = GetComponent<Toggle>();
        }

        public void OnPointerClick(PointerEventData pointerEventData)
        {
            if ((_button != null && !_button.interactable) || (_toggle != null && !_toggle.interactable)) return;
            
            AudioService.Instance.PlaySFXClip(OnClick);
        }
    }
}
