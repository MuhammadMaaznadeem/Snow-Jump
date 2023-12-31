Shader "PLAYER TWO/Platformer Project/Water"
{
	Properties
	{
		_Color ("Color", Color) = (1, 1, 1, 0.5)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_NoiseTex ("Noise", 2D) = "white" {}
		_Distortion ("Distortion", Range(0, 1)) = 0.2
		_SpeedX ("Speed X", Float) = 5
		_SpeedY ("Speed Y", Float) = 5
	}

	SubShader
	{
		Tags { "RenderType" = "Transparent" "IgnoreProjector"="True" "Queue" = "Transparent" }
		LOD 100

		CGPROGRAM

		#pragma surface surf Lambert alpha

		sampler2D _MainTex;
		sampler2D _NoiseTex;

		half4 _Color;

		half _SpeedX;
		half _SpeedY;
		half _Distortion;

		struct Input
		{
			float2 uv_MainTex;
		};

		float4 _NoiseTex_ST;

		void surf (Input IN, inout SurfaceOutput o)
		{
			half speedX = _Time.x *_SpeedX;
			half speedY = _Time.x *_SpeedY;
			fixed2 noiseUv = TRANSFORM_TEX(IN.uv_MainTex, _NoiseTex);
			fixed4 noise = tex2D(_NoiseTex, half2(noiseUv.x + speedX, noiseUv.y + speedY));
			half offset = noise.r - 0.5;
			offset = offset * _Distortion;
			fixed4 col = tex2D(_MainTex, half2(IN.uv_MainTex.x + offset, IN.uv_MainTex.y + offset)) * _Color;
			o.Albedo = col.rgb;
			o.Emission = col.rgb;
			o.Alpha = col.a;
		}

		ENDCG
	}

	FallBack "Diffuse"
}
