using UnityEngine;

namespace Logbound.Gameplay
{
    public class BallRoller : MonoBehaviour
    {
        [SerializeField] private float _speed = 10f;
        [SerializeField] private float _jump = 10f;
        
        private Rigidbody _rb;
        
        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            float horizontal = Input.GetAxis("Horizontal") * _speed;
            float vertical = Input.GetAxis("Vertical") * _speed;
            float jump = Input.GetAxis("Jump") * _jump;
            _rb.AddForce(new Vector3(horizontal, jump, vertical));
        }
    }
}