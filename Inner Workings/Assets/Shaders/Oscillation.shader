/*hausmaus unity forums December/9/2011*/

Shader "Custom/Oscillation" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_PhaseOffset("Phase Offset", Range(0,1)) = 0
		_Speed ("Speed", Range(0.1, 10)) = 1
		_Depth ("Depth", Range(0.01, 1)) = 0.2
		_Scale ("Scale", Range(0.1, 20)) = 10
	}
	SubShader {
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off
		LOD 200
	
		CGPROGRAM

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0
		#pragma vertex vert
		#pragma surface surf Lambert vertex:vert alpha
		#include "UnityCG.cginc"
	
		float _Displacement;
		float _TimeScale;
		float4 _Color;

		float _PhaseOffset;
		float _Speed;
		float _Depth;
		float _Scale;

		struct Input {
			float4 pos : SV_POSITION;
			float3 normal;
		};

		void vert(inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);
		
			// Obtain tangent space rotation matrix
			float3 binormal = cross(v.normal, v.tangent.xyz) * v.tangent.w;
			float3x3 rotation = transpose(float3x3(v.tangent.xyz, binormal, v.normal));

			// Create two sample vectors (small +x/+y deflections from +z), put them in tangent space, normalize them, and halve the result.
			// This is equivalent to sampling neighboring vertex data since we're on a unit sphere.
			float3 v1 = normalize(mul(rotation, float3(0.1, 0, 1))) * 0.5;
			float3 v2 = normalize(mul(rotation, float3(0, 0.1, 1))) * 0.5;

			// Some animation values
			float phase = _PhaseOffset * 3.14 * 2;
			float speed = _Time.y * _Speed;

			// Modify the real vertex and two theoretical samples by the distortion algorithm (here a simple sine wave on XZ)
			v.vertex.x += sin(phase + speed + (v.vertex.z * _Scale)) * _Depth;
			v1.x += sin(phase + speed + (v1.z * _Scale)) * _Depth;
			v2.x += sin(phase + speed + (v2.z * _Scale)) * _Depth;

			// Modify the real vertex and two theoretical samples by the distortion algorithm (here a simple sine wave on XZ)
			v.vertex.y += sin(phase + 0.5 + speed + (v.vertex.x * _Scale)) * _Depth;
			v1.y += sin(phase + 0.5 + speed + (v1.x * _Scale)) * _Depth;
			v2.y += sin(phase + 0.5 + speed + (v2.x * _Scale)) * _Depth;

			// Modify the real vertex and two theoretical samples by the distortion algorithm (here a simple sine wave on XZ)
			v.vertex.z += sin(phase + 1 + speed + (v.vertex.y * _Scale)) * _Depth;
			v1.z += sin(phase + 1 + speed + (v1.y * _Scale)) * _Depth;
			v2.z += sin(phase + 1 + speed + (v2.y * _Scale)) * _Depth;

			// Take the cross product of the sample-original positions, resulting in a dynamic normal
			float3 vn = cross(v2 - v.vertex.xyz, v1 - v.vertex.xyz);

			// Normalize
			v.normal = normalize(vn);

			// OPTIONAL pass this out to a custom value.  Uncomment the showNormals finalcolor profile option above to see the result
			//o.debugColor = (v.normal.xyz * 0.5) + 0.5;
			
		
		}

		void surf (Input IN, inout SurfaceOutput o) {
			//half4 tex = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = _Color.rgb;
			o.Alpha = _Color.a;
			o.Specular = 0;
			o.Gloss = 0;
			o.Emission = _Color.rgb;
			//o.Smoothness = 0;
			//o.Metallic = 0;

		}
		ENDCG
	}
	FallBack "Diffuse"
}
