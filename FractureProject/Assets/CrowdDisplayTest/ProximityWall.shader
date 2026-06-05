Shader "Custom/ProximityWall"
{
    Properties
    {
        _MainTex ("Texture (Panneau Stop)", 2D) = "white" {}
        _BaseColor ("Couleur de Base (Loin)", Color) = (1, 0, 0, 0.3)
        _GlowColor ("Couleur Proche (Près)", Color) = (1, 0.2, 0.2, 0.9)
        _PlayerPos ("Position du Joueur", Vector) = (0,0,0,0)
        _EffectRadius ("Rayon de l'effet", Float) = 4.0
        _EmissionIntensity ("Intensité de la Surbrillance", Float) = 2.0
        _WorldYOffset ("Position Y dans le Monde", Float) = 0.0
        
        _CrowdHeadDist ("Tête de Foule (UV X)", Float) = 1000.0
        _CrowdTailDist ("Queue de Foule (UV X)", Float) = -1000.0
    }
    
    SubShader
    {
        Tags 
        { 
            "RenderType" = "Transparent" 
            "Queue" = "Transparent" 
            "RenderPipeline" = "UniversalPipeline" 
        }
        LOD 100
        
        Cull Off 
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                float rawUvX : TEXCOORD2;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float4 _BaseColor;
                float4 _GlowColor;
                float4 _PlayerPos;
                float _EffectRadius;
                float _EmissionIntensity;
                float _WorldYOffset;
                float _CrowdHeadDist;
                float _CrowdTailDist;
            CBUFFER_END

            Varyings vert (Attributes v)
            {
                Varyings o;
                
                float3 worldPos = TransformObjectToWorld(v.positionOS.xyz);
                
                worldPos.y += _WorldYOffset;
                o.worldPos = worldPos;
                
                o.positionCS = TransformWorldToHClip(worldPos);
                
                o.rawUvX = v.uv.x; 
                
                o.uv = v.uv * _MainTex_ST.xy + _MainTex_ST.zw;
                return o;
            }

            half4 frag (Varyings i) : SV_Target
            {
                if (i.rawUvX <= _CrowdHeadDist && i.rawUvX >= _CrowdTailDist)
                {
                    return half4(0, 0, 0, 0); 
                }

                half4 texColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                
                float dist = distance(i.worldPos, _PlayerPos.xyz);
                
                float influence = 1.0 - saturate(dist / _EffectRadius);
                influence = smoothstep(0.0, 1.0, influence);

                half4 currentColor = lerp(_BaseColor, _GlowColor, influence);
                
                half4 finalColor;
                finalColor.rgb = texColor.rgb * currentColor.rgb * (1.0 + influence * _EmissionIntensity);
                
                half effectiveTexAlpha = lerp(texColor.a, 1.0, influence);
                
                finalColor.a = currentColor.a * effectiveTexAlpha; 

                return finalColor;
            }
            ENDHLSL
        }
    }
}