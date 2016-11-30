Shader "Custom/fadeShader" {

    Properties {
      _MainTex ("Texture", 2D) = "white" {}
      _Color ("Main Color", Color) = (1,1,1,1)
      _aTex ("Texture", 2D) = "white" {}
    }
    
    SubShader {
    
      Tags { "Queue"="Transparent" "RenderType"="Transparent"  }
      Blend SrcAlpha One
      Cull Off
      
      CGPROGRAM
      
      #pragma surface surf Lambert alpha decal:add vertex:vert 
      
      struct Input {
          float2 uv_MainTex : TEXCOORD0;
          float2 uv2_aTex : TEXCOORD1;
          float2 uv_BumpMap;
          float3 worldPos;
          fixed Alpha;
          float3 customColor;
          float customAlpha;
          float2 uv : TEXCOORD0;
         
      };
      
      void vert (inout appdata_full v, out Input o) {
          UNITY_INITIALIZE_OUTPUT(Input,o);
          o.customColor = abs(v.normal);
          float3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
          o.customColor = float4( dot(viewDir,v.normal), 1.0, 0.0, 1.0 );
          o.customAlpha = (o.customColor.x*o.customColor.x*o.customColor.x);
      }
       float4 _Color;
      sampler2D _MainTex;
      sampler2D _aTex;
      sampler2D _BumpMap;
      
      void surf (Input IN, inout SurfaceOutput o) {
     	 float4 c = tex2D(_MainTex, IN.uv);
//          clip (frac((IN.worldPos.y+IN.worldPos.z*0.1) * 5) - 0.5);
          o.Alpha =  tex2D (_MainTex, IN.uv_MainTex).r * IN.customAlpha ;//tex2D (_MainTex, IN.uv_MainTex).a;//
          o.Albedo = 0;
          o.Emission = (tex2D (_MainTex, IN.uv_MainTex).rgba )*IN.customAlpha * _Color.rgb;// * IN.customAlpha;
//          o.Albedo = IN.customColor;
//          o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
      }
      
      ENDCG
      
    } 
    Fallback "Diffuse"
  }