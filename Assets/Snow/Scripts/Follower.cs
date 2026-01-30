using System;
using UnityEngine;

public class Follower : MonoBehaviour
{
    [SerializeField] private Transform _target;
    
    private Vector3 _offset;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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

    // Update is called once per frame
    void Update()
    {
        Shader.SetGlobalVector("_MainCameraPosition", transform.position);
        transform.position = _target.position - _offset;
    }
}
