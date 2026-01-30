using UnityEngine;

namespace Logbound.Gameplay
{
    public class Follower : MonoBehaviour
    {
        [SerializeField] private Transform _target;

        private Vector3 _offset;

        void Start()
        {
            _offset = _target.position - transform.position;
        }

    #if UNITY_EDITOR
        private void OnValidate()
        {
            Shader.SetGlobalVector("_MainCameraPosition", transform.position);
        }
    #endif

        void Update()
        {
            Shader.SetGlobalVector("_MainCameraPosition", transform.position);
            transform.position = _target.position - _offset;
        }
    }
}