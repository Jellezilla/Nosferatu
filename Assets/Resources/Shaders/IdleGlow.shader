Shader "Custom/IdleGlow" {
	Properties {
		_ContactPoint("Contact", Vector) = (0,0,0,0)
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_GlowColor ("Glow Color Tint", Color) = (1,1,1,1)
		_GlowPower ("Glow Power", Range(0.5, 6)) = 3.0
		_NormalMap ("Normal Map", 2D) = "normal" {}
		
	}
		/// GRAB VECTOR3 LOCAL SPACE multiply by point
		/// I TRY and I FAIL T_T, Alex
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		float4 _GlowColor;
		float _GlowPower;
		sampler2D _NormalMap;
		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		fixed3 _ContactPoint;

		struct Input {
			float2 uv_MainTex;
			float2 uv_NormalMap;
			float3 viewDir;
		};



		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Applying normal map
			o.Normal = UnpackNormal(tex2D(_NormalMap, IN.uv_NormalMap));
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
			// Based on view direction affect the normals of the surface
			half glowRim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
			// Pass Emission data by multiplying the glow color with the normalized glowRim ^ glow power
			o.Emission = _GlowColor.rgb * pow(glowRim, _GlowPower);


		}
		ENDCG
	}
	FallBack "Diffuse"
}
