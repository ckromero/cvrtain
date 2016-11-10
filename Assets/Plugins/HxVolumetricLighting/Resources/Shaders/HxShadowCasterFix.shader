﻿Shader "Hidden/HxShadowCasterFix"
{
	SubShader
	{

		Pass
	{
		Tags{ "LightMode" = "ShadowCaster" }

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile_shadowcaster
#include "UnityCG.cginc"

	struct v2f {
		V2F_SHADOW_CASTER;
	};
	
	v2f vert(appdata_base v)
	{
		v2f o;
		TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
		o.pos = float4(0, 0, 0, 0);

			return o;
	}

	float4 frag(v2f i) : SV_Target
	{
		discard;
		SHADOW_CASTER_FRAGMENT(i)
	}
		ENDCG
	}
	}
}