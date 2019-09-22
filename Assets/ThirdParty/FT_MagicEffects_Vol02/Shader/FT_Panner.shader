// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.05 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.05;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:0,uamb:True,mssp:True,lmpd:False,lprd:False,rprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:2,bsrc:0,bdst:0,culm:0,dpts:2,wrdp:False,dith:2,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:8272,x:33781,y:32753,varname:node_8272,prsc:2|emission-3506-OUT;n:type:ShaderForge.SFN_Tex2d,id:8575,x:32399,y:32980,ptovrint:False,ptlb:Texture,ptin:_Texture,varname:node_8575,prsc:2,ntxv:0,isnm:False|UVIN-3629-OUT;n:type:ShaderForge.SFN_Lerp,id:5816,x:32598,y:32980,varname:node_5816,prsc:2|A-1338-OUT,B-8575-RGB,T-8575-A;n:type:ShaderForge.SFN_Vector1,id:1338,x:32388,y:32887,varname:node_1338,prsc:2,v1:0;n:type:ShaderForge.SFN_Time,id:3530,x:31714,y:33309,varname:node_3530,prsc:2;n:type:ShaderForge.SFN_Multiply,id:2466,x:32007,y:33201,varname:node_2466,prsc:2|A-1875-OUT,B-3530-T;n:type:ShaderForge.SFN_ValueProperty,id:5788,x:31594,y:33119,ptovrint:False,ptlb:X_speed,ptin:_X_speed,varname:node_5788,prsc:2,glob:False,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:6571,x:31594,y:33222,ptovrint:False,ptlb:Y_speed,ptin:_Y_speed,varname:node_6571,prsc:2,glob:False,v1:2;n:type:ShaderForge.SFN_Append,id:1875,x:31825,y:33156,varname:node_1875,prsc:2|A-5788-OUT,B-6571-OUT;n:type:ShaderForge.SFN_TexCoord,id:3955,x:32013,y:33015,varname:node_3955,prsc:2,uv:0;n:type:ShaderForge.SFN_Add,id:3629,x:32200,y:33071,varname:node_3629,prsc:2|A-3955-UVOUT,B-2466-OUT;n:type:ShaderForge.SFN_Multiply,id:4899,x:32915,y:32969,varname:node_4899,prsc:2|A-5816-OUT,B-2145-RGB,C-9543-RGB,D-6383-OUT,E-2145-A;n:type:ShaderForge.SFN_ValueProperty,id:6383,x:32596,y:33429,ptovrint:False,ptlb:Emission,ptin:_Emission,varname:node_6383,prsc:2,glob:False,v1:1;n:type:ShaderForge.SFN_VertexColor,id:2145,x:32596,y:33109,varname:node_2145,prsc:2;n:type:ShaderForge.SFN_Color,id:9543,x:32596,y:33259,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_9543,prsc:2,glob:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Fresnel,id:2301,x:32586,y:32709,varname:node_2301,prsc:2|EXP-8082-OUT;n:type:ShaderForge.SFN_Multiply,id:3506,x:33579,y:32850,varname:node_3506,prsc:2|A-7994-OUT,B-4899-OUT,C-657-OUT;n:type:ShaderForge.SFN_OneMinus,id:5918,x:32743,y:32709,varname:node_5918,prsc:2|IN-2301-OUT;n:type:ShaderForge.SFN_Vector1,id:8082,x:32351,y:32732,varname:node_8082,prsc:2,v1:1;n:type:ShaderForge.SFN_Power,id:4684,x:32915,y:32709,varname:node_4684,prsc:2|VAL-5918-OUT,EXP-3093-OUT;n:type:ShaderForge.SFN_ValueProperty,id:3093,x:32694,y:32886,ptovrint:False,ptlb:Fresnel_Strength,ptin:_Fresnel_Strength,varname:node_3093,prsc:2,glob:False,v1:5;n:type:ShaderForge.SFN_SwitchProperty,id:7994,x:33163,y:32750,ptovrint:False,ptlb:Use_Fresnel,ptin:_Use_Fresnel,varname:node_7994,prsc:2,on:True|A-5455-OUT,B-4684-OUT;n:type:ShaderForge.SFN_Vector1,id:5455,x:32979,y:32637,varname:node_5455,prsc:2,v1:1;n:type:ShaderForge.SFN_DepthBlend,id:9240,x:33153,y:33113,varname:node_9240,prsc:2|DIST-6241-OUT;n:type:ShaderForge.SFN_Slider,id:6241,x:32837,y:33216,ptovrint:False,ptlb:SoftParticle_Value,ptin:_SoftParticle_Value,varname:node_6241,prsc:2,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Vector1,id:9234,x:33153,y:33326,varname:node_9234,prsc:2,v1:1;n:type:ShaderForge.SFN_SwitchProperty,id:657,x:33352,y:33071,ptovrint:False,ptlb:Use_SoftParticle,ptin:_Use_SoftParticle,varname:node_657,prsc:2,on:False|A-9234-OUT,B-9240-OUT;proporder:9543-8575-6383-5788-6571-7994-3093-657-6241;pass:END;sub:END;*/

Shader "FT/Panner" {
    Properties {
        _Color ("Color", Color) = (0.5,0.5,0.5,1)
        _Texture ("Texture", 2D) = "white" {}
        _Emission ("Emission", Float ) = 1
        _X_speed ("X_speed", Float ) = 1
        _Y_speed ("Y_speed", Float ) = 2
        [MaterialToggle] _Use_Fresnel ("Use_Fresnel", Float ) = 0
        _Fresnel_Strength ("Fresnel_Strength", Float ) = 5
        [MaterialToggle] _Use_SoftParticle ("Use_SoftParticle", Float ) = 1
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
            uniform sampler2D _Texture; uniform float4 _Texture_ST;
            uniform float _X_speed;
            uniform float _Y_speed;
            uniform float _Emission;
            uniform float4 _Color;
            uniform float _Fresnel_Strength;
            uniform fixed _Use_Fresnel;
            uniform float _SoftParticle_Value;
            uniform fixed _Use_SoftParticle;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float4 vertexColor : COLOR;
                float4 projPos : TEXCOORD3;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.normalDir = mul(unity_ObjectToWorld, float4(v.normal,0)).xyz;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex);
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float sceneZ = max(0,LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)))) - _ProjectionParams.g);
                float partZ = max(0,i.projPos.z - _ProjectionParams.g);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
////// Lighting:
////// Emissive:
                float node_1338 = 0.0;
                float4 node_3530 = _Time + _TimeEditor;
                float2 node_3629 = (i.uv0+(float2(_X_speed,_Y_speed)*node_3530.g));
                float4 _Texture_var = tex2D(_Texture,TRANSFORM_TEX(node_3629, _Texture));
                float3 emissive = (lerp( 1.0, pow((1.0 - pow(1.0-max(0,dot(normalDirection, viewDirection)),1.0)),_Fresnel_Strength), _Use_Fresnel )*(lerp(float3(node_1338,node_1338,node_1338),_Texture_var.rgb,_Texture_var.a)*i.vertexColor.rgb*_Color.rgb*_Emission*i.vertexColor.a)*lerp( 1.0, saturate((sceneZ-partZ)/_SoftParticle_Value), _Use_SoftParticle ));
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
