
Shader "KOL/Charactor Scale Mobile"
{
	Properties
	{
		//TOONY COLORS
		_Color ("Color", Color) = (0.5,0.5,0.5,1.0)
		_HColor("Highlight Color", Color) = (0.6,0.6,0.6,1.0)
		_SColor ("Shadow Color", Color) = (0.3,0.3,0.3,1.0)
		
		_IllumColor("Illumination Color", Color) = (1,1,1,1.0)

		//DIFFUSE
		_MainTex ("Main Texture (RGB)", 2D) = "white" {}		

		_lightDir ("Light Direction",Vector) = (1,1,1,1.0)
		//_lightColor("Light Color",Color) = (1,1,1,1.0)
		
		//TOONY COLORS RAMP
		_RampThreshold ("#RAMPF# Ramp Threshold", Range(0,1)) = 0.5
		_RampSmooth ("#RAMPF# Ramp Smoothing", Range(0.001,1)) = 0.1
		
		//SPECULAR
		//_SpecColor ("#SPEC# Specular Color", Color) = (0.5, 0.5, 0.5, 1)
		_Shininess ("#SPEC# Shininess", Range(0,2)) = 0.5
		_SpecSmooth ("#SPECT# Smoothness", Range(0,1)) = 0.05
		
		//RIM LIGHT
		_RimColor ("#RIM# Rim Color", Color) = (0.8,0.8,0.8,0.6)
		_RimMin ("#RIM# Rim Min", Range(0,1)) = 0.5
		_RimMax ("#RIM# Rim Max", Range(0,1)) = 1.0
		
		
		//OUTLINE
		_OutlineColor ("#OUTLINE# Outline Color", Color) = (0.2, 0.2, 0.2, 1.0)
		_Outline ("#OUTLINE# Outline Width", Float) = 1
		
		//Outline Textured
		_TexLod ("#OUTLINETEX# Texture LOD", Range(0,10)) = 5
		
		//ZSmooth
		_ZSmooth ("#OUTLINEZ# Z Correction", Range(-3.0,3.0)) = -0.5
		
		//Z Offset
		_Offset1 ("#OUTLINEZ# Z Offset 1", Float) = 0
		_Offset2 ("#OUTLINEZ# Z Offset 2", Float) = 0


		_Scale("Scale",float) = 1
		
	}
	
	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		
		CGPROGRAM
		
		//#include "../../Shaders 2.0/Include/TCP2_Include.cginc"
		#pragma vertex vert
		#pragma surface surf ToonyColorsSpec

		#pragma target 3.0
		#pragma glsl
		
		#pragma multi_compile TCP2_SPEC_TOON
		
		//== == == == == == == == == == == == == == == == == == == == == == == == == == == == == == == =  = 
		// VARIABLES
		
		fixed4 _Color;
		sampler2D _MainTex;
		fixed4 _IllumColor;
		float4 _lightDir;
		fixed4 _lightColor;
		
		fixed _Shininess;
		fixed4 _RimColor;
		fixed _RimMin;
		fixed _RimMax;
		float4 _RimDir;

#ifndef TOONYCOLORS_INCLUDED
#define TOONYCOLORS_INCLUDED

#if TCP2_RAMPTEXT
		//Lighting Ramp
		sampler2D _Ramp;
#else
		float _RampThreshold;
		float _RampSmooth;
#endif

#if TCP2_SPEC_TOON
		fixed _SpecSmooth;
#endif

		//Highlight/Shadow Colors
		fixed4 _HColor;
		fixed4 _SColor;

#endif

		float _Scale;
		
		struct Input
		{
			half2 uv_MainTex;
			float3 viewDir;
			float3 worldNormal;
		};
		
		void vert(inout appdata_full v) {
			if (v.color.a != 0 && _Scale != 0)
			{
				v.vertex.xyz = (1 - v.color.a * _Scale)  * v.vertex.xyz;
			}
		}
		//== == == == == == == == == == == == == == == == == == == == == == == == == == == == == == == =  = 
		// SURFACE FUNCTION

		void surf (Input IN, inout SurfaceOutput o)
		{
			fixed4 mainTex = tex2D(_MainTex, IN.uv_MainTex);
			
			o.Albedo = mainTex.rgb * _Color.rgb;
			o.Alpha = mainTex.a * _Color.a;
			
			//Specular
			o.Gloss = 1;
			o.Specular = _Shininess;
			//Rim
			float3 viewDir = normalize(IN.viewDir);
			half rim = 1.0f - saturate( dot(viewDir, o.Normal) );
			rim = smoothstep(_RimMin, _RimMax, rim);
			o.Emission +=  (_RimColor.rgb * rim) * _RimColor.a;
			o.Emission +=  mainTex.rgb * (mainTex.a * _IllumColor.a) * _IllumColor.rgb;
			
		}

		inline half4 LightingToonyColorsSpec(SurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
		{
			_lightColor = _HColor;
			_SpecColor = _HColor;

			s.Normal = normalize(s.Normal);
			fixed ndl = max(0, dot(s.Normal, _lightDir) * 0.5 + 0.5);
#if TCP2_RAMPTEXT
			fixed3 ramp = tex2D(_Ramp, fixed2(ndl, ndl));
#else
			fixed3 ramp = smoothstep(_RampThreshold - _RampSmooth * 0.5, _RampThreshold + _RampSmooth * 0.5, ndl);
#endif
#if !(POINT) && !(SPOT)
			ramp *= atten;
#endif
			_SColor = lerp(_HColor, _SColor, _SColor.a);	//Shadows intensity through alpha
			ramp = lerp(_SColor.rgb, _HColor.rgb, ramp);
			//Specular
			half3 h = normalize(_lightDir + viewDir);
			float ndh = max(0, dot(s.Normal, h));
			float spec = pow(ndh, s.Specular * 128.0) * s.Gloss * 2.0;
#if TCP2_SPEC_TOON
			spec = smoothstep(0.5 - _SpecSmooth * 0.5, 0.5 + _SpecSmooth * 0.5, spec);
#endif
			spec *= atten;
			fixed4 c;
			c.rgb = s.Albedo  * ramp;
#if (POINT || SPOT)
			c.rgb *= atten;
#endif
			c.rgb += _lightColor.rgb * _SpecColor.rgb * spec;
			c.a = s.Alpha + _lightColor.a * _SpecColor.a * spec;

			return c;
		}
		
		ENDCG
		
		//Outlines
		UsePass "Hidden/Toony Colors Pro 2/Outline Only Scale (Shader Model 2)/OUTLINE"
	}
	
	Fallback "Diffuse"
}
