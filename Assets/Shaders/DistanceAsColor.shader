Shader "Unlit/DistanceAsColor"
{
    Properties {}
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float4 vertexWorld : TEXCOORD1;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.vertexWorld = mul(unity_ObjectToWorld, v.vertex);
                //UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }

            fixed invLerp(float a, float b, float v)
            {
                return (v - a) / (b - a);
            }

            uniform float _SnowWorldHeightMin;
            uniform float _SnowWorldHeightMax;

            fixed4 frag(v2f i) : SV_Target
            {
                float4 v = mul(unity_ObjectToWorld, i.vertex);
                float t = invLerp(_SnowWorldHeightMax, _SnowWorldHeightMin, i.vertexWorld.y);
                return fixed4(t, 0, 0, 1);
            }
            ENDCG
        }
    }
}