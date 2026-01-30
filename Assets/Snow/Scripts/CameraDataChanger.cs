using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraDataChanger : MonoBehaviour
{
    [SerializeField] private int _rendererIndex;

    [ContextMenu("Apply")]
    public void UpdateValues()
    {
        GetComponent<UniversalAdditionalCameraData>().SetRenderer(_rendererIndex);
    }
}