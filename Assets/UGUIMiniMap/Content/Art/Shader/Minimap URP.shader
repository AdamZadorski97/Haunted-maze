Shader "Lovatto/MiniMap/Plane URP" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _Power("Power", Range(0, 2)) = 1
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Geometry+1"
            "RenderType"="Opaque"
            "PreviewType"="Plane"
            "RenderPipeline" = "UniversalPipeline"
        }
        Pass {
            Name "StandardLit"
            Tags {
                "LightMode"="UniversalForward"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off
            
             HLSLPROGRAM
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D( _MainTex);
            SAMPLER(sampler_MainTex);
            uniform float4 _MainTex_ST;
            uniform float4 _Color;
            uniform float _Power;

            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };

            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };

            VertexOutput vert (VertexInput v) {

                VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);

                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = vertexInput.positionCS;
                return o;
            }

            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float4 _MainTex_var = _MainTex.Sample(sampler_MainTex,i.uv0);
                _MainTex_var = pow(_MainTex_var.rgba, _Power);
                return _MainTex_var;
            }
                ENDHLSL
        }
    }
    FallBack "Diffuse"
}
