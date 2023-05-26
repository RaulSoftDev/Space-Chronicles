Shader "Custom/Sprites/Overlay"
{
	Properties
	{
		[PerRendererData]_MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
	}

	SubShader
	{
		Tags
		{ 
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}
		
			Cull Off
			Lighting Off
			ZWrite Off
			Blend One OneMinusSrcAlpha

		GrabPass { }

		Pass
		{
			CGPROGRAM
			
			#pragma vertex ComputeVertex
			#pragma fragment ComputeFragment
			#pragma target 2.0
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
			#include "UnityCG.cginc"

			
			sampler2D _MainTex;
			sampler2D _GrabTexture;
			sampler2D _AlphaTex;
			fixed4 _Color;
			
			struct VertexInput
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
				half2 texcoord : TEXCOORD0;
				float4 screenPos : TEXCOORD1;
				UNITY_VERTEX_OUTPUT_STEREO
			};
			
			VertexOutput ComputeVertex (VertexInput vertexInput)
			{
				VertexOutput vertexOutput;
				UNITY_SETUP_INSTANCE_ID(vertexInput);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(vertexOutput);
				vertexOutput.vertex = UnityObjectToClipPos(vertexInput.vertex);
				vertexOutput.screenPos = vertexOutput.vertex;	
				vertexOutput.texcoord = vertexInput.texcoord;
				vertexOutput.color = vertexInput.color * _Color;
#ifdef PIXELSNAP_ON
				vertexOutput.vertex = UnityPixelSnap(vertexOutput.vertex);
#endif		
				return vertexOutput;
			}

			float Overlay(float base, float top)
			{
				if (base < 0.5) {
					return 2 * base*top;
				}
				else {
					return 1 - 2 * (1 - base) *(1 - top);
				}
			}


			fixed4 SampleSpriteTexture(float2 uv)
			{
				fixed4 color = tex2D(_MainTex, uv);

#if ETC1_EXTERNAL_ALPHA
			//	color.a = tex2D(_AlphaTex, uv).r;
#endif 

				return color;
			}


			fixed4 ComputeFragment (VertexOutput vertexOutput) : SV_Target
			{
				fixed4 color = SampleSpriteTexture(vertexOutput.texcoord) * vertexOutput.color;
				
				float2 grabTexcoord = vertexOutput.screenPos.xy / vertexOutput.screenPos.w; 
				grabTexcoord.x = (grabTexcoord.x + 1.0) * .5;
				grabTexcoord.y = (grabTexcoord.y + 1.0) * .5; 
				#if UNITY_UV_STARTS_AT_TOP
				grabTexcoord.y = 1.0 - grabTexcoord.y;
				#endif
				
				fixed4 grabColor = tex2D(_GrabTexture, grabTexcoord); 

				color.r = Overlay(grabColor.r, color.r);
				color.g = Overlay(grabColor.g, color.g);
				color.b = Overlay(grabColor.b, color.b);
				color.rgb *= color.a;
				return color;
			}
			ENDCG
		}
	}

	Fallback "Sprites/Default"
}
