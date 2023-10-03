Shader "PLAYER TWO/Platformer Project/Soft Lit"
{
	Properties
	{
		_Color ("Color", Color) = (1, 1, 1, 1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
	}

	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100

		CGPROGRAM

		#pragma surface surf SoftLit

		sampler2D _MainTex;

		half4 _Color;

		struct Input
		{
			float2 uv_MainTex;
		};

		half4 LightingSoftLit (SurfaceOutput s, half3 lightDir, half atten)
		{
			half NdotL = dot (s.Normal, lightDir);
			half diff = NdotL * 0.5 + 0.5;
			half4 c;
			c.rgb = s.Albedo * _LightColor0.rgb * (diff * atten);
			c.a = s.Alpha;
			return c;
		}

		void surf (Input IN, inout SurfaceOutput o)
		{
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
		}

		ENDCG
	}

	FallBack "Diffuse"
}
