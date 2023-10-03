Shader "PLAYER TWO/Platformer Project/Soft Lit (Cutout)"
{
	Properties
	{
		_Color ("Color", Color) = (1, 1, 1, 1)
		_MainTex ("Albedo (RGBA)", 2D) = "white" {}
		_Cutoff  ("Cutoff", Range(0, 1)) = 0.5
	}

	SubShader
	{
		Tags { "Queue" = "AlphaTest" "RenderType" = "TransparentCutout" }
		LOD 100

		CGPROGRAM

		#pragma surface surf SoftLit alphatest:_Cutoff

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
			o.Alpha = c.a;
		}

		ENDCG
	}

	FallBack "Diffuse"
}
