//溶解
Shader "KOL/Dissolve" {
	Properties
	{
		_MainTex("Base", 2D) = "white" {}
	_DissolorTex("DissolorTex (RGB)", 2D) = "white" {}
	_RAmount("RAmount", Range(0, 1)) = 0.5

		_DissolorWith("DissolorWith", float) = 0.1//溶解过度宽度
		_DissColor("DissColor", Color) = (1,1,1,1)//溶解颜色
		_Illuminate("Illuminate", Range(0, 4)) = 1
	}
		SubShader
	{
		Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }
		LOD 200
		Pass
	{
		Blend SrcAlpha OneMinusSrcAlpha
		CGPROGRAM
#include "UnityCG.cginc"

		struct appdata
	{
		float4 vertex : POSITION;
		float2 texcoord : TEXCOORD0;
		float2 texcoord1 : TEXCOORD1;
	};
	struct v2f
	{
		float4 pos : POSITION;
		half2 texcoord : TEXCOORD0;
		half2 texcoord1 : TEXCOORD1;
	};

	sampler2D _MainTex;
	float4 _MainTex_ST;
	sampler2D _DissolorTex;
	float4 _DissolorTex_ST;
	half _RAmount;

	half _DissolorWith;
	half4 _DissColor;
	half _Illuminate;

	v2f vert(appdata v)
	{
		v2f o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
		o.texcoord1 = TRANSFORM_TEX(v.texcoord1, _DissolorTex);
		return o;
	}


	half4 frag(v2f i) :COLOR
	{
		half4 mainCol = tex2D(_MainTex,i.texcoord);
		half4 DissolorTexCol = tex2D(_DissolorTex,i.texcoord1);

		half4 col = mainCol;
		half clipVauleR = DissolorTexCol.r - _RAmount;
		if (clipVauleR <= 0)
		{
			if (clipVauleR > -_DissolorWith)
			{
				if (_RAmount != 1)
				{
					//插值颜色过度
					float t = clipVauleR / -_DissolorWith;
					col = lerp(mainCol, _DissColor, t);
				}
				else
				{
					discard;
				}
			}
			else
			{
				discard;
			}

		}

		return col;
	}
#pragma vertex vert
#pragma fragment frag

		ENDCG
	}

	}

}