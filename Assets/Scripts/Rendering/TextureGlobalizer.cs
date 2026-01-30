using UnityEngine;

public class TextureGlobalizer : MonoBehaviour
{
    [SerializeField] private RenderTexture _texture;
    [SerializeField] private string _textureName;

    private void Awake()
    {
        SetGlobal();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        SetGlobal();
    }
#endif

    public void SetGlobal()
    {
        Shader.SetGlobalTexture(_textureName, _texture);
    }
}