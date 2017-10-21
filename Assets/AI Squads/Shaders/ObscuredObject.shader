Shader "Custom/Obscured Object"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Normal("Normal map", 2D) = "bump" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", 2D) = "white" {}
		_MetallicMult("Metallic Mult", Range(0,1)) = 0
		_OccludedColor("Occluded Color", Color) = (1,1,1,1)
	}

	SubShader
	{
		Pass
		{
			Tags{ "Queue" = "Geometry+1" }//offset render Queue
			ZTest Greater
			ZWrite Off

			CGPROGRAM
			#pragma vertex vert            
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest

			half4 _OccludedColor;


			float4 vert(float4 pos : POSITION) : SV_POSITION
			{
				float4 view_pos = UnityObjectToClipPos(pos);//get clipping
				return view_pos;
			}


			half4 frag(float4 pos : SV_POSITION) : COLOR
			{
				return _OccludedColor;
			}

			ENDCG
		}

		Tags{ "RenderType" = "Opaque" "Queue" = "Geometry+1" }
		LOD 200
		ZWrite On
		ZTest LEqual

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _Normal;
		sampler2D _Metallic;

		struct Input 
		{
			float2 uv_MainTex;
			float2 uv_NormalMap;
			float2 uv_MetallicMap;
		};

		half _Glossiness;
		half _MetallicMult;
		fixed4 _Color;


		void surf(Input IN, inout SurfaceOutputStandard o) 
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;//main colour is tinted texture
			o.Albedo = c.rgb;

			fixed4 m = tex2D(_Metallic, IN.uv_MetallicMap) + _MetallicMult;
			o.Metallic = m.rgb;

			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
			o.Normal = UnpackNormal(tex2D(_Normal, IN.uv_NormalMap));
		}
		ENDCG
	}
	FallBack "Diffuse"
}