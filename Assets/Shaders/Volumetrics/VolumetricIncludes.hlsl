#ifndef VOLUMETRICINCLUDES_INCLUDED
#define VOLUMETRICINCLUDES_INCLUDED

//#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
//#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
//#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
//#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
//#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RealtimeLights.hlsl"

#include "shadows_stripped.hlsl"
#include "realtimelights_stripped.hlsl"

#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
#pragma multi_compile _ _SHADOWS_SOFT

#pragma multi_compile_fragment _SHADOWS_SOFT _SHADOWS_SOFT_LOW _SHADOWS_SOFT_MEDIUM _SHADOWS_SOFT_HIGH
//#define MAX_VISIBLE_LIGHTS = 64

//#ifdef UNIVERSAL_SHADOWS_INCLUDED
void RaymarchLights_float(float2 UV, float3 start, float3 end, float depth, float maxLength, int steps,
                          out float3 color)
{
    //float3 start = _WorldSpaceCameraPos;
    //tex2D(_CameraDepthTexture, uv);
    end = ComputeWorldSpacePosition(UV, depth, UNITY_MATRIX_I_VP);

    float3 ray = end - start;
    float3 dir = normalize(ray);

    float rayLength = clamp(length(ray), 0, maxLength);

    float stepLength = rayLength / steps;
    float3 step = dir * stepLength;

    float accumulatedLight = 0;
    float3 currentPosition = start;

    for (int i = 0; i < steps; ++i)
    {
        //half light = GetMainLightShadowFade(currentPosition);
        half light = MainLightRealtimeShadow(TransformWorldToShadowCoord(currentPosition));
        //half light = AdditionalLightRealtimeShadow(0, TransformWorldToShadowCoord(currentPosition));
        //half light = GetMainLight(TransformWorldToShadowCoord(currentPosition)).shadowAttenuation;

        accumulatedLight += light;
        currentPosition += step;
    }

    accumulatedLight /= (steps);
    //accumulatedLight = 1 - accumulatedLight;
    
    color = float3(accumulatedLight, accumulatedLight, accumulatedLight);
    //color = float3(end);
    //color = float3(depth, 0, 0);

    // half endlight = GetMainLight(TransformWorldToShadowCoord(end)).shadowAttenuation;
    // color = float3(endlight, endlight, endlight);

}
#endif
