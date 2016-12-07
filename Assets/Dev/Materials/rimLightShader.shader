// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/rimLightShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Position ("Position", Vector) = (0,0,0,0)
		_Data ("X-Emboss Y-Intensity Z-Dropoff",Vector) = (0,0,0,0)
		_LightColor("Light Color", Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue" = "Transparent"}
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
//			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 pos : TEXCOORD1;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float3 pos : TEXCOORD1;
//				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _LightColor;
			float4 _Data;
			float4 _Position;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.pos = mul (unity_ObjectToWorld, v.vertex).xyz;
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
//				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float alpha1 = tex2D(_MainTex, i.uv).a;
				float alpha2 = (1-tex2D(_MainTex, i.uv+float2(0,_Data.x)).a)*(i.uv.y-.3);
				fixed4 col = fixed4(alpha2,alpha2,alpha2,alpha1);
				float dist = min(1.0,((1-distance(_Position,i.pos)*_Data.z))+_Data.y);
				float4 ret = col*dist*_LightColor;
				return fixed4(ret.x,ret.y,ret.z,alpha1);//_LightColor*dist*col;
//				return _LightColor;
			}
			ENDCG
		}
	}
}
