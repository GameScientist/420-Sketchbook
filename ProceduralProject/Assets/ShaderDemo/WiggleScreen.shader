Shader "TylersShaders/WiggleScreen"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_NoiseTex("Noise", 2D) = "black" {}
	}
		SubShader
	{
		Cull off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 uv_MainTex : TEXCOORD1;
				float4 uv_NoiseTex : TEXCOORD2;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			float4 _MainTex_ST;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			sampler2D _MainTex;
			sampler2D _NoiseTex;

			fixed4 frag(v2f i) : SV_Target
			{
				float2 uv = i.uv;

				fixed4 colorNoise = tex2D(_NoiseTex, uv + float2(0, - _Time.x));

				uv += colorNoise.rg / 10;

				fixed4 col = tex2D(_MainTex, uv);
				//TODO: Wiggle the screen.
				return col;
			}
			ENDCG
		}
	}
}
