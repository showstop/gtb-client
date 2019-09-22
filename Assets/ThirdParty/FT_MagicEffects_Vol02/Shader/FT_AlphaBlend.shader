// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.05 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.05;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:0,uamb:True,mssp:True,lmpd:False,lprd:False,rprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:1,bsrc:3,bdst:7,culm:0,dpts:2,wrdp:False,dith:2,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1457,x:33288,y:32719,varname:node_1457,prsc:2|emission-316-OUT,alpha-9525-OUT;n:type:ShaderForge.SFN_Tex2d,id:2230,x:32007,y:32827,ptovrint:False,ptlb:Texture,ptin:_Texture,varname:node_2230,prsc:2,tex:6b6582862e488694db9e84872a8fa4c5,ntxv:2,isnm:False;n:type:ShaderForge.SFN_Color,id:5327,x:32242,y:32573,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_5327,prsc:2,glob:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:316,x:32507,y:32780,varname:node_316,prsc:2|A-2230-RGB,B-5327-RGB,C-3762-RGB,D-3800-OUT;n:type:ShaderForge.SFN_VertexColor,id:3762,x:32302,y:32949,varname:node_3762,prsc:2;n:type:ShaderForge.SFN_Multiply,id:9525,x:32812,y:32942,varname:node_9525,prsc:2|A-2230-A,B-3762-A;n:type:ShaderForge.SFN_ValueProperty,id:3800,x:32490,y:32619,ptovrint:False,ptlb:Emission,ptin:_Emission,varname:node_3800,prsc:2,glob:False,v1:1;proporder:5327-2230-3800;pass:END;sub:END;*/

Shader "FT/AlphaBlend" {
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _Texture ("Texture", 2D) = "black" {}
        _Emission ("Emission", Float ) = 1
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform sampler2D _Texture; uniform float4 _Texture_ST;
            uniform float4 _Color;
            uniform float _Emission;
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
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
/////// Vectors:
////// Lighting:
////// Emissive:
                float4 _Texture_var = tex2D(_Texture,TRANSFORM_TEX(i.uv0, _Texture));
                float3 emissive = (_Texture_var.rgb*_Color.rgb*i.vertexColor.rgb*_Emission);
                float3 finalColor = emissive;
                return fixed4(finalColor,(_Texture_var.a*i.vertexColor.a));
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
