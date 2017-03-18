// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Hidden/Fresnel"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Scale ("Scale", Range(0.1, 5.0)) = 1.0
		_Power ("Power", Range(0.1, 1.0)) = 1.0
			_Color("Reflection", Color) = (0.5, 0.5, 0.5, 1.0)
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
// Upgrade NOTE: excluded shader from DX11; has structs without semantics (struct v2f members R)
#pragma exclude_renderers d3d11
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float R;
				float4 color : COLOR;
			};

			float _Scale;
			float _Power;
			float4 _MainTex_ST;
			float4 _Color;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;

				float3 posWorld = mul(unity_ObjectToWorld, v.vertex).xyz;
				float3 normWorld = normalize(mul(v.normal, (float3x3) unity_ObjectToWorld));

				float3 I = normalize(posWorld - _WorldSpaceCameraPos.xyz);
				o.R = _Scale * pow(1.0 + dot(I, normWorld), _Power);

				return o;
			}
			
			sampler2D _MainTex;

			fixed4 frag (v2f i) : COLOR
			{
				float4 col = tex2D(_MainTex, i.uv.xy * _MainTex_ST.xy + _MainTex_ST.zw);
				return lerp(col,_Color, i.R);
			}
			ENDCG
		}
	}
}
