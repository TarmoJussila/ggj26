using UnityEngine;

namespace Logbound.Gameplay
{
    public class SnowShaderCamera : MonoBehaviour
    {
        [SerializeField] private RenderTexture _drawRt;
        [SerializeField] private RenderTexture _globalRt;

        [SerializeField] private Transform _target;

        [SerializeField] private float _yPos = -64;

        [SerializeField] private float _snowWorldHeightMin;
        [SerializeField] private float _snowWorldHeightMax;

        private Vector3 _lastPos;

        private void Awake()
        {
            var cam = GetComponent<Camera>();
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
}