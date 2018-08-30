

 Shader "SeeThroughObjects3" 
 {
    Properties 
	{
        _TintColor ("Color", Color) = (0,0.2551723,1,1)
        _Color_copy ("Color_copy", Color) = (0.07843139,0.2293441,0.7843137,1)
        _node_5761 ("node_5761", 2D) = "white" {}
        _speed_copy_copy ("speed_copy_copy", Float ) = 1.3
        _Distort ("Distort", Float ) = 2
        _speed_copy ("speed_copy", Float ) = 1.3
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
		_MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
    }
    SubShader 
	{
        Tags 
		{
			"Queue"="Overlay+1" 
            "RenderType"="TransparentCutout"
			
        }
       /* Pass 
		{

			ZWrite Off
            ZTest Greater
			SetTexture [_MainTex] {combine texture}
        }*/
		Pass 
        {
             ZTest Less  
			 Name "FORWARD"
            Tags 
			{
                "LightMode"="ForwardBase"
            }
            Blend One One
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _TintColor;
            uniform float4 _Color_copy;
            uniform sampler2D _node_5761; uniform float4 _node_5761_ST;
            uniform float _speed_copy;
            uniform float _speed_copy_copy;
            uniform float _Distort;
            struct VertexInput
			{
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput
			{
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
                UNITY_FOG_COORDS(1)
            };
            VertexOutput vert (VertexInput v)
			{
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR 
			{
                float4 node_9400 = _Time;
                float2 node_8149 = float2(i.uv0.r,(i.uv0.g+(node_9400.b*_speed_copy)));
                float4 node_7394 = tex2D(_node_5761,TRANSFORM_TEX(node_8149, _node_5761));
                float2 node_5823 = (float2(i.uv0.r,(i.uv0.g+(node_9400.a*_speed_copy_copy)))+(_Distort*node_7394.b));
                float4 node_5095 = tex2D(_node_5761,TRANSFORM_TEX(node_5823, _node_5761));
                float3 node_3130 = (node_5095.r+node_5095.rgb+node_5095.b);
                float4 node_2264 = _Time;
                float node_8313 = ((node_3130*10.0+-1.0)+saturate(( (1.0 - distance(i.uv0.g,(2.0*sin(node_2264.b)))) > 0.5 ? (_Color_copy.rgb + 2.0*(1.0 - distance(i.uv0.g,(2.0*sin(node_2264.b)))) -1.0) : (_Color_copy.rgb + 2.0*((1.0 - distance(i.uv0.g,(2.0*sin(node_2264.b))))-0.5))))).r;
                clip(node_8313 - 0.5);
////// Lighting:
////// Emissive:
                float3 emissive = (node_8313*i.vertexColor.rgb*_TintColor.rgb*2.0);
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG_COLOR(i.fogCoord, finalRGBA, fixed4(0,0,0,1));
                return finalRGBA;
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}

 