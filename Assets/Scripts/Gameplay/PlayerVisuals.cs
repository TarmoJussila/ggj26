using System;
using System.Collections.Generic;
using System.Linq;
using Logbound.Data;
using Logbound.Masks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Logbound.Gameplay
{
    public class PlayerVisuals : MonoBehaviour
    {
        [SerializeField] private float _animSpeed;

        [SerializeField] private int _playerIndex;

        [SerializeField] private List<MaskFrames> _maskFrames;

        private MaskFrames _currentMaskFrames;

        public MaskType CurrentMaskType;

        public List<SpritePair> WalkAnim;
        public List<SpritePair> IdleAnim;
        public List<SpritePair> JumpAnim;
        public List<SpritePair> HitAnim;

        [SerializeField] private SpriteRenderer _rend;
        [SerializeField] private SpriteRenderer _maskRend;

        public Anim CurrentAnimation { get; private set; }

        private List<SpritePair> _currentAnim;
        private int _currentFrame;
        private int _maxFrames;

        private float _frameTimer;

        private Transform _targetPlayerTransform;

        [SerializeField] private float _minimumPlayTimeBeforeSwitch = 0.3f;
        private float _currentPlayTime;

        private SplitScreenPlayer _splitScreenPlayer;

        private void Awake()
        {
            PlayerJoinHelper.OnPlayerAdded += CheckPlayers;
            PlayerJoinHelper.OnPlayerRemoved += CheckPlayers;

            _splitScreenPlayer = GetComponentInParent<SplitScreenPlayer>();
        }

        private void OnDestroy()
        {
            PlayerJoinHelper.OnPlayerAdded -= CheckPlayers;
            PlayerJoinHelper.OnPlayerRemoved -= CheckPlayers;
        }

        private void Start()
        {
            SetAnimation(Anim.Walk);
            CheckPlayers();
        }

        private void CheckPlayers()
        {
            var inputs = FindObjectsByType<PlayerInput>(FindObjectsSortMode.None);

            var matchingInput = inputs.FirstOrDefault(i => i.playerIndex == _playerIndex);

            PlayerInput self = GetComponentInParent<PlayerInput>();

            if (matchingInput != null)
            {
                _targetPlayerTransform = matchingInput.transform;
            }

            _rend.enabled = matchingInput != null && matchingInput != self;
        }

        private void Update()
        {
            if (_currentAnim == null || _targetPlayerTransform == null)
            {
                return;
            }

            _frameTimer += Time.deltaTime;
            _currentPlayTime += Time.deltaTime;

            if (_targetPlayerTransform != null)
            {
                   Vector3 forward = _targetPlayerTransform.position - transform.position;
                   forward.y = 0;
                   transform.forward = forward;
            }

            if (_frameTimer >= _animSpeed)
            {
                _currentFrame++;

                if (_currentFrame >= _maxFrames)
                {
                    _currentFrame = 0;
                }

                _frameTimer = 0f;
            }

            bool front = Vector3.Dot((transform.position - _targetPlayerTransform.position), _splitScreenPlayer.transform.forward) < 0;

            _rend.sprite = front ? _currentAnim[_currentFrame].Front : _currentAnim[_currentFrame].Back;

            if (_currentMaskFrames != null && _maskRend != null)
            {
                _maskRend.enabled = front && CurrentMaskType != MaskType.NONE;

                _maskRend.sprite = _currentMaskFrames.Frames[_currentFrame];
            }
        }

        public void SetMaskType(MaskType maskType)
        {
            CurrentMaskType = maskType;
            _maskRend.enabled = maskType != MaskType.NONE;

            if (CurrentMaskType != MaskType.NONE)
            {
                _currentMaskFrames = _maskFrames.FirstOrDefault(mf => mf.MaskType == CurrentMaskType);
            }
            else
            {
                _currentMaskFrames = null;
            }
        }

        public void SetAnimation(Anim anim, bool force = false)
        {
            if (CurrentAnimation == anim && !force)
            {
                return;
            }

            if (!force && _currentAnim != null && _currentPlayTime < _minimumPlayTimeBeforeSwitch)
            {
                return;
            }

            _currentPlayTime = 0.0f;
            _currentAnim = new(GetFrames(anim));
            CurrentAnimation = anim;
            _currentFrame = 0;
            _maxFrames = _currentAnim.Count;
        }

        private List<SpritePair> GetFrames(Anim anim)
        {
            switch (anim)
            {
                case Anim.Idle: return IdleAnim;
                case Anim.Walk: return WalkAnim;
                case Anim.Jump: return JumpAnim;
                case Anim.Hit: return HitAnim;
                default: return IdleAnim;
            }
        }
    }

    [Serializable]
    public struct SpritePair
    {
        public Sprite Front;
        public Sprite Back;
    }

    public enum Anim
    {
        Idle,
        Walk,
        Jump,
        Hit
    }
}
