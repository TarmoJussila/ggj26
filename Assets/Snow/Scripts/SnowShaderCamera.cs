using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SnowShaderCamera : MonoBehaviour
{
    [SerializeField] RenderTexture _drawRt;
    [SerializeField] RenderTexture _globalRt;

    [SerializeField] Transform _target;

    [SerializeField] private float _yPos = -64;

    [SerializeField] private float _snowWorldHeightMin;
    [SerializeField] private float _snowWorldHeightMax;

    private Vector3 _lastPos;

    // Start is called before the first frame update
    void Awake()
    {
        Camera cam = GetComponent<Camera>();
        cam.depthTextureMode = DepthTextureMode.DepthNormals;

        Shader.SetGlobalTexture("_GlobalEffectRT", _globalRt);
        Shader.SetGlobalFloat("_OrthographicCamSize", GetComponent<Camera>().orthographicSize);
    }

    private void Update()
    {
        transform.position =
            new Vector3(_target.transform.position.x, _yPos,
                _target.transform.position.z);
        Shader.SetGlobalVector("_Position", transform.position);
        Shader.SetGlobalVector("_PositionDelta", transform.position - _lastPos);
        
        Shader.SetGlobalFloat("_SnowWorldHeightMin", _snowWorldHeightMin);
        Shader.SetGlobalFloat("_SnowWorldHeightMax", _snowWorldHeightMax);

        _lastPos = transform.position;
    }
}