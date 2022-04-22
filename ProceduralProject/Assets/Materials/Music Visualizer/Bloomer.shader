Shader "TylersShaders/Bloomer"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	_Vignette("Vignette", 2D) = "black"{}
	_Bloom("Bloom", float) = 0.5
	}
		SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 uv_MainTex : TEXCOORD1;
				float4 uv_Vignette : TEXCOORD2;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			sampler2D _MainTex;
			sampler2D _Vignette;
			float _Bloom;

			fixed4 frag(v2f i) : SV_Target
			{
				float2 uv = i.uv;
				fixed4 vig = tex2D(_Vignette, uv);
				fixed4 col = tex2D(_MainTex, uv);
			// just invert the colors
			return vig * _Bloom + col;
		}
		ENDCG
	}
	}
}
