// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Sprites/Default_Glow"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint1", Color) = (1,1,1,1)
		_Color1("Tint2", Color) = (1,1,1,1)
		_Color2("Tint3", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[PerRendererData][MaterialToggle]_GlowEnable("Glow Enable", float) = 0 // can't use bool property

		_GlowTex1("Glow Texture1", 2D) = "white" {}
		_GlowTex2("Glow Texture2", 2D) = "white" {}
		_GlowTex3("Glow Texture3", 2D) = "white" {}

		_GlowAmount1("Glow Amount1", Range(0,02)) = 0
		_GlowBlipSpeed1("Glow Blip Speed1", Range(0,02)) = 0
		_GlowAmount2("Glow Amount2", Range(0,02)) = 0
		_GlowBlipSpeed2("Glow Blip Speed2", Range(0,02)) = 0
		_GlowAmount3("Glow Amount3", Range(0,02)) = 0
		_GlowBlipSpeed3("Glow Blip Speed3", Range(0,02)) = 0
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			//"RenderType" = "Opaque"
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha
		// Blend BlendOp
		// Blend SrcAlpha OneMinusSrcAlpha //https://docs.unity3d.com/Manual/SL-Blend.html
		//	Blend One One //http://docs.unity3d.com/412/Documentation/Components/SL-Blend.html

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
		//	#pragma multi_compile _ PIXELSNAP_ON
		//	#pragma shader_feature ETC1_EXTERNAL_ALPHA
			#include "UnityCG.cginc"
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    :	COLOR;
				float2 texcoord  : TEXCOORD0;
			};
			
			fixed4 _Color;
			fixed4 _Color1;
			fixed4 _Color2;
			float _GlowAmount1;
			float _GlowBlipSpeed1;
			float _GlowAmount2;
			float _GlowBlipSpeed2;
			float _GlowAmount3;
			float _GlowBlipSpeed3;
			float _GlowEnable;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
			//	OUT.color = IN.color * _Color1;
#ifdef PIXELSNAP_ON
			//	OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			sampler2D _MainTex;
			sampler2D _GlowTex1;
			sampler2D _GlowTex2;
			sampler2D _GlowTex3;
			sampler2D _AlphaTex;

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);

#if ETC1_EXTERNAL_ALPHA
				// get the color from an external texture (usecase: Alpha support for ETC1 on android)
			//	color.a = tex2D (_AlphaTex, uv).r;
#endif //ETC1_EXTERNAL_ALPHA

				return color;
			}

			fixed4 SampleGlowSpriteTexture1(float2 uv)
			{
				fixed4 color = tex2D(_GlowTex1, uv);

#if ETC1_EXTERNAL_ALPHA
				// get the color from an external texture (usecase: Alpha support for ETC1 on android)
			//	color.a = tex2D(_AlphaTex, uv).r;
#endif //ETC1_EXTERNAL_ALPHA

				return color;
			}
			fixed4 SampleGlowSpriteTexture2(float2 uv)
			{
				fixed4 color = tex2D(_GlowTex2, uv);

#if ETC1_EXTERNAL_ALPHA
				// get the color from an external texture (usecase: Alpha support for ETC1 on android)
				//	color.a = tex2D(_AlphaTex, uv).r;
#endif //ETC1_EXTERNAL_ALPHA

				return color;
			}
			fixed4 SampleGlowSpriteTexture3(float2 uv)
			{
				fixed4 color = tex2D(_GlowTex3, uv);

#if ETC1_EXTERNAL_ALPHA
				// get the color from an external texture (usecase: Alpha support for ETC1 on android)
				//	color.a = tex2D(_AlphaTex, uv).r;
#endif //ETC1_EXTERNAL_ALPHA

				return color;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 c = SampleSpriteTexture (IN.texcoord) ;
				c.rgb *= c.a;
				if (_GlowEnable) {
					fixed4 gC1 = SampleGlowSpriteTexture1(IN.texcoord)*_Color;
					fixed4 gC2 = SampleGlowSpriteTexture2(IN.texcoord)*_Color1;
					fixed4 gC3 = SampleGlowSpriteTexture3(IN.texcoord)*_Color2;
					gC1.rgb *= gC1.a;
					gC2.rgb *= gC2.a;
					gC3.rgb *= gC3.a;
					c.rgb += gC1.rgb *_GlowAmount1*abs(cos(_Time.w * 5 * _GlowBlipSpeed1));
					c.rgb += gC2.rgb *_GlowAmount2*abs(cos(_Time.w * 3 * _GlowBlipSpeed2));
					c.rgb += gC3.rgb *_GlowAmount3*abs(cos(_Time.w  * _GlowBlipSpeed3));
				}

			//	gC *= gC.a; // *abs(cos(_Time.w * 5 * _GlowBlipSpeed)); //http://http.developer.nvidia.com/Cg/abs.html
			
				
				return c;
			}
		ENDCG
		}
	}
}
