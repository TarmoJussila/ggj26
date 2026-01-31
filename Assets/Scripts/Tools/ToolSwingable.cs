using System;
using System.Collections;
using Logbound.Gameplay;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace Logbound
{
    public abstract class ToolSwingable : CarryableItem
    {
        [SerializeField] private AnimationCurve _swingCurve;
        [SerializeField] private float _startRotation = 0.0f;
        [SerializeField] private float _endRotation = 65f;
        [SerializeField] private Transform _rotateRoot;
        [SerializeField] private float _animDuration = 0.3f;

        private bool _isSwinging;

        private void OnValidate()
        {
            if (_rotateRoot == null)
            {
                _rotateRoot = transform.GetChild(0);
            }
        }

        public override void Interact(PlayerInteraction playerInteraction)
        {
            if (_isSwinging)
            {
                return;
            }

            SwingTool();
            playerInteraction.GetComponentInChildren<PlayerAnimator>().SetAnimation(Anim.Hit, true);

            OnToolUsed(playerInteraction);
        }

        protected virtual void OnToolUsed(PlayerInteraction playerInteraction) { }

        private void SwingTool()
        {
            _isSwinging = true;

            StartCoroutine(SwingAnimation());
        }

        private IEnumerator SwingAnimation()
        {
            float t = 0.0f;

            Vector3 euler = transform.localRotation.eulerAngles;

            while (t < 1.0f)
            {
                t += Time.deltaTime / _animDuration;

                transform.localRotation = Quaternion.Euler(Mathf.Lerp(_startRotation, _endRotation, _swingCurve.Evaluate(t)), euler.y, euler.z);
                yield return null;
            }

            transform.localRotation = Quaternion.Euler(euler);

            _isSwinging = false;
        }
    }
}
