#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareNormalsTexture.hlsl"

void GetSceneNormal_float(float2 Uv, out float3 Out)
{
  Out = SampleSceneNormals(Uv);
}