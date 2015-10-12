Shader "Edge highlight"
{
	Properties
	{
		_Color ("Color", Color) = (0, 0, 0, 1)
		_Strength ("Strength", Float) = 0.0
		_MainTex ("Font Texture", 2D) = "white" {}
	}

	SubShader
	{
		Pass
		{
			Tags { "RenderType"="Opaque" }
			ZTest Always
			LOD 200
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			float4 _Color;
			float _Strength;
			sampler2D _MainTex;

			struct VertexOutput
			{
				float4 pos: SV_POSITION;
				float2 uv: TEXCOORD0;
			};

			float4 _MainTex_ST;

			VertexOutput vert(appdata_base v)
			{
				VertexOutput o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				return o;
			}

			half4 frag(VertexOutput i): COLOR
			{
				half4 texColor = tex2D(_MainTex, i.uv);
				half4 red = _Strength * abs(i.uv.x - 0.5) * _Color; //half4(_Strength * abs(i.uv.x - 0.5), 0, 0, 1);
				return texColor + red;
			}

			ENDCG
		}
	}
	FallBack "Diffuse"
}
