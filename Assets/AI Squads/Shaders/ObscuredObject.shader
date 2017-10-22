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
		_OccludedColor("Occluded Colour", Color) = (1,1,1,1)
		_RimColor("Rim Colour", Color) = (1,1,1,1)
		_RimWidth("Rim Width", Range(0,1)) = 0
	}

	SubShader
	{
		Pass
		{
			Tags{ "Queue" = "Geometry+1" }//offset render Queue
			ZTest Greater
			ZWrite Off//don't write into depth buffer

			CGPROGRAM
			#pragma vertex vert            
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest

			half4 _OccludedColor;
			half4 _RimColor;
			half _RimWidth;

			struct VertInput
			{
                float4 pos : POSITION;
                float3 normal : NORMAL;
            };

			struct VertOutput
			{
				float4 pos : POSITION;
				half4 colour : COLOR;
			};	

			VertOutput vert(VertInput v)
			{
				VertOutput o;
				o.pos = UnityObjectToClipPos(v.pos);//get clipping
				
                float dot_product = 1 - dot(v.normal, v.pos);
                o.colour = _OccludedColor + smoothstep(1 - _RimWidth, 1.0, dot_product);           
                o.colour *= _RimColor;

				return o;
			}

			half4 frag(VertOutput o) : COLOR
			{	
				half4 c = o.colour;
				return c;
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