// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.18 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.18;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,culm:0,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:2,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1457,x:33288,y:32719,varname:node_1457,prsc:2|emission-9525-OUT,custl-1039-OUT;n:type:ShaderForge.SFN_Tex2d,id:2230,x:32006,y:32827,ptovrint:False,ptlb:Texture,ptin:_Texture,varname:node_2230,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:2,isnm:False;n:type:ShaderForge.SFN_Color,id:5327,x:32242,y:32573,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_5327,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:316,x:32493,y:32832,varname:node_316,prsc:2|A-9569-OUT,B-5327-RGB,C-8967-OUT;n:type:ShaderForge.SFN_ValueProperty,id:8967,x:32493,y:32700,ptovrint:False,ptlb:Emission,ptin:_Emission,varname:node_8967,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_VertexColor,id:3762,x:32637,y:32874,varname:node_3762,prsc:2;n:type:ShaderForge.SFN_Multiply,id:9525,x:32857,y:32793,varname:node_9525,prsc:2|A-316-OUT,B-3762-RGB,C-3762-A,D-287-OUT;n:type:ShaderForge.SFN_Lerp,id:9569,x:32251,y:32916,varname:node_9569,prsc:2|A-8405-OUT,B-2230-RGB,T-2230-A;n:type:ShaderForge.SFN_Vector1,id:8405,x:31995,y:33110,varname:node_8405,prsc:2,v1:0;n:type:ShaderForge.SFN_Slider,id:5073,x:32493,y:32516,ptovrint:False,ptlb:SoftParticle_Value,ptin:_SoftParticle_Value,varname:node_5073,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_SwitchProperty,id:287,x:33050,y:32461,ptovrint:False,ptlb:Use_SoftPaticle,ptin:_Use_SoftPaticle,varname:node_287,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False|A-7306-OUT,B-6100-OUT;n:type:ShaderForge.SFN_DepthBlend,id:6100,x:32843,y:32526,varname:node_6100,prsc:2|DIST-5073-OUT;n:type:ShaderForge.SFN_Vector1,id:7306,x:32843,y:32461,varname:node_7306,prsc:2,v1:1;n:type:ShaderForge.SFN_Fresnel,id:3723,x:32367,y:33283,varname:node_3723,prsc:2|EXP-7038-OUT;n:type:ShaderForge.SFN_ValueProperty,id:7038,x:32145,y:33380,ptovrint:False,ptlb:Fresnel_Range,ptin:_Fresnel_Range,varname:node_3614,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:5;n:type:ShaderForge.SFN_Color,id:182,x:32346,y:33425,ptovrint:False,ptlb:FresnelColor,ptin:_FresnelColor,varname:node_7002,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.4411765,c2:0.6794984,c3:1,c4:1;n:type:ShaderForge.SFN_ValueProperty,id:5258,x:32401,y:33652,ptovrint:False,ptlb:Fresnel_Emission,ptin:_Fresnel_Emission,varname:node_2338,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Vector1,id:7750,x:32401,y:33578,varname:node_7750,prsc:2,v1:0;n:type:ShaderForge.SFN_SwitchProperty,id:6083,x:32709,y:33505,ptovrint:False,ptlb:Use_Fresnel,ptin:_Use_Fresnel,varname:node_7155,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:True|A-7750-OUT,B-5258-OUT;n:type:ShaderForge.SFN_Multiply,id:1039,x:32906,y:33396,varname:node_1039,prsc:2|A-3723-OUT,B-182-RGB,C-6083-OUT,D-3762-A;proporder:5327-2230-8967-6083-182-5258-7038-287-5073;pass:END;sub:END;*/

Shader "FT/Emission_Fresnel" {
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _Texture ("Texture", 2D) = "black" {}
        _Emission ("Emission", Float ) = 1
        [MaterialToggle] _Use_Fresnel ("Use_Fresnel", Float ) = 1
        _FresnelColor ("FresnelColor", Color) = (0.4411765,0.6794984,1,1)
        _Fresnel_Emission ("Fresnel_Emission", Float ) = 1
        _Fresnel_Range ("Fresnel_Range", Float ) = 5
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
            Name "FORWARD"
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
            #pragma multi_compile_fog
            #pragma exclude_renderers xbox360 ps3 
            #pragma target 3.0
            uniform sampler2D _CameraDepthTexture;
            uniform sampler2D _Texture; uniform float4 _Texture_ST;
            uniform float4 _Color;
            uniform float _Emission;
            uniform float _SoftParticle_Value;
            uniform fixed _Use_SoftPaticle;
            uniform float _Fresnel_Range;
            uniform float4 _FresnelColor;
            uniform float _Fresnel_Emission;
            uniform fixed _Use_Fresnel;
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
                UNITY_FOG_COORDS(4)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex);
                UNITY_TRANSFER_FOG(o,o.pos);
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float sceneZ = max(0,LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)))) - _ProjectionParams.g);
                float partZ = max(0,i.projPos.z - _ProjectionParams.g);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
////// Lighting:
////// Emissive:
                float node_8405 = 0.0;
                float4 _Texture_var = tex2D(_Texture,TRANSFORM_TEX(i.uv0, _Texture));
                float3 emissive = ((lerp(float3(node_8405,node_8405,node_8405),_Texture_var.rgb,_Texture_var.a)*_Color.rgb*_Emission)*i.vertexColor.rgb*i.vertexColor.a*lerp( 1.0, saturate((sceneZ-partZ)/_SoftParticle_Value), _Use_SoftPaticle ));
                float3 finalColor = emissive + (pow(1.0-max(0,dot(normalDirection, viewDirection)),_Fresnel_Range)*_FresnelColor.rgb*lerp( 0.0, _Fresnel_Emission, _Use_Fresnel )*i.vertexColor.a);
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
