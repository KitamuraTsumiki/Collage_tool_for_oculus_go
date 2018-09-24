// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

Shader "Custom/Sketch" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_DrawingPattern1 ("Drawing Pattern 1", 2D) = "white" {}
		_Contrast ("Contrast", Range(0, 2)) = 1
		_Brightness ("Brightness", Range(0, 2)) = 1
		_StrokeWidth ("Stroke Width", Range(0, 0.8)) = 0.1
		_Alpha("Alpha Mask", 2D) = "white"{}
	}
	SubShader {
		Tags { "RenderType"="Transparent" }
		Cull Off
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows alpha:fade

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _DrawingPattern1;
		sampler2D _Alpha;
		half _Contrast;
		half _Brightness;
		half _StrokeWidth;

		struct Input {
			float2 uv_MainTex;
			float2 uv_DrawingPattern1;
			float3 viewDir;
		};

		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {

			// outline
			half dotp = dot(IN.viewDir, o.Normal);
			dotp = dotp < _StrokeWidth ? float3(0,0,0): 1;
			fixed3 outline = fixed3(dotp, dotp, dotp);
			
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);

			fixed baseGrayScale = pow((c.r + c.g + c.b) / 3, _Contrast) * _Brightness;
			baseGrayScale = baseGrayScale < 0.2 ? 0.1 : baseGrayScale < 0.5 ? 0.4 : baseGrayScale < 0.8 ? 0.7 : 1;

			fixed baseShading = dot(IN.viewDir, o.Normal);
			baseShading = baseShading  < 0.2 ? 0.2 : baseShading < 0.5 ? 0.5 : baseShading < 0.8 ? 0.8 : 1;

			fixed Shading = baseShading * baseGrayScale;

			o.Albedo = fixed3(Shading, Shading, Shading) * outline;
			o.Alpha = tex2D(_Alpha, IN.uv_MainTex);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
