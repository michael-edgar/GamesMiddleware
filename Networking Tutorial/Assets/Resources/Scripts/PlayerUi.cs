using System;
using UnityEngine;
using UnityEngine.UI;

namespace Resources.Scripts
{
    public class PlayerUi : MonoBehaviour
    {

        #region Private Fields

        [Tooltip("UI Text to display Player's Name")] [SerializeField]
        private Text playerNameText;

        [Tooltip("UI Slider to display Player's Health")] [SerializeField]
        private Slider playerHealthSlider;

        private PlayerManager _target;
        private float _characterControllerHeight = 0f;
        private Transform _targetTransform;
        private Renderer _targetRenderer;
        private CanvasGroup _canvasGroup;
        private Vector3 _targetPosition;
            
        [Tooltip("Pixel offset from the player target")]
        [SerializeField]
        private Vector3 screenOffset = new Vector3(0f,30f,0f);

        #endregion

        #region MonoBehaviour Callbacks

        void Update()
        {
            // Reflect the Player Health
            if (playerHealthSlider != null)
            {
                playerHealthSlider.value = _target.health;
            }

            if (_target == null)
            {
                Destroy(this.gameObject);
                return;
            }
        }

        private void Awake()
        {
            this.transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
            _canvasGroup = this.GetComponent<CanvasGroup>();
        }

        private void LateUpdate()
        {
            // Do not show the UI if we are not visible to the camera, thus avoiding potential bugs with seeing the UI but not the player itself.
            if (_targetRenderer != null)
            {
                this._canvasGroup.alpha = _targetRenderer.isVisible ? 1f : 0f;
            }
            
            //#Critical
            // Follow the Target GameObject on screen.
            if (_targetTransform != null)
            {
                _targetPosition = _targetTransform.position;
                _targetPosition.y += _characterControllerHeight;
                this.transform.position = Camera.main.WorldToScreenPoint(_targetPosition) + screenOffset;
            }
        }

        #endregion

        #region Public Methods

        public void SetTarget(PlayerManager target)
        {
            if (target == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> PlayerManager target for PlayerUI.SetTarget", this);
                return;
            }

            _target = target;
            _targetTransform = this._target.GetComponent<Transform>();
            _targetRenderer = this._target.GetComponent<Renderer>();
            CharacterController characterController = _target.GetComponent<CharacterController>();
            
            // Get data from the Player that won't change during the lifetime of this Componenet
            if (characterController != null)
            {
                _characterControllerHeight = characterController.height;
            }
            
            if (playerNameText != null)
            {
                playerNameText.text = _target.photonView.Owner.NickName;
            }
        }

        #endregion
    }
}
