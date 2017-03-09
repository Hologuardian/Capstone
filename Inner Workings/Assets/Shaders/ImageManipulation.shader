Shader "Hidden/ImageManipulation"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_HatchTex("Hatch Tex", 2D) = "white" {}


		
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always
		//Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }

		

		Pass
		{
			CGPROGRAM
			#pragma vertex vert alpha
			#pragma fragment frag Lambert alpha
		

			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.vertex.y += sin(_Time[3] / 50);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _HatchTex;
			
			
			
			fixed4 alphaBlend(fixed4 dst, fixed4 src)
			{
				fixed4 result = fixed4(0, 0, 0, 0);
				result.a = src.a + dst.a * (1 - src.a);

				if (result.a != 0)
				{
					result.rgb = (src.rgb * src.a + dst.rgb * dst.a * (1 - src.a)) / result.a; 
				}
				return result;
			}

			float2 texLookUp(float2 texcoord, float row, float col)
			{
				row = row % 3;
				col = col % 3;

				return (texcoord / 3) + float2(row / 3, col / 3);
			}

			fixed4 frag(v2f i) : SV_Target
			{

				float texIndex = floor(_Time.y * 10 % 9);
				float row = 1 + texIndex % 2;
				float col = floor(texIndex / 3);

				fixed4 original = tex2D(_MainTex, i.uv);
				fixed4 output = (0, 0, 0, 0); //full black???
				fixed4 hatch; // = tex2D(_HatchTex, i.uv * 2); //multiplying makes lines more dense

				if (original.r > 0.3)//test the a, not the r when  proper image is ready the .1 determines the cutoff of how much to draw
				{
					hatch = tex2D(_HatchTex, texLookUp(i.uv * 21, row, col)); //Multiplying determines the frequency of the lines
					output = alphaBlend(output, hatch * 0.8);//higher value on the .8 determines the intensity of the hatch
				}

				if (original.r > 0.5)
				{
					hatch = tex2D(_HatchTex, texLookUp(i.uv * 27, row + 1, col));
					output = alphaBlend(output, hatch * 1.2);
				}

				if (original.r > 0.7)
				{
					hatch = tex2D(_HatchTex, texLookUp(i.uv * 30, row, col + 1));
					output = alphaBlend(output, hatch * 1.5);
				}

				return output;
			}
			ENDCG
		}
	}
}
