Shader "Custom/HologramFresnel"
{
    Properties
    {
        _MainColor ("Main Color", Color) = (0,0.5,1,1)
        _RimColor ("Rim Color", Color) = (0,1,1,1)

        _Alpha ("Main Alpha", Range(0,1)) = 0.2
        _RimAlpha ("Rim Alpha", Range(0,1)) = 1.0

        _FresnelPower ("Rim Width", Range(0,8)) = 2
        _Intensity ("Intensity", Range(0,20)) = 5
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Transparent"
            "Queue"="Transparent"
        }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Back

            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 normalWS : TEXCOORD0;
                float3 viewDirWS : TEXCOORD1;
            };

            float4 _MainColor;
            float4 _RimColor;

            float _Alpha;
            float _RimAlpha;
            float _FresnelPower;
            float _Intensity;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                VertexPositionInputs pos =
                    GetVertexPositionInputs(IN.positionOS.xyz);

                OUT.positionCS = pos.positionCS;
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                OUT.viewDirWS = GetWorldSpaceViewDir(pos.positionWS);

                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float3 N = normalize(IN.normalWS);
                float3 V = normalize(IN.viewDirWS);

                float ndv = saturate(dot(N, V));

                // Higher value = wider rim
                float rim = pow(1.0 - ndv, 1.0 / max(_FresnelPower, 0.001));

                float3 mainColor =
                    _MainColor.rgb * _Intensity;

                float3 rimColor =
                    _RimColor.rgb * rim * _Intensity;

                float3 finalColor =
                    mainColor + rimColor;

                float finalAlpha =
                    saturate(_Alpha + rim * _RimAlpha);

                return half4(finalColor, finalAlpha);
            }

            ENDHLSL
        }
    }
}