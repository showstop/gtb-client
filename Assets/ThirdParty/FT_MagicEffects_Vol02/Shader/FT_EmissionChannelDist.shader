// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.05 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.05;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:0,uamb:True,mssp:True,lmpd:False,lprd:False,rprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:2,bsrc:0,bdst:0,culm:0,dpts:2,wrdp:False,dith:2,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1457,x:33288,y:32719,varname:node_1457,prsc:2|emission-9525-OUT;n:type:ShaderForge.SFN_Tex2d,id:2230,x:31839,y:32565,varname:node_2230,prsc:2,tex:9202e64dabb3a0f4596d2a8821c4ef41,ntxv:0,isnm:False|UVIN-1171-OUT,TEX-383-TEX;n:type:ShaderForge.SFN_Color,id:5327,x:31989,y:32862,ptovrint:False,ptlb:Color_R,ptin:_Color_R,varname:node_5327,prsc:2,glob:False,c1:1,c2:0.3529412,c3:0,c4:1;n:type:ShaderForge.SFN_ValueProperty,id:8967,x:32672,y:32731,ptovrint:False,ptlb:Emission,ptin:_Emission,varname:node_8967,prsc:2,glob:False,v1:16;n:type:ShaderForge.SFN_VertexColor,id:3762,x:32672,y:33051,varname:node_3762,prsc:2;n:type:ShaderForge.SFN_Multiply,id:9525,x:33104,y:32800,varname:node_9525,prsc:2|A-4821-OUT,B-3762-RGB,C-8967-OUT,D-3762-A,E-3338-OUT;n:type:ShaderForge.SFN_Multiply,id:4821,x:32409,y:32792,varname:node_4821,prsc:2|A-2230-R,B-5327-RGB;n:type:ShaderForge.SFN_TexCoord,id:3783,x:30429,y:32537,varname:node_3783,prsc:2,uv:0;n:type:ShaderForge.SFN_Tex2dAsset,id:383,x:31187,y:32910,ptovrint:False,ptlb:Maintexture,ptin:_Maintexture,varname:node_383,tex:9202e64dabb3a0f4596d2a8821c4ef41,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:3358,x:31075,y:32472,varname:node_3358,prsc:2,tex:9202e64dabb3a0f4596d2a8821c4ef41,ntxv:0,isnm:False|UVIN-2662-OUT,TEX-383-TEX;n:type:ShaderForge.SFN_Multiply,id:6546,x:31465,y:32447,varname:node_6546,prsc:2|A-3358-G,B-3008-OUT;n:type:ShaderForge.SFN_TexCoord,id:8104,x:31350,y:32292,varname:node_8104,prsc:2,uv:0;n:type:ShaderForge.SFN_Add,id:1171,x:31626,y:32308,varname:node_1171,prsc:2|A-8104-UVOUT,B-6546-OUT;n:type:ShaderForge.SFN_Slider,id:3008,x:31045,y:32398,ptovrint:False,ptlb:Distortion_Strength,ptin:_Distortion_Strength,varname:node_3008,prsc:2,min:0,cur:0.5,max:4;n:type:ShaderForge.SFN_ValueProperty,id:6940,x:30216,y:32372,ptovrint:False,ptlb:Panner_X_Sspeed,ptin:_Panner_X_Sspeed,varname:node_6940,prsc:2,glob:False,v1:0;n:type:ShaderForge.SFN_ValueProperty,id:7479,x:30216,y:32468,ptovrint:False,ptlb:Panner_Y_Speed,ptin:_Panner_Y_Speed,varname:node_7479,prsc:2,glob:False,v1:0;n:type:ShaderForge.SFN_Append,id:3485,x:30404,y:32368,varname:node_3485,prsc:2|A-6940-OUT,B-7479-OUT;n:type:ShaderForge.SFN_Time,id:7412,x:30394,y:32247,varname:node_7412,prsc:2;n:type:ShaderForge.SFN_Multiply,id:8802,x:30576,y:32307,varname:node_8802,prsc:2|A-7412-T,B-3485-OUT;n:type:ShaderForge.SFN_Add,id:2662,x:30825,y:32490,varname:node_2662,prsc:2|A-8802-OUT,B-3783-UVOUT;n:type:ShaderForge.SFN_Slider,id:5905,x:32375,y:32554,ptovrint:False,ptlb:SoftParticle_Value,ptin:_SoftParticle_Value,varname:node_5073,prsc:2,min:0,cur:0,max:1;n:type:ShaderForge.SFN_SwitchProperty,id:3338,x:32932,y:32499,ptovrint:False,ptlb:Use_SoftPaticle,ptin:_Use_SoftPaticle,varname:node_287,prsc:2,on:False|A-1558-OUT,B-8182-OUT;n:type:ShaderForge.SFN_DepthBlend,id:8182,x:32725,y:32564,varname:node_8182,prsc:2|DIST-5905-OUT;n:type:ShaderForge.SFN_Vector1,id:1558,x:32725,y:32499,varname:node_1558,prsc:2,v1:1;proporder:5327-383-8967-3008-6940-7479-3338-5905;pass:END;sub:END;*/

Shader "FT/EmissionChannelDist" {
    Properties {
        _Color_R ("Color_R", Color) = (1,0.3529412,0,1)
        _Maintexture ("Maintexture", 2D) = "white" {}
        _Emission ("Emission", Float ) = 16
        _Distortion_Strength ("Distortion_Strength", Range(0, 4)) = 0.5
        _Panner_X_Sspeed ("Panner_X_Sspeed", Float ) = 0
        _Panner_Y_Speed ("Panner_Y_Speed", Float ) = 0
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
            uniform float4 _TimeEditor;
            uniform float4 _Color_R;
            uniform float _Emission;
            uniform sampler2D _Maintexture; uniform float4 _Maintexture_ST;
            uniform float _Distortion_Strength;
            uniform float _Panner_X_Sspeed;
            uniform float _Panner_Y_Speed;
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
                float4 node_7412 = _Time + _TimeEditor;
                float2 node_2662 = ((node_7412.g*float2(_Panner_X_Sspeed,_Panner_Y_Speed))+i.uv0);
                float4 node_3358 = tex2D(_Maintexture,TRANSFORM_TEX(node_2662, _Maintexture));
                float2 node_1171 = (i.uv0+(node_3358.g*_Distortion_Strength));
                float4 node_2230 = tex2D(_Maintexture,TRANSFORM_TEX(node_1171, _Maintexture));
                float3 emissive = ((node_2230.r*_Color_R.rgb)*i.vertexColor.rgb*_Emission*i.vertexColor.a*lerp( 1.0, saturate((sceneZ-partZ)/_SoftParticle_Value), _Use_SoftPaticle ));
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
