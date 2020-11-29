// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/GUILine"
{
Properties {
	_displayArea ("Display Area", Vector) = (0,30,1024,768)
}

Category {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	Blend SrcAlpha OneMinusSrcAlpha
	ColorMask RGB
	ZWrite Off Cull Off Lighting Off Fog { Mode Off } 
	
	SubShader {
		Pass {
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_particles
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"
			
			
			
			struct appdata_t {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
			};
			
			float4 _displayArea;
			
			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = v.color;
				
				return o;
			}

			
			fixed4 frag (v2f i) : SV_Target
			{	
				clip((i.vertex.x- _displayArea.x)/ _displayArea.z);
				clip((i.vertex.y- _displayArea.y)/ _displayArea.w);
				clip(1-(i.vertex.x- _displayArea.x)/ _displayArea.z);
				clip(1-(i.vertex.y- _displayArea.y)/ _displayArea.w);
			
				fixed4 col = i.color;
				return col;
			}
			ENDCG 
		}
	}	
}
}