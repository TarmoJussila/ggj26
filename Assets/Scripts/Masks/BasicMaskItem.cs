using System;
using System.Collections.Generic;
using System.Linq;
using Logbound.Gameplay;
using UnityEngine;

namespace Logbound.Masks
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class BasicMaskItem : InteractableItem
    {
        [field: SerializeField] public MaskType MaskType { get; private set; }
        
        private Rigidbody _rb;
        private Collider[] _colliders;

        private SpriteRenderer _rend;

        public PlayerInteraction BeingWornBy { get; private set; }

        private Camera[] _cameras;
        private float _billboardRefreshTimer = 1.0f;

        private Transform _billboardTarget;
        
        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _colliders = GetComponentsInChildren<Collider>();
            _rend = GetComponentInChildren<SpriteRenderer>();

            PlayerJoinHelper.OnPlayerAdded += GetCameras;
        }

        private void OnDestroy()
        {
            PlayerJoinHelper.OnPlayerRemoved += GetCameras;
        }

        private void Start()
        {
            GetCameras();
            FindNearestCamera();
        }

        private void GetCameras()
        {
            _cameras = FindObjectsByType<Camera>(FindObjectsSortMode.None);
        }

        private void Update()
        {
            _billboardRefreshTimer -= Time.deltaTime;
            if (_billboardRefreshTimer < 0.0f)
            {
                FindNearestCamera();
                _billboardRefreshTimer = 0.1f;
            }

            transform.forward = _billboardTarget.transform.position - transform.position;
        }

        private void FindNearestCamera() {
            GetCameras();

            _cameras = _cameras.OrderBy(c => Vector3.Distance(c.transform.position, transform.position)).ToArray();

            _billboardTarget = _cameras[0].transform;
        }

        public override void Interact(PlayerInteraction playerInteraction)
        {
            playerInteraction.GetComponent<PlayerMaskHelper>().WearMask(this);
            
            StartWearing(playerInteraction);
        }

        public void StartWearing(PlayerInteraction playerInteraction)
        {
            _rb.isKinematic = true;
            _rend.enabled = false;

            foreach (Collider col in _colliders)
            {
                col.enabled = false;
            }

            BeingWornBy = playerInteraction;
        }

        public void StopCarry()
        {
            _rb.isKinematic = false;
            _rend.enabled = true;

            foreach (Collider col in _colliders)
            {
                col.enabled = true;
            }

            BeingWornBy = null;
        }
    }

    public enum MaskType
    {
        NONE = -1,
        GAS = 0,
        COOL = 1,
        WELDING = 2,
        SKIMASK = 3,
        COVID = 4,
        AIRSOFT = 5,
        PROTECTIVE = 6,
        SKINCARE = 7,
        SLEEPING = 8
    }
}
