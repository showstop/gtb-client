// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.05 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.05;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:0,uamb:True,mssp:True,lmpd:False,lprd:False,rprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:2,bsrc:0,bdst:0,culm:0,dpts:2,wrdp:False,dith:2,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1457,x:33288,y:32719,varname:node_1457,prsc:2|emission-9525-OUT;n:type:ShaderForge.SFN_Tex2d,id:2230,x:32007,y:32827,ptovrint:False,ptlb:Texture,ptin:_Texture,varname:node_2230,prsc:2,tex:57200c6da9b09444687c5e78b3bbe893,ntxv:2,isnm:False;n:type:ShaderForge.SFN_Color,id:5327,x:32242,y:32573,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_5327,prsc:2,glob:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:316,x:32493,y:32832,varname:node_316,prsc:2|A-9569-OUT,B-5327-RGB,C-8967-OUT;n:type:ShaderForge.SFN_ValueProperty,id:8967,x:32493,y:32700,ptovrint:False,ptlb:Emission,ptin:_Emission,varname:node_8967,prsc:2,glob:False,v1:1;n:type:ShaderForge.SFN_VertexColor,id:3762,x:32637,y:32874,varname:node_3762,prsc:2;n:type:ShaderForge.SFN_Multiply,id:9525,x:32857,y:32793,varname:node_9525,prsc:2|A-316-OUT,B-3762-RGB,C-3762-A,D-287-OUT;n:type:ShaderForge.SFN_Lerp,id:9569,x:32251,y:32916,varname:node_9569,prsc:2|A-8405-OUT,B-2230-RGB,T-2230-A;n:type:ShaderForge.SFN_Vector1,id:8405,x:31995,y:33110,varname:node_8405,prsc:2,v1:0;n:type:ShaderForge.SFN_Slider,id:5073,x:32493,y:32516,ptovrint:False,ptlb:SoftParticle_Value,ptin:_SoftParticle_Value,varname:node_5073,prsc:2,min:0,cur:0,max:1;n:type:ShaderForge.SFN_SwitchProperty,id:287,x:33050,y:32461,ptovrint:False,ptlb:Use_SoftPaticle,ptin:_Use_SoftPaticle,varname:node_287,prsc:2,on:False|A-7306-OUT,B-6100-OUT;n:type:ShaderForge.SFN_DepthBlend,id:6100,x:32843,y:32526,varname:node_6100,prsc:2|DIST-5073-OUT;n:type:ShaderForge.SFN_Vector1,id:7306,x:32843,y:32461,varname:node_7306,prsc:2,v1:1;proporder:5327-2230-8967-287-5073;pass:END;sub:END;*/

Shader "FT/Emission" {
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _Texture ("Texture", 2D) = "black" {}
        _Emission ("Emission", Float ) = 1
        [MaterialToggle] _Use_SoftPaticle ("Use_SoftPaticle", Float ) = 1
        _SoftParticle_Value ("SoftParticle_Value", Range(0, 1)) = 0
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
            Blend One One
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform sampler2D _CameraDepthTexture;
            uniform sampler2D _Texture; uniform float4 _Texture_ST;
            uniform float4 _Color;
            uniform float _Emission;
            uniform float _SoftParticle_Value;
            uniform fixed _Use_SoftPaticle;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
                float4 projPos : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                float sceneZ = max(0,LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)))) - _ProjectionParams.g);
                float partZ = max(0,i.projPos.z - _ProjectionParams.g);
/////// Vectors:
////// Lighting:
////// Emissive:
                float node_8405 = 0.0;
                float4 _Texture_var = tex2D(_Texture,TRANSFORM_TEX(i.uv0, _Texture));
                float3 emissive = ((lerp(float3(node_8405,node_8405,node_8405),_Texture_var.rgb,_Texture_var.a)*_Color.rgb*_Emission)*i.vertexColor.rgb*i.vertexColor.a*lerp( 1.0, saturate((sceneZ-partZ)/_SoftParticle_Value), _Use_SoftPaticle ));
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
